using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace ITKamianets.Engine.Scene.Model
{
    /// <summary>
    /// Mirrors scene.schema.json (v1) exactly. This is the engine-only data model --
    /// it has no concept of interiors, floors, or spaces; the application storing scenes
    /// (e.g. vroom-sync) owns that identity, not this type.
    /// </summary>
    [System.Serializable]
    public class SceneData
    {
        public const string CurrentSchemaVersion = "1.0.0";
        public const string Meters = "meters";
        public const string RightHandedYUp = "right-handed-y-up";

        [JsonProperty("schemaVersion")] public string SchemaVersion = CurrentSchemaVersion;
        [JsonProperty("sceneId")] public string SceneId;
        [JsonProperty("createdAt", NullValueHandling = NullValueHandling.Ignore)] public string CreatedAt;
        [JsonProperty("updatedAt", NullValueHandling = NullValueHandling.Ignore)] public string UpdatedAt;
        [JsonProperty("units")] public string Units = Meters;
        [JsonProperty("coordinateSystem")] public string CoordinateSystem = RightHandedYUp;
        [JsonProperty("origin")] public TransformData Origin = TransformData.Identity;
        [JsonProperty("objects")] public List<SceneObjectData> Objects = new List<SceneObjectData>();
        [JsonProperty("metadata", NullValueHandling = NullValueHandling.Ignore)] public MetadataData Metadata;

        public static SceneData CreateEmpty(string sceneId = null)
        {
            var nowIso = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ");
            return new SceneData
            {
                SceneId = sceneId ?? Guid.NewGuid().ToString(),
                CreatedAt = nowIso,
                UpdatedAt = nowIso
            };
        }
    }
}
