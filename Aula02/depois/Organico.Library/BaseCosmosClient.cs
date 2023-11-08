using Microsoft.Azure.Cosmos;

namespace Organico.Library
{
    public abstract class BaseCosmosClient
    {
        protected CosmosClient _client;
        protected Database _database;
        protected Container _container;

        protected abstract string ContainerId { get; set; }

        public void InitializeAsync()
        {
            _client = new CosmosClient(Environment.GetEnvironmentVariable("CosmosDB_URI"), Environment.GetEnvironmentVariable("CosmosDB_KEY"));
            _database = _client.GetDatabase("organico");
            _container = _database.GetContainer(ContainerId);
        }
    }
}
