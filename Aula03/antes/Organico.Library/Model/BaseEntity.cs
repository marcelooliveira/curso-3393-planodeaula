using Newtonsoft.Json;

namespace Organico.Library.Model
{
    /// <summary>
    /// Classe base para todas as entidades
    /// </summary>
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