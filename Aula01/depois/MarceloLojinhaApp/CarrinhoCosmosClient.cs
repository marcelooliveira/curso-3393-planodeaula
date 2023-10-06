using Microsoft.Azure.Cosmos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarceloLojinhaApp
{
    public class CarrinhoCosmosClient
    {

        public static async Task<string> GetCarrinho()
        {
            CosmosClient client = new CosmosClient(Environment.GetEnvironmentVariable("CosmosDB_URI"), Environment.GetEnvironmentVariable("CosmosDB_KEY"));
            Database database = await client.CreateDatabaseIfNotExistsAsync("lojinha");
            Container container = await database.CreateContainerIfNotExistsAsync(
                "Carrinho",
                "/myPartitionKey",
                400);

            //// Create an item
            //dynamic testItem = new { id = "MyTestItemId", partitionKeyPath = "MyTestPkValue", details = "it's working", status = "done" };
            //ItemResponse<dynamic> createResponse = await container.CreateItemAsync(testItem);

            string result = string.Empty;

            // Query for an item
            using (FeedIterator<dynamic> feedIterator = container.GetItemQueryIterator<dynamic>(
                "select * from T where T.id = 'fulano@detal.com.br'"))
            {
                while (feedIterator.HasMoreResults)
                {
                    FeedResponse<dynamic> response = await feedIterator.ReadNextAsync();
                    foreach (var item in response)
                    {
                        result = item;
                    }
                    break;
                }
            }

            return result;
        }
    }
}
