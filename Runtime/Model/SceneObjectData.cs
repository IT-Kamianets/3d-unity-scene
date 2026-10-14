using System.Collections.Generic;
using Newtonsoft.Json;

namespace ITKamianets.Engine.Scene.Model
{
    /// <summary>Mirrors scene-object.schema.json#/$defs/SceneObject.</summary>
    public class SceneObjectData
    {
        [JsonProperty("id")] public string Id;
        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)] public string Name;
        [JsonProperty("type")] public SemanticLabel Type;
        [JsonProperty("parentId")] public string ParentId;
        [JsonProperty("transform")] public TransformData Transform;

        [JsonProperty("boundary", NullValueHandling = NullValueHandling.Ignore)]
        public List<Vector2Data> Boundary;

        [JsonProperty("volume", NullValueHandling = NullValueHandling.Ignore)]
        public BoundsData Volume;

        /// <summary>Since schema 1.1.0: id of the asset (3d-asset-schema) the object is shown with.</summary>
        [JsonProperty("asset", NullValueHandling = NullValueHandling.Ignore)]
        public string Asset;

        [JsonProperty("metadata", NullValueHandling = NullValueHandling.Ignore)]
        public MetadataData Metadata;
    }
}
