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

	// 1. Novo objeto cliente para acesso cliente de requisições HTTP 
    private static HttpClient httpClient = new();

    public List<CartItem> CartItems { get; set; }

    public CartModel(ILogger<CartModel> logger, IConfiguration configuration)
    {
        _logger = logger;
        _configuration = configuration;
    }

    public async Task OnGetAsync()
    {
    	// 2. Desativar acesso aos dados do carrinho em memória
        //CartItems = ECommerceData.Instance.GetCartItems();

        // 3. Obter a URI da Azure Function do carrinho
        Uri carrinhoUri = new Uri(_configuration["CarrinhoUrl"]);

        // 4. Realizar a requisição para a Azure Function do carrinho
        using HttpResponseMessage response = await httpClient.GetAsync(carrinhoUri);

        // 5. Tratar o resultado JSON do carrinho
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
