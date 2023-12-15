using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Organico.Library.Model;

namespace Organico.FunctionApp
{
    public class CarrinhoComOutputBinding
    {
        private readonly ILogger _logger;

        public CarrinhoComOutputBinding(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<CarrinhoComOutputBinding>();
        }

        [Function("CarrinhoComOutputBinding")]
        [CosmosDBOutput("organico", "CarrinhoComOutputBinding",
            Connection = "CosmosDBConnection",
            CreateIfNotExists = true,
            PartitionKey = "/id")]
        public dynamic Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequestData req)
        {
            var content = new StreamReader(req.Body).ReadToEnd();
            var cartItem = JsonConvert.DeserializeObject<CartItem>(content);
            return new 
            {
                id = cartItem.Id,
                productId = cartItem.ProductId,
                icon = cartItem.Icon,
                quantity = cartItem.Quantity,
                unitPrice = cartItem.UnitPrice,
            };
        }
    }
}
