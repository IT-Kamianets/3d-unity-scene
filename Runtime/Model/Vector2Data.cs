using Newtonsoft.Json;

namespace ITKamianets.Engine.Scene.Model
{
    [System.Serializable]
    public struct Vector2Data
    {
        [JsonProperty("x")] public float X;
        [JsonProperty("y")] public float Y;

        public Vector2Data(float x, float y)
        {
            X = x;
            Y = y;
        }
    }
}
