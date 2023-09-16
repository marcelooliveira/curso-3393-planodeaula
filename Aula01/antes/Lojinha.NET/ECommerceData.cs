using System;
using System.Collections.Generic;
using System.Linq;

namespace Lojinha.NET
{
    // Wrapper for Hazelcast Client data operations
    public class ECommerceData : BaseECommerceData
    {
        // Wraps Hazelcast Client data operations
        private Dictionary<int, CartItem> _cartItems;
        private Queue<Order> _ordersAwaitingPayment;
        private Queue<Order> _ordersForDelivery;
        private Queue<Order> _ordersRejected;
        private int _maxOrderId;

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

        public List<CartItem> GetCartItems()
        {
            var items = _cartItems.Values.ToList();
            items.Sort((item1, item2) => item1.ProductId.CompareTo(item2.ProductId));
            return items;
        }

        // Adiciona um item ao carrinho de compras
        public void AddCartItem(CartItem cartItem)
        {
            var products = GetProductList();
            var product = products.FirstOrDefault(p => p.Id == cartItem.ProductId);

            if (product != null)
            {
                var newCartItem = new CartItem(cartItem.Id, product.Id, product.Icon, product.Description, product.UnitPrice, cartItem.Quantity);
                _cartItems[newCartItem.ProductId] = newCartItem;
            }
        }

        // Obtain orders awaiting payment
        public List<Order> GetOrdersAwaitingPayment()
        {
            var orders = _ordersAwaitingPayment.ToList();
            orders.Sort((order1, order2) => order2.Id.CompareTo(order1.Id));
            return orders;
        }

        // Obtain orders ready for delivery
        public Queue<Order> GetOrdersForDelivery()
        {
            return _ordersForDelivery;
        }

        // Obtain orders with rejected payment
        public Queue<Order> GetOrdersRejected()
        {
            return _ordersRejected;
        }

        // Move order from awaiting payment to ready for delivery
        public void ApprovePayment()
        {
            var order = _ordersAwaitingPayment.Dequeue();
            _ordersForDelivery.Enqueue(order);
        }

        // Move order from awaiting payment to payment rejected
        public void RejectPayment()
        {
            var order = _ordersAwaitingPayment.Dequeue();
            _ordersRejected.Enqueue(order);
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
    }
}
