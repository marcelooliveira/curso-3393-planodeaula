using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Lojinha.NET.Pages;

public class PaymentModel : PageModel
{
    private readonly ILogger<PaymentModel> _logger;

    public PaymentModel(ILogger<PaymentModel> logger)
    {
        _logger = logger;
    }

    public List<Order> OrdersAwaitingPayment { get; private set; }

    public void OnGet()
    {
        OrdersAwaitingPayment = ECommerceData.Instance.GetOrdersAwaitingPayment();
    }

    public IActionResult OnPost()
    {
        if (Request.Form.Keys.Contains("approveSubmit"))
        {
            ECommerceData.Instance.ApprovePayment();
        }

        if (Request.Form.Keys.Contains("rejectSubmit"))
        {
            ECommerceData.Instance.RejectPayment();
        }

        OrdersAwaitingPayment = ECommerceData.Instance.GetOrdersAwaitingPayment();
        return Page();
    }
}
