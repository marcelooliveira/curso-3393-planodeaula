using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using Organico.Library.Data;
using Organico.Library.Model;

namespace Organico.Pages;

public class CartModel : PageModel
{
    private readonly IConfiguration _configuration;

    private static HttpClient httpClient = new();

    public List<CartItem> CartItems { get; set; }

    public CartModel(ILogger<CartModel> logger, IConfiguration configuration)
    {
        _logger = logger;
        _configuration = configuration;
    }

    public async Task OnGet()
    {
        Uri carrinhoUri = new Uri(new Uri($"{_configuration["FunctionAppUrl"]}"), "/api/carrinho");
        using HttpResponseMessage response = await httpClient.GetAsync(carrinhoUri);

        var jsonResponse = await response.Content.ReadAsStringAsync();
        CartItems = JsonConvert.DeserializeObject<List<CartItem>>(jsonResponse)!;
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
