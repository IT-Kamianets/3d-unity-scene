using Newtonsoft.Json;

namespace ITKamianets.Engine.Scene.Model
{
    [System.Serializable]
    public struct QuaternionData
    {
        [JsonProperty("x")] public float X;
        [JsonProperty("y")] public float Y;
        [JsonProperty("z")] public float Z;
        [JsonProperty("w")] public float W;

        public QuaternionData(float x, float y, float z, float w)
        {
            X = x;
            Y = y;
            Z = z;
            W = w;
        }

        public static QuaternionData Identity => new QuaternionData(0f, 0f, 0f, 1f);
    }
}
