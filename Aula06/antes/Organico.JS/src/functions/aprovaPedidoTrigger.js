const { app } = require('@azure/functions');
const PedidosCosmosClient = require('../../pedidosCosmosClient');

app.storageQueue('aprovaPedidoTrigger', {
    queueName: 'fila-aprova-pedido',
    connection: 'organicofunctionapp20231_STORAGE',
    handler: async (queueItem, context) => {
        const pedidosCosmosClient = new PedidosCosmosClient();

        await pedidosCosmosClient.initializeAsync();
        const pedido = await pedidosCosmosClient.get(queueItem);

        if (!pedido) {
            context.error('Pedido não encontado!', queueItem);
            return;
        }

        pedido.status = 2; //AGUARDANDO ENTREGA
        await pedidosCosmosClient.post(pedido);

        context.log('Pedido aprovado com sucesso!', queueItem);
    }
});
