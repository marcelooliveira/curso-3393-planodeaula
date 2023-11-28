const { app } = require('@azure/functions');

app.storageQueue('aprovaPedidoTrigger', {
    queueName: 'fila-aprova-pedido',
    connection: 'organicofunctionapp20231_STORAGE',
    handler: (queueItem, context) => {
        context.log('Storage queue function processed work item:', queueItem);
    }
});
