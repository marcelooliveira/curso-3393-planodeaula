using Newtonsoft.Json;

namespace Organico.Library.Model
{
    // Base class for all entities
    public abstract class BaseEntity
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        protected BaseEntity(string id)
        {
            Id = id;
        }
    }
}