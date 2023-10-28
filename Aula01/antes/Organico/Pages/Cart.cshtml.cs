using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using Organico.Library.Data;
using Organico.Library.Model;

namespace Organico.Pages;

public class CartModel : PageModel
{
    private readonly ILogger<CartModel> _logger;

	// 1. Adicionar campo com objeto de configuração
	// 4. Novo objeto cliente para acesso cliente de requisições HTTP 
    public List<CartItem> CartItems { get; set; }

	// 2. Novo parâmetro do construtor: objeto de configuração
    public CartModel(ILogger<CartModel> logger)
    {
        _logger = logger;
        // 3. atribuir parâmetro de configuração

    }

    public void OnGet()
    {
    	// 5. Desativar acesso aos dados do carrinho em memória
        CartItems = ECommerceData.Instance.GetCartItems();

        // 6. Obter a URI da Azure Function do carrinho
        
        // 7. Realizar a requisição para a Azure Function do carrinho
        
        // 8. Tratar o resultado JSON do carrinho
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
