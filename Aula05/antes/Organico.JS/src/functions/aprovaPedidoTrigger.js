const { app } = require('@azure/functions');
const PedidoCosmosClient = require('../../pedidosCosmosClient');

app.storageQueue('aprovaPedidoTrigger', {
    queueName: 'fila-aprova-pedido',
    connection: 'organicofunctionapp20231_STORAGE',
    handler: async (queueItem, context) => {
        const pedidosCosmosClient = new PedidoCosmosClient();
        await pedidosCosmosClient.initializeAsync();
        const pedido = await pedidosCosmosClient.get(queueItem);

        if (!pedido) {
            context.error('Pedido não encontrado: ', queueItem)
            return
        }

        pedido.status = 2; //PEDIDO APROVADO
        await pedidosCosmosClient.post(pedido);

        context.log('Pedido aprovado com sucesso:', queueItem);
    }
});
