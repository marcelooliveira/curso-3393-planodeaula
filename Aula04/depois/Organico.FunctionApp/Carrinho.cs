using System.Net;
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
        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequestData req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            var response = req.CreateResponse(HttpStatusCode.OK);

            // 1. Comentar a resposta padrão
            //response.Headers.Add("Content-Type", "text/plain; charset=utf-8");

            //response.WriteString("Welcome to Azure Functions!");

            // 2. Acessar o objeto cliente de acesso a dados do Carrinho de Compras
            var cosmosClient = CarrinhoCosmosClient.Instance();

            // 3. Identificar quando a requisição é GET ou POST 
            if (req.Method == "GET")
            {
                // 4. Adicionar o cabeçalho para a resposta JSON
                response.Headers.Add("Content-Type", "application/json; charset=utf-8");

                // 5. ler carrinho de compras da nuvem
                var cart = await cosmosClient.Get();
                response.WriteString(JsonConvert.SerializeObject(cart.Items));
            }

            if (req.Method == "POST")
            {
                // 6. inserir/modificar/remover item do carrinho
                var content = await new StreamReader(req.Body).ReadToEndAsync();
                var cartItem = JsonConvert.DeserializeObject<CartItem>(content);

                await cosmosClient.Post(cartItem);
            }

            return response;
        }
    }
}
