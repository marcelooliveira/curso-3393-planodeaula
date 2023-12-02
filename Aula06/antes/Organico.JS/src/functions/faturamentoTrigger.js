const { app } = require('@azure/functions');
const FaturamentoCosmosClient = require('../../faturamentoCosmosClient');

app.timer('faturamentoTrigger', {
    // schedule: '0 * * * *',
    schedule: '*/5 * * * *',
    handler: async (myTimer, context) => {
        const faturamentoCosmosClient = new FaturamentoCosmosClient();

        await faturamentoCosmosClient.initializeAsync();
        await faturamentoCosmosClient.gerarFaturamentoMensal();

        context.log('Faturamento gerado com sucesso.');
    }
});
