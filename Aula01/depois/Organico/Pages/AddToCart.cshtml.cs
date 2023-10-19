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

    //int Id, int ProductId, int Quantity
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

    public IActionResult OnPost()
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
        ECommerceData.Instance.AddCartItem(cartItem);
        return Redirect("/cart");
    }
}
