
// Representa um pedido
public class Order : BaseEntity
{
    public DateTime Placement { get; set; }
    public int ItemCount { get; set; }
    public decimal Total { get; set; }

    public Order(int id, DateTime placement, int itemCount, decimal total)
        : base(id)
    {
        Placement = placement;
        ItemCount = itemCount;
        Total = total;
    }
}
