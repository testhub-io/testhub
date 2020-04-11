using System.Text.Json.Serialization;

namespace TestHub.Api.Data
{
    public abstract class DataObjectBase
    {
        [JsonPropertyName("uri")]
        public string Uri { get; set; }
    }
}
