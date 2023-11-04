namespace Organico.Library.Model
{
    /// <summary>
    /// Status do pedido
    /// </summary>
    public enum OrderStatus : byte
    {
        // Aguardando pagamento
        AwaitingPayment = 1,
        // Aguardando Entrega
        ForDelivery = 2,
        // Pagamento Recusado
        Rejected = 3
    }
}