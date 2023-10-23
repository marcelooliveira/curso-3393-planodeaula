using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using Organico.Library.Data;
using Organico.Library.Model;

namespace Organico.Pages;

public class CartModel : PageModel
{
    private readonly ILogger<CartModel> _logger;

    private readonly IConfiguration _configuration;

    private static HttpClient httpClient = new();

    public List<CartItem> CartItems { get; set; }

    public CartModel(ILogger<CartModel> logger, IConfiguration configuration)
    {
        _logger = logger;
        _configuration = configuration;
    }

    public async Task OnGetAsync()
    {
        //CartItems = ECommerceData.Instance.GetCartItems();

        // Obtém a URI da Azure Function do carrinho
        Uri carrinhoUri = new Uri(_configuration["CarrinhoUrl"]);

        // Realiza a requisição para a Azure Function do carrinho
        using HttpResponseMessage response = await httpClient.GetAsync(carrinhoUri);

        // Tratar o resultado JSON do carrinho
        var jsonResponse = await response.Content.ReadAsStringAsync();
        CartItems = JsonConvert.DeserializeObject<List<CartItem>>(jsonResponse);
    }

    public IActionResult OnPost()
    {
        if (Request.Form.Keys.Contains("addToCartSubmit"))
        {
            return Redirect("/addToCart");
        }

        if (Request.Form.Keys.Contains("checkoutSubmit"))
        {
            ECommerceData.Instance.CheckOut();
        }

        return Redirect("/cart");
    }
}
