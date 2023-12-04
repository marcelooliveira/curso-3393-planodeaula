const { app } = require('@azure/functions');

app.timer('faturamentoTrigger', {
    schedule: '0 0 * * * *',
    handler: (myTimer, context) => {
        context.log('Timer function processed request.');
    }
});
