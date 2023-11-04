using Newtonsoft.Json;

namespace Organico.Library.Model
{
    /// <summary>
    /// Representa um pedido
    /// </summary>
    public class Order : BaseEntity
    {
        [JsonProperty("customer")]
        public string Customer { get; set; } = "fulano@detal.com.br";
        
        [JsonProperty("placement")]
        public DateTime Placement { get; set; }
        
        [JsonProperty("itemCount")]
        public int ItemCount { get; set; }
        
        [JsonProperty("total")]
        public decimal Total { get; set; }
        
        [JsonProperty("status")]
        public byte Status { get; set; }

        public Order(string id, DateTime placement, int itemCount, decimal total, byte status = (byte)OrderStatus.AwaitingPayment)
            : base(id)
        {
            Placement = placement;
            ItemCount = itemCount;
            Total = total;
            Status = status;
        }
    }
}