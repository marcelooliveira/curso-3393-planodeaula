const { app } = require('@azure/functions');
const PedidoCosmosClient = require('../../pedidosCosmosClient');

app.storageQueue('rejeitaPedidoTrigger', {
    queueName: 'fila-rejeita-pedido',
    connection: 'organicofunctionapp20231_STORAGE',
    handler: async (queueItem, context) => {
        const pedidosCosmosClient = new PedidoCosmosClient();
        await pedidosCosmosClient.initializeAsync();
        const pedido = await pedidosCosmosClient.get(queueItem);

        if (!pedido) {
            context.error('Pedido não encontrado: ', queueItem)
            return
        }

        pedido.status = 3; //PEDIDO REJEITADO
        await pedidosCosmosClient.post(pedido);

        context.log('Pedido rejeitado com sucesso:', queueItem);
    }
});
