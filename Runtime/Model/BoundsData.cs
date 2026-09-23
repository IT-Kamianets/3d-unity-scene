using Newtonsoft.Json;

namespace ITKamianets.Engine.Scene.Model
{
    /// <summary>Mirrors common.schema.json#/$defs/Bounds3D.</summary>
    [System.Serializable]
    public class BoundsData
    {
        [JsonProperty("min")] public Vector3Data Min;
        [JsonProperty("max")] public Vector3Data Max;
    }
}
