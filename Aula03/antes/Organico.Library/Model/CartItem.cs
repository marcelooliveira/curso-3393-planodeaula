using Newtonsoft.Json;

namespace Organico.Library.Model
{
    // Represents a group of a specific product in the cart
    public class CartItem : BaseEntity
    {
        [JsonProperty("productId")]
        public string ProductId { get; set; }

        [JsonProperty("icon")]
        public string Icon { get; set; }
        
        [JsonProperty("description")]
        public string Description { get; set; }
        
        [JsonProperty("unitPrice")]
        public decimal UnitPrice { get; set; }
        
        [JsonProperty("quantity")]
        public int Quantity { get; set; }
        
        [JsonProperty("total")]
        public decimal Total
        {
            get { return Quantity * UnitPrice; }
        }

        public CartItem(string id, string productId, string icon, string description, decimal unitPrice, int quantity)
            : base(id)
        {
            ProductId = productId;
            Icon = icon;
            Description = description;
            UnitPrice = unitPrice;
            Quantity = quantity;
        }
    }
}