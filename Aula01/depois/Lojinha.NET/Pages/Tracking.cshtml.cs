using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Lojinha.NET.Pages;

public class TrackingModel : PageModel
{
    private readonly ILogger<TrackingModel> _logger;

    public TrackingModel(ILogger<TrackingModel> logger)
    {
        _logger = logger;
    }

    public Queue<Order> OrdersForDelivery { get; private set; }
    public Queue<Order> OrdersRejected { get; private set; }

    public void OnGet()
    {
        OrdersForDelivery = ECommerceData.Instance.GetOrdersForDelivery();
        OrdersRejected = ECommerceData.Instance.GetOrdersRejected();
    }
}
