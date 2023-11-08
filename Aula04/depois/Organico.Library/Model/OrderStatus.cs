namespace Organico.Library.Model
{
    public enum OrderStatus : byte
    {
        AwaitingPayment = 1,
        ForDelivery = 2,
        Rejected = 3
    }
}