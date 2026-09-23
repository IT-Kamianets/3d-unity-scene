using Newtonsoft.Json;
using ITKamianets.Engine.Scene.Model;

namespace ITKamianets.Engine.Scene
{
    /// <summary>Converts SceneData to/from the JSON shape defined by 3d-scene-schema (v1).</summary>
    public static class SceneJson
    {
        private static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore,
            Formatting = Formatting.None
        };

        public static string Serialize(SceneData scene)
        {
            return JsonConvert.SerializeObject(scene, Settings);
        }

        public static SceneData Deserialize(string json)
        {
            return JsonConvert.DeserializeObject<SceneData>(json, Settings);
        }
    }
}
