using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Organico.Library.Model;
using System.Text;

namespace Organico.Library.Data
{
    public class ECommerceData : BaseECommerceData, IECommerceData
    {
        private List<Order> _orders = new List<Order>();
        private Dictionary<string, CartItem> _cartItems;

        private IConfiguration _configuration;

        // 1. Novo objeto cliente para acesso cliente de requisições HTTP
        private static HttpClient httpClient = new();

        private static ECommerceData? instance;
        public static ECommerceData Instance
        {
            get
            {
                instance ??= new ECommerceData();
                return instance;
            }
        }

        private ECommerceData()
        {
            _cartItems = new Dictionary<string, CartItem>();
            //_cartItems = new Dictionary<int, CartItem>
            //{
            //    { 17, new CartItem(1, 17, "🥥", "Coco (un)", 4.50m, 2) },
            //    { 13, new CartItem(2, 13, "🍒", "Cereja (kg)", 3.50m, 3) },
            //    { 4, new CartItem(3, 4, "🍊", "Tangerina (kg)", 3.50m, 1) }
            //};

            _orders = new List<Order>
            {
                new Order("1006", new DateTime(2021, 10, 11, 3, 3, 0), 7, 70.00m),
                new Order("1007", new DateTime(2021, 10, 12, 17, 17, 0), 2, 20.00m),
                new Order("1008", new DateTime(2021, 10, 13, 21, 9, 0), 5, 50.00m),
                new Order("1002", new DateTime(2021, 10, 2, 23, 3, 0), 5, 50.00m, (byte)OrderStatus.ForDelivery),
                new Order("1003", new DateTime(2021, 10, 9, 7, 7, 0), 3, 30.00m, (byte)OrderStatus.ForDelivery),
                new Order("1001", new DateTime(2021, 10, 1, 18, 32, 0), 5, 35.00m, (byte)OrderStatus.Rejected),
                new Order("1004", new DateTime(2021, 10, 3, 17, 17, 0), 2, 24.00m, (byte)OrderStatus.Rejected),
                new Order("1005", new DateTime(2021, 10, 7, 9, 12, 0), 4, 17.00m, (byte)OrderStatus.Rejected)
            };
        }

        public void SetConfiguration(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<List<CartItem>> GetCartItems()
        {
            // 1. Comentar o fluxo atual de listagem de itens
            //var items = _cartItems.Values.ToList();
            //items.Sort((item1, item2) => item1.ProductId.CompareTo(item2.ProductId));
            //return items;

            // 2. Obter a URI da Azure Function do carrinho
            Uri carrinhoUri = new Uri(_configuration["CarrinhoUrl"]);

            // 3. Realizar a requisição para a Azure Function do carrinho
            using HttpResponseMessage response = await httpClient.GetAsync(carrinhoUri);

            // 4. Tratar o resultado JSON do carrinho
            var jsonResponse = await response.Content.ReadAsStringAsync();
            var items = JsonConvert.DeserializeObject<List<CartItem>>(jsonResponse);
            _cartItems.Clear();
            foreach (var item in items)
            {
                _cartItems[item.ProductId] = item;
            }
            return items;
        }
        
        // Adiciona um item ao carrinho de compras
        public async Task AddCartItem(CartItem cartItem)
        {
            var products = GetProductList();
            var product = products.FirstOrDefault(p => p.Id == cartItem.ProductId);

            // 1. Comentar o fluxo atual
            //if (product != null)
            //{
            //    var newCartItem = new CartItem(cartItem.Id, product.Id, product.Icon, product.Description, product.UnitPrice, cartItem.Quantity);
            //    _cartItems[newCartItem.ProductId] = newCartItem;
            //}

            // 2. Obter a URI da Azure Function do carrinho
            Uri carrinhoUri = new Uri(_configuration["CarrinhoUrl"]);

            // 3. Serializar o item do carrinho
            var stringContent = new StringContent(JsonConvert.SerializeObject(cartItem),
                Encoding.UTF8, "application/json");

            // 4. Invocar o HTTP Post para adicionar/modificar/remover item do carrinho
            using HttpResponseMessage response = await httpClient.PostAsync(carrinhoUri, stringContent);
        }

        // Cria um novo pedido e limpa o carrinho de compras
        public async Task CheckOutAsync()
        {
            _orders = GetOrders();
            var orderId = _orders.Any() ? _orders.Max(o => int.Parse(o.Id) + 1) : 0;
            int itemCount = _cartItems.Count;
            var total = _cartItems.Values.Sum(item => item.Quantity * item.UnitPrice);
            var order = new Order(orderId.ToString(), DateTime.Now, itemCount, total);

            SaveOrder(order);

            // 5. Limpar o carrinho
            foreach (var item in _cartItems)
            {
                var cartItem = item.Value;
                cartItem.Quantity = 0;
                await AddCartItem(cartItem);
            }

            _cartItems.Clear();
        }
        
        public List<Order> GetOrdersAwaitingPayment()
        {
            return GetFilteredOrders(OrderStatus.AwaitingPayment);
        }

        public List<Order> GetOrdersForDelivery()
        {
            return GetFilteredOrders(OrderStatus.ForDelivery);
        }

        public List<Order> GetOrdersRejected()
        {
            return GetFilteredOrders(OrderStatus.Rejected);
        }

        public void ApprovePayment()
        {
            var orders = GetFilteredOrders(OrderStatus.AwaitingPayment);
            if (orders.Any())
            {
                var order = orders.OrderBy(o => int.Parse(o.Id)).First();
                order.Status = (int)OrderStatus.ForDelivery;
                SaveOrder(order);
            }
        }

        public void RejectPayment()
        {
            var orders = GetFilteredOrders(OrderStatus.AwaitingPayment);
            if (orders.Any())
            {
                var order = orders.OrderBy(o => int.Parse(o.Id)).First();
                order.Status = (int)OrderStatus.Rejected;
                SaveOrder(order);
            }
        }

        private List<Order> GetFilteredOrders(OrderStatus filterStatus)
        {
            _orders = GetOrders();
            return _orders.Where(o => o.Status == (byte)filterStatus).ToList();
        }

        private List<Order> GetOrders()
        {
            var orders = _orders.ToList();
            orders.Sort((order1, order2) => order2.Id.CompareTo(order1.Id));
            return orders;
        }

        private void SaveOrder(Order order)
        {
            var existingOrder = _orders.Where(o => o.Id == order.Id).SingleOrDefault();
            if (existingOrder != null)
            {
                _orders.Remove(existingOrder);
            }

            _orders.Add(order);
        }
    }
}
