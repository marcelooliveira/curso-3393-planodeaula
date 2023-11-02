using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using Organico.Library.Data;
using Organico.Library.Model;
using System.Reflection;
using System.Text;

namespace Organico.Pages;

public class AddToCartModel : PageModel
{
    private readonly ILogger<AddToCartModel> _logger;
    private readonly IConfiguration _configuration;
    private static HttpClient httpClient = new();

    public CartItem CartItem { get;set; }
    public List<Product> Products { get; set; }

    public AddToCartModel(ILogger<AddToCartModel> logger, IConfiguration configuration)
    {
        _logger = logger;
        _configuration = configuration;
    }

    public void OnGet()
    {
        CartItem = new CartItem(0, 1, "🍇", "Grapes box", 3.50m, 1);
        Products = ECommerceData.Instance.GetProductList();
    }

    public IActionResult OnGetCartItem()
    {
        var products = ECommerceData.Instance.GetProductList();

        var productId = int.Parse(Request.Query["ProductId"].ToString());
        var itemId = int.Parse(Request.Query["Id"].ToString());
        var quantity = int.Parse(Request.Query["Quantity"].ToString());

        var product = products.FirstOrDefault(p => p.Id == productId);

        var newCartItem = new CartItem(itemId, product.Id, product.Icon, product.Description, product.UnitPrice, quantity);
        var json = new JsonResult(newCartItem);
        return json;
    }

    public async Task<IActionResult> OnPostAsync()
    {
        int productId = int.Parse(Request.Form["ProductId"].ToString());
        int quantity = int.Parse(Request.Form["Quantity"].ToString());
        var product = ECommerceData.Instance.GetProduct(productId);
        CartItem cartItem = new CartItem(productId,
            productId,
            product.Icon,
            product.Description,
            product.UnitPrice,
            quantity
        );

        // 1. Desativar acesso estático ao carrinho
        //ECommerceData.Instance.AddCartItem(cartItem);

        // 2. Obter a URI da Azure Function do carrinho
        Uri carrinhoUri = new Uri(_configuration["CarrinhoUrl"]);

        // 3. Serializar o item do carrinho
        var stringContent = new StringContent(JsonConvert.SerializeObject(cartItem),
            Encoding.UTF8, "application/json");

        // 4. Invocar o HTTP Post para adicionar/modificar/remover item do carrinho
        using HttpResponseMessage response = await httpClient.PostAsync(carrinhoUri, stringContent);

        return Redirect("/cart");
    }
}
