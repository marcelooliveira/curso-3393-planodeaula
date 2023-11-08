using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Organico.Library.Data;
using Organico.Library.Model;

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
        CartItem = new CartItem("0", "1", "🍇", "Grapes box", 3.50m, 1);
        Products = ECommerceData.Instance.GetProductList();
    }

    public IActionResult OnGetCartItem()
    {
        var products = ECommerceData.Instance.GetProductList();

        var productId = Request.Query["ProductId"].ToString();
        var itemId = Request.Query["Id"].ToString();
        var quantity = int.Parse(Request.Query["Quantity"].ToString());

        var product = products.FirstOrDefault(p => p.Id == productId);

        var newCartItem = new CartItem(itemId, product.Id, product.Icon, product.Description, product.UnitPrice, quantity);
        var json = new JsonResult(newCartItem);
        return json;
    }

    public async Task<IActionResult> OnPostAsync()
    {
        string productId = Request.Form["ProductId"].ToString();
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
