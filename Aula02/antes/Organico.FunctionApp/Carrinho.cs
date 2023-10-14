using System.Net;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Organico.Library;
using Organico.Library.Model;

namespace Organico.FunctionApp
{
    public class Carrinho
    {
        private readonly ILogger _logger;

        public Carrinho(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<Carrinho>();
        }

        [Function("Carrinho")]
        public async Task<HttpResponseData> RunAsync([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequestData req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            HttpResponseData response;

            if (req.Method == "GET")
            {
                response = req.CreateResponse(HttpStatusCode.OK);
                response.Headers.Add("Content-Type", "application/json; charset=utf-8");

                var cosmosClient = await CarrinhoCosmosClient.CreateAsync();
                var cart = await cosmosClient.Get();

                if (cart != null)
                {
                    response.WriteString(JsonConvert.SerializeObject(cart.items));
                }

                response.WriteString("");

                return response;
            }

            if (req.Method == "POST")
            {
                response = req.CreateResponse(HttpStatusCode.OK);
                var content = await new StreamReader(req.Body).ReadToEndAsync();
                CartItem cartItem = JsonConvert.DeserializeObject<CartItem>(content);

                var cosmosClient = await CarrinhoCosmosClient.CreateAsync();
                await cosmosClient.Post(cartItem);
                return response;
            }

            response = req.CreateResponse(HttpStatusCode.BadRequest);
            response.WriteString("Tipo Http inválido");
            return response;
        }
    }
}
