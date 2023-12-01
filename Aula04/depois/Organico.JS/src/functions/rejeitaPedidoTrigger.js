const { app } = require('@azure/functions');
const PedidosCosmosClient = require('../../pedidosCosmosClient');

app.storageQueue('rejeitaPedidoTrigger', {
    queueName: 'fila-rejeita-pedido',
    connection: 'organicofunctionapp20231_STORAGE',
    handler: async (queueItem, context) => {
        const pedidosCosmosClient = new PedidosCosmosClient();

        await pedidosCosmosClient.initializeAsync();
        const pedido = await pedidosCosmosClient.get(queueItem);

        if (!pedido) {
            context.error('Pedido não encontado!', queueItem);
            return;
        }

        pedido.status = 3; //PEDIDO REJEITADO
        await pedidosCosmosClient.post(pedido);

        context.log('Pedido rejeitado com sucesso!', queueItem);
    }
});
