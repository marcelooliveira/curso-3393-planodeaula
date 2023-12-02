const { CosmosClient } = require("@azure/cosmos");

class FaturamentoCosmosClient {
  constructor() {
    this._client = new CosmosClient({
      endpoint: process.env.CosmosDB_URI,
      key: process.env.CosmosDB_KEY
    });

    this._databaseId = 'organico';
  }

  async initializeAsync() {
    this._containerPedidos = await this._client.database(this._databaseId).container('Pedidos');
    this._containerFaturamento = await this._client.database(this._databaseId).container('Faturamento');
  }

  async gerarFaturamentoMensal() {
      // Consulta para obter todos os itens do container "Pedidos"
      const query = "SELECT * FROM c";
      const { resources: resultados } = await this._containerPedidos.items.query(query).fetchAll();

      const faturamentoMensal = {};

      // Processa os resultados para calcular o faturamento mensal
      resultados.forEach(resultado => {
          const placementDate = new Date(resultado.placement);
          const mesAno = `${placementDate.getFullYear()}-${(placementDate.getMonth() + 1).toString().padStart(2, '0')}`;

          if (!faturamentoMensal[mesAno]) {
              faturamentoMensal[mesAno] = { quantidadeItens: 0, quantidadePedidos: 0, total: 0 };
          }

          faturamentoMensal[mesAno].quantidadeItens += resultado.itemCount;
          faturamentoMensal[mesAno].quantidadePedidos += 1;
          faturamentoMensal[mesAno].total += resultado.total;
      });

      // Grava o resultado no container "Faturamento"
      for (const [mesAno, faturamento] of Object.entries(faturamentoMensal)) {
          const documento = {
              id: mesAno,
              quantidadeItens: faturamento.quantidadeItens,
              quantidadePedidos: faturamento.quantidadePedidos,
              total: faturamento.total
          };

          await this._containerFaturamento.items.upsert(documento);
      }
  }
}

module.exports = FaturamentoCosmosClient;
