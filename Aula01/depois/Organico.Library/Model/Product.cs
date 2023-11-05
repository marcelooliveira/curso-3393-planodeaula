using Newtonsoft.Json;

namespace Organico.Library.Model
{
    /// <summary>
    /// Representa um produto
    /// </summary>
    public class Product : BaseEntity
    {
        [JsonProperty("icon")]
        public string Icon { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }
        
        [JsonProperty("unitPrice")]
        public decimal UnitPrice { get; set; }

        public decimal Total(int quantity)
        {
            return quantity * UnitPrice;
        }

        public Product(string id, string icon, string description, decimal unitPrice)
            : base(id)
        {
            Icon = icon;
            Description = description;
            UnitPrice = unitPrice;
        }
    }
}