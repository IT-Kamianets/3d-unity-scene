using Newtonsoft.Json;

namespace ITKamianets.Engine.Scene.Model
{
    [System.Serializable]
    public struct Vector3Data
    {
        [JsonProperty("x")] public float X;
        [JsonProperty("y")] public float Y;
        [JsonProperty("z")] public float Z;

        public Vector3Data(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public static Vector3Data Zero => new Vector3Data(0f, 0f, 0f);
        public static Vector3Data One => new Vector3Data(1f, 1f, 1f);
    }
}
