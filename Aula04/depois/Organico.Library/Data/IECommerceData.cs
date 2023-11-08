using Microsoft.Extensions.Configuration;
using Organico.Library.Model;

namespace Organico.Library.Data
{
    public interface IECommerceData
    {
        /// <summary>
        /// Define o objeto de configuração
        /// </summary>
        /// <param name="cartItem">Objeto com as configurações do projeto</param>
        void SetConfiguration(IConfiguration configuration);
        
        /// <summary>
        /// Obtém os itens do carrinho de compras
        /// </summary>
        /// <returns>A lista do carrinho de compras</returns>
        Task<List<CartItem>> GetCartItems();

        /// <summary>
        /// Adiciona, modifica ou remove um item do carrinho de compras
        /// </summary>
        /// <param name="cartItem">Item do carrinho</param>
        Task AddCartItem(CartItem cartItem);

        /// <summary>
        /// Cria um novo pedido e limpa o carrinho de compras
        /// </summary>
        Task CheckOutAsync();

        /// <summary>
        /// Mover pedido de de "aguardando pagamento" para "pronto para entrega"
        /// </summary>
        Task ApprovePaymentAsync();

        /// <summary>
        /// Pedidos aguardando pagamento
        /// </summary>
        Task<List<Order>> GetOrdersAwaitingPayment();

        /// <summary>
        /// Pedidos prontos para entrega
        /// </summary>
        /// <returns>Uma fila com os pedidos</returns>
        Task<List<Order>> GetOrdersForDeliveryAsync();

        /// <summary>
        /// Pedidos com pagamento rejeitado
        /// </summary>
        /// <returns>Uma fila com os pedidos</returns>
        Task<List<Order>> GetOrdersRejectedAsync();

        /// <summary>
        /// Mover pedido de de "aguardando pagamento" para "pagamento rejeitado"
        /// </summary>
        Task RejectPaymentAsync();
    }
}