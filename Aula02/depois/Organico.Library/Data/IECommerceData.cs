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
        List<CartItem> GetCartItems();

        /// <summary>
        /// Adiciona, modifica ou remove um item do carrinho de compras
        /// </summary>
        /// <param name="cartItem">Item do carrinho</param>
        void AddCartItem(CartItem cartItem);

        /// <summary>
        /// Cria um novo pedido e limpa o carrinho de compras
        /// </summary>
        void CheckOut();

        /// <summary>
        /// Mover pedido de de "aguardando pagamento" para "pronto para entrega"
        /// </summary>
        void ApprovePayment();

        /// <summary>
        /// Pedidos aguardando pagamento
        /// </summary>
        List<Order> GetOrdersAwaitingPayment();

        /// <summary>
        /// Pedidos prontos para entrega
        /// </summary>
        /// <returns>Uma fila com os pedidos</returns>
        List<Order> GetOrdersForDelivery();

        /// <summary>
        /// Pedidos com pagamento rejeitado
        /// </summary>
        /// <returns>Uma fila com os pedidos</returns>
        List<Order> GetOrdersRejected();

        /// <summary>
        /// Mover pedido de de "aguardando pagamento" para "pagamento rejeitado"
        /// </summary>
        void RejectPayment();
    }
}