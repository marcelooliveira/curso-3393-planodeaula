using Microsoft.Azure.Cosmos;
using Organico.Library.Model;

namespace Organico.Library
{
    public class PedidosCosmosClient : BaseCosmosClient
    {
        protected override string ContainerId { get; set; } = "Pedidos";

        private static PedidosCosmosClient? instance;

        private PedidosCosmosClient()
        {
        }

        public static PedidosCosmosClient Instance()
        {
            if (instance == null)
            {
                instance = new PedidosCosmosClient();
                instance.InitializeAsync();
            }
            return instance;
        }

        public async Task<List<Order>> GetList()
        {
            var result = new List<Order>();

            using (FeedIterator<Order?> feedIterator = _container.GetItemQueryIterator<Order?>(
                "select * from Items i"))
            {
                while (feedIterator.HasMoreResults)
                {
                    FeedResponse<Order?> response = await feedIterator.ReadNextAsync();
                    foreach (var responseItem in response)
                    {
                        result.Add(responseItem);
                    }
                }
            }

            return result;
        }

        public async Task Post(Order order)
        {
            var itemResponse = await _container.UpsertItemAsync(order);
        }
    }
}
