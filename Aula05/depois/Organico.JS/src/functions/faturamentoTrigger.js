const { app } = require('@azure/functions');
const FaturamentoCosmosClient = require('../../faturamentoCosmosClient');

app.timer('faturamentoTrigger', {
    schedule: '0 0 * * * *',
    handler: async (myTimer, context) => {
        const faturamentoCosmosClient = new FaturamentoCosmosClient();

        await faturamentoCosmosClient.initializeAsync();
        await faturamentoCosmosClient.gerarFaturamentoMensal();

        context.log('Faturamento mensal gerado com sucesso!');
    }
});
