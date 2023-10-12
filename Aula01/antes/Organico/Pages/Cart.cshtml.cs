using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Organico.Library.Data;
using Organico.Library.Model;

namespace Organico.Pages;

public class CartModel : PageModel
{
    private readonly ILogger<CartModel> _logger;

    public List<CartItem> CartItems { get; set; }

    public CartModel(ILogger<CartModel> logger)
    {
        _logger = logger;
    }

    public void OnGet()
    {
        CartItems = ECommerceData.Instance.GetCartItems();
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
