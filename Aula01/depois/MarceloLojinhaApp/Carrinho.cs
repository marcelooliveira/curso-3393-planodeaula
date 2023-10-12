using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Organico.Vida.Model;

namespace MarceloLojinhaApp
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

                var cart = await CarrinhoCosmosClient.GetCarrinho();

                if (cart != null)
                {
                    response.WriteString(JsonConvert.SerializeObject(cart.items));
                    //response.WriteString("[{\"ProductId\":4,\"Icon\":\"🍊\",\"Description\":\"Tangerina (kg)\",\"UnitPrice\":3.50,\"Quantity\":1,\"Total\":3.50,\"Id\":3},{\"ProductId\":13,\"Icon\":\"🍒\",\"Description\":\"Cereja (kg)\",\"UnitPrice\":3.50,\"Quantity\":3,\"Total\":10.50,\"Id\":2},{\"ProductId\":17,\"Icon\":\"\U0001f965\",\"Description\":\"Coco (un)\",\"UnitPrice\":4.50,\"Quantity\":2,\"Total\":9.00,\"Id\":1}]");
                }

                response.WriteString("");
                
                return response;
            }

            if (req.Method == "POST")
            {
                response = req.CreateResponse(HttpStatusCode.OK);
                response.WriteString("ISTO É UM POST");
                return response;
            }

            response = req.CreateResponse(HttpStatusCode.BadRequest);
            response.WriteString("Tipo Http inválido");
            return response;
        }
    }
}
