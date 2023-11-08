using Microsoft.Azure.Cosmos;
using Organico.Library.Model;

namespace Organico.Library
{
    public class CarrinhoCosmosClient : BaseCosmosClient
    {
        protected override string ContainerId { get; set; } = "Carrinho";

        private static CarrinhoCosmosClient? instance;

        private CarrinhoCosmosClient()
        {
        }

        public static CarrinhoCosmosClient Instance()
        {
            if (instance == null)
            {
                instance = new CarrinhoCosmosClient();
                instance.InitializeAsync();
            }
            return instance;
        }

        public async Task<Cart?> Get()
        {
            Cart? result = null;

            using (FeedIterator<Cart?> feedIterator = _container.GetItemQueryIterator<Cart?>(
                "select * from Items i where i.id = 'fulano@detal.com.br'"))
            {
                while (feedIterator.HasMoreResults)
                {
                    FeedResponse<Cart?> response = await feedIterator.ReadNextAsync();
                    foreach (var responseItem in response)
                    {
                        result = responseItem;
                        break;
                    }
                    break;
                }
            }

            return result;
        }

        public async Task<Cart> Post(CartItem cartItem)
        {
            var cart = await Get();
            var oldCartItem = cart.Items.SingleOrDefault(i => i.ProductId == cartItem.Id);
            cart.Items.RemoveAll(i => i.ProductId == cartItem.Id);
            if (cartItem.Quantity > 0)
            {
                cart.Items.Add(cartItem);
            }
            ItemResponse<Cart> itemResponse = await _container.UpsertItemAsync(cart);
            return cart;
        }
    }
}
