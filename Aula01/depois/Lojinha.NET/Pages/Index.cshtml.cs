using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Net.Http;

namespace Lojinha.NET.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;

    // HttpClient lifecycle management best practices:
    // https://learn.microsoft.com/dotnet/fundamentals/networking/http/httpclient-guidelines#recommended-use
    private static HttpClient httpClient = new()
    {
        BaseAddress = new Uri("https://marcelolojinhaapp.azurewebsites.net"),
    };

    public List<CartItem> CartItems { get; set; }

    public IndexModel(ILogger<IndexModel> logger)
    {
        _logger = logger;
    }

    public async Task OnGet()
    {
        using HttpResponseMessage response = await httpClient.GetAsync("api/carrinho");

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

        return Redirect("/");
    }
}
