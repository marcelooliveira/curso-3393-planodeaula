const { app } = require('@azure/functions');

app.storageQueue('rejeitaPedidoTrigger', {
    queueName: 'fila-rejeita-pedido',
    connection: 'organicofunctionapp20231_STORAGE',
    handler: (queueItem, context) => {
        context.log('Storage queue function processed work item:', queueItem);
    }
});
