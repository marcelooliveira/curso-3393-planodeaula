using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Organico.Library.Data;
using Organico.Library.Model;

namespace Organico.Pages;

public class PaymentModel : PageModel
{
    private readonly ILogger<PaymentModel> _logger;

    public PaymentModel(ILogger<PaymentModel> logger)
    {
        _logger = logger;
    }

    public List<Order> OrdersAwaitingPayment { get; private set; }

    public async Task OnGetAsync()
    {
        OrdersAwaitingPayment = await ECommerceData.Instance.GetOrdersAwaitingPaymentAsync();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (Request.Form.Keys.Contains("approveSubmit"))
        {
            await ECommerceData.Instance.ApprovePaymentAsync();
        }

        if (Request.Form.Keys.Contains("rejectSubmit"))
        {
            await ECommerceData.Instance.RejectPaymentAsync();
        }

        OrdersAwaitingPayment = await ECommerceData.Instance.GetOrdersAwaitingPaymentAsync();
        return Page();
    }
}
