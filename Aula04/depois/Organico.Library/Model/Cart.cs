using Newtonsoft.Json;

namespace Organico.Library.Model
{
    /// <summary>
    /// Representa um carrinho de compras
    /// </summary>
    public class Cart : BaseEntity
    {
        [JsonProperty("items")]
        public List<CartItem> Items { get; set; } = new List<CartItem>();

        public Cart(string id) : base(id)
        {
        }
    }
}