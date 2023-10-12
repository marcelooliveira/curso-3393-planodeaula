namespace Organico.Vida.Model
{
    public class Cart
    {
        public string id { get; set; }

        public List<CartItem> items { get; set; } = new List<CartItem>();
    }
}