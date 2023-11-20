using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Organico.Library.Model;
using Organico.Library;

namespace Organico.FunctionApp
{
    public class Pedido
    {
        private readonly ILogger _logger;

        public Pedido(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<Pedido>();
        }

        [Function("Pedido")]
        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequestData req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            var response = req.CreateResponse(HttpStatusCode.OK);
            //response.Headers.Add("Content-Type", "text/plain; charset=utf-8");

            //response.WriteString("Welcome to Azure Functions!");

            response.Headers.Add("Content-Type", "application/json; charset=utf-8");


            var cosmosClient = PedidosCosmosClient.Instance();


            if (req.Method == "GET")

            {

                //ler pedidos

                var orders = await cosmosClient.GetList();

                orders = orders.OrderByDescending(o => o.Id).ToList();

                response.WriteString(JsonConvert.SerializeObject(orders));

            }


            if (req.Method == "POST")

            {

                //gravar pedido

                var body = req.Body;

                var content = await new StreamReader(body).ReadToEndAsync();

                var order = JsonConvert.DeserializeObject<Order>(content);


                await cosmosClient.Post(order);

            }

            return response;
        }
    }
}

