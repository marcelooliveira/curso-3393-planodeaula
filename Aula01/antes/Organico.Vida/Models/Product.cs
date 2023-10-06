
// Represents a product
public class Product : BaseEntity
{
    public string Icon { get; set; }
    public string Description { get; set; }
    public decimal UnitPrice { get; set; }

    public Product(int id, string icon, string description, decimal unitPrice)
        : base(id)
    {
        Icon = icon;
        Description = description;
        UnitPrice = unitPrice;
    }

    public decimal Total(int quantity)
    {
        return quantity * UnitPrice;
    }
}
