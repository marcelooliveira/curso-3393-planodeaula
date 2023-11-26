const { CosmosClient } = require("@azure/cosmos");

class PedidosCosmosClient {
  constructor() {
    this._client = new CosmosClient({
      endpoint: process.env.CosmosDB_URI,
      key: process.env.CosmosDB_KEY
    });

    this._databaseId = 'organico';
  }

  async initializeAsync() {
    this._container = await this._client.database(this._databaseId).container('Pedidos');
  }

  async get(id) {
    const result = [];

    const querySpec = {
      query: `SELECT * FROM c WHERE c.id = '${id}'`
    };

    const { resources: pedidos } = await this._container.items.query(querySpec).fetchNext();
    if (pedidos.length == 0) {
        return null;
    }
    return pedidos[0]
  }

  async post(order) {
    const itemResponse = await this._container.items.upsert(order);
  }
}

module.exports = PedidosCosmosClient;
