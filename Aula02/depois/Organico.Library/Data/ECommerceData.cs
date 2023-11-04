using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Organico.Library.Model;
using System.Text;

namespace Organico.Library.Data
{
    public class ECommerceData : BaseECommerceData, IECommerceData
    {
        private Dictionary<int, CartItem> _cartItems;
        private Queue<Order> _ordersAwaitingPayment;
        private Queue<Order> _ordersForDelivery;
        private Queue<Order> _ordersRejected;
        private int _maxOrderId;

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
            _cartItems = new Dictionary<int, CartItem>
            {
                { 17, new CartItem(1, 17, "🥥", "Coco (un)", 4.50m, 2) },
                { 13, new CartItem(2, 13, "🍒", "Cereja (kg)", 3.50m, 3) },
                { 4, new CartItem(3, 4, "🍊", "Tangerina (kg)", 3.50m, 1) }
            };

            _ordersAwaitingPayment = new Queue<Order>(new[]
            {
                new Order(1006, new DateTime(2021, 10, 11, 3, 3, 0), 7, 70.00m),
                new Order(1007, new DateTime(2021, 10, 12, 17, 17, 0), 2, 20.00m),
                new Order(1008, new DateTime(2021, 10, 13, 21, 9, 0), 5, 50.00m)
            });

            _ordersForDelivery = new Queue<Order>(new[]
            {
                new Order(1002, new DateTime(2021, 10, 2, 23, 3, 0), 5, 50.00m),
                new Order(1003, new DateTime(2021, 10, 9, 7, 7, 0), 3, 30.00m)
            });

            _ordersRejected = new Queue<Order>(new[]
            {
                new Order(1001, new DateTime(2021, 10, 1, 18, 32, 0), 5, 35.00m),
                new Order(1004, new DateTime(2021, 10, 3, 17, 17, 0), 2, 24.00m),
                new Order(1005, new DateTime(2021, 10, 7, 9, 12, 0), 4, 17.00m)
            });

            _maxOrderId = 1008;
        }

        public void SetConfiguration(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<List<CartItem>> GetCartItems()
        {
            //var items = _cartItems.Values.ToList();
            //items.Sort((item1, item2) => item1.ProductId.CompareTo(item2.ProductId));
            //return items;

            // 2. Obter a URI da Azure Function do carrinho
            Uri carrinhoUri = new Uri(_configuration["CarrinhoUrl"]);

            // 3. Realizar a requisição para a Azure Function do carrinho
            using HttpResponseMessage response = await httpClient.GetAsync(carrinhoUri);

            // 4. Tratar o resultado JSON do carrinho
            var jsonResponse = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<CartItem>>(jsonResponse);
        }
        
        // Adiciona um item ao carrinho de compras
        public async Task AddCartItem(CartItem cartItem)
        {
            var products = GetProductList();
            var product = products.FirstOrDefault(p => p.Id == cartItem.ProductId);

            if (product != null)
            {
                var newCartItem = new CartItem(cartItem.Id, product.Id, product.Icon, product.Description, product.UnitPrice, cartItem.Quantity);
                _cartItems[newCartItem.ProductId] = newCartItem;
            }

            // 1. Obter a URI da Azure Function do carrinho
            Uri carrinhoUri = new Uri(_configuration["CarrinhoUrl"]);

            // 2. Serializar o item do carrinho
            var stringContent = new StringContent(JsonConvert.SerializeObject(cartItem),
                Encoding.UTF8, "application/json");

            // 3. Invocar o HTTP Post para adicionar/modificar/remover item do carrinho
            using HttpResponseMessage response = await httpClient.PostAsync(carrinhoUri, stringContent);
        }

        // Cria um novo pedido e limpa o carrinho de compras
        public void CheckOut()
        {
            _maxOrderId++;
            var orderId = _maxOrderId;
            var total = _cartItems.Values.Sum(item => item.Quantity * item.UnitPrice);
            var order = new Order(orderId, DateTime.Now, _cartItems.Count, total);
            _ordersAwaitingPayment.Enqueue(order);
            _cartItems.Clear();
        }

        // Obtém pedidos aguardando pagamento
        public List<Order> GetOrdersAwaitingPayment()
        {
            var orders = _ordersAwaitingPayment.ToList();
            orders.Sort((order1, order2) => order2.Id.CompareTo(order1.Id));
            return orders;
        }

        // Obtém pedidos prontos para entrega
        public Queue<Order> GetOrdersForDelivery()
        {
            return _ordersForDelivery;
        }

        // Obtém pedidos com pagamento recusado
        public Queue<Order> GetOrdersRejected()
        {
            return _ordersRejected;
        }

        // Move pedidos aguardando pagamento para prontos para entrega
        public void ApprovePayment()
        {
            var order = _ordersAwaitingPayment.Dequeue();
            _ordersForDelivery.Enqueue(order);
        }

        // Move pedidos aguardando pagamento para pagamento recusado
        public void RejectPayment()
        {
            var order = _ordersAwaitingPayment.Dequeue();
            _ordersRejected.Enqueue(order);
        }
    }
}