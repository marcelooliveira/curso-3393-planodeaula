using Microsoft.Azure.Cosmos;
using Organico.Library.Model;

namespace Organico.Library
{
    public class CarrinhoCosmosClient
    {
        private CosmosClient _client;
        private Database _database;
        private Container _container;

        private CarrinhoCosmosClient()
        {
        }

        public static Task<CarrinhoCosmosClient> CreateAsync()
        {
            var instance = new CarrinhoCosmosClient();
            return instance.InitializeAsync();
        }

        public async Task<Cart?> GetCarrinho()
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

        private async Task<CarrinhoCosmosClient> InitializeAsync()
        {
            this._client = new CosmosClient(Environment.GetEnvironmentVariable("CosmosDB_URI"), Environment.GetEnvironmentVariable("CosmosDB_KEY"));
            this._database = await _client.CreateDatabaseIfNotExistsAsync("organico");
            this._container = await _database.CreateContainerIfNotExistsAsync(
                "Carrinho",
                "/id",
                400);
            return this;
        }
    }
}
