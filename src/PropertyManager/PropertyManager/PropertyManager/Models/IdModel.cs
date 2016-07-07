using Newtonsoft.Json;

namespace PropertyManager.Models
{
    public class IdModel
    {
        [JsonProperty("@odata.id")]
        public string Id { get; set; }
    }
}