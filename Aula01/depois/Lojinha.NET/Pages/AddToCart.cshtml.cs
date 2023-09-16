using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace Lojinha.NET.Pages;

public class AddToCartModel : PageModel
{
    private readonly ILogger<AddToCartModel> _logger;
    public CartItem CartItem { get;set; }
    public List<Product> Products { get; set; }

    public AddToCartModel(ILogger<AddToCartModel> logger)
    {
        _logger = logger;
    }

    public void OnGet()
    {
        CartItem = new CartItem(0, 1, "🍇", "Grapes box", 3.50m, 1);
        Products = ECommerceData.Instance.GetProductList();
    }

    //int Id, int ProductId, int Quantity
    public IActionResult OnGetCartItem()
    {
        var products = ECommerceData.Instance.GetProductList();

        var productId = int.Parse(Request.Query["ProductId"].ToString());
        var itemId = int.Parse(Request.Query["Id"].ToString());
        var quantity = int.Parse(Request.Query["Quantity"].ToString());

        var product = products.FirstOrDefault(p => p.Id == productId);

        var newCartItem = new CartItem(itemId, product.Id, product.Icon, product.Description, product.UnitPrice, quantity);
        return new JsonResult(newCartItem);
    }

    public IActionResult OnPost()
    {
        CartItem = new CartItem(0, 1, "🍇", "Grapes box", 3.50m, 1);
        ECommerceData.Instance.AddCartItem(
            new CartItem(0, 
            int.Parse(Request.Form["ProductId"].ToString()), 
            "", 
            "", 
            0, 
            int.Parse(Request.Form["Quantity"].ToString())
        ));
        return Redirect("/");
    }
}
