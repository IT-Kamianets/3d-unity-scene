using Newtonsoft.Json;

namespace ITKamianets.Engine.Scene.Model
{
    /// <summary>
    /// Mirrors transform.schema.json#/$defs/Transform. "scale" is omitted (schema-optional)
    /// unless explicitly set.
    /// </summary>
    [System.Serializable]
    public class TransformData
    {
        [JsonProperty("position")] public Vector3Data Position;
        [JsonProperty("rotation")] public QuaternionData Rotation;

        [JsonProperty("scale", NullValueHandling = NullValueHandling.Ignore)]
        public Vector3Data? Scale;

        public static TransformData Identity => new TransformData
        {
            Position = Vector3Data.Zero,
            Rotation = QuaternionData.Identity
        };
    }
}
