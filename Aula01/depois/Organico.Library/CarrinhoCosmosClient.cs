using Microsoft.Azure.Cosmos;
using Organico.Library.Model;

namespace MarceloLojinhaApp
{
    public class CarrinhoCosmosClient
    {

        public static async Task<Cart?> GetCarrinho()
        {
            CosmosClient client = new CosmosClient(Environment.GetEnvironmentVariable("CosmosDB_URI"), Environment.GetEnvironmentVariable("CosmosDB_KEY"));
            Database database = await client.CreateDatabaseIfNotExistsAsync("lojinha");
            Container container = await database.CreateContainerIfNotExistsAsync(
                "Carrinho",
                "/id",
                400);

            Cart? result = null;

            // Query for an item
            using (FeedIterator<Cart?> feedIterator = container.GetItemQueryIterator<Cart?>(
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
    }
}
