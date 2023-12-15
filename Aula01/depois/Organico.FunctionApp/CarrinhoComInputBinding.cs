using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Organico.Library.Model;

namespace Organico.FunctionApp
{
    public class CarrinhoComInputBinding
    {
        private readonly ILogger _logger;

        public CarrinhoComInputBinding(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<CarrinhoComInputBinding>();
        }

        [Function("CarrinhoComInputBinding")]
        public HttpResponseData Run([HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequestData req,
        [CosmosDBInput(
            databaseName: "organico", containerName: "Carrinho", Connection  = "CosmosDBConnection"
            )] List<Cart> carrinhos)
        {
            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "application/json; charset=utf-8");
            response.WriteString(JsonConvert.SerializeObject(carrinhos.First().Items));

            return response;
        }
    }
}
