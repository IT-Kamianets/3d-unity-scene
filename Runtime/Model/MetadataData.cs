using System.Collections.Generic;
using Newtonsoft.Json;

namespace ITKamianets.Engine.Scene.Model
{
    /// <summary>Mirrors common.schema.json#/$defs/Metadata.</summary>
    [System.Serializable]
    public class MetadataData
    {
        [JsonProperty("tags", NullValueHandling = NullValueHandling.Ignore)]
        public List<string> Tags;

        [JsonProperty("properties", NullValueHandling = NullValueHandling.Ignore)]
        public Dictionary<string, object> Properties;
    }
}
