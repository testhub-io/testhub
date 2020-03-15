using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace TestsHub.Api.Data
{
    public abstract class DataObjectBase
    {
        [JsonPropertyName("uri")]
        public string Uri { get; set; }
    }
}
