using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Organico.Library.Data;
using Organico.Library.Model;

namespace Organico.Pages;

public class TrackingModel : PageModel
{
    private readonly ILogger<TrackingModel> _logger;

    public TrackingModel(ILogger<TrackingModel> logger)
    {
        _logger = logger;
    }

    public List<Order> OrdersForDelivery { get; private set; }
    public List<Order> OrdersRejected { get; private set; }

    public async Task OnGetAsync()
    {
        OrdersForDelivery = await ECommerceData.Instance.GetOrdersForDeliveryAsync();
        OrdersRejected = await ECommerceData.Instance.GetOrdersRejectedAsync();
    }
}
