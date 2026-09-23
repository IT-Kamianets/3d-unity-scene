using UnityEngine;
using ITKamianets.Engine.Scene.Model;

namespace ITKamianets.Engine.Scene
{
    /// <summary>
    /// Converts between Unity's left-handed, Y-up coordinate space and the schema's
    /// right-handed, Y-up space (3d-scene-schema's "coordinateSystem": "right-handed-y-up").
    /// Both are Y-up, so only Z needs to flip.
    /// </summary>
    public static class CoordinateConversion
    {
        public static Vector3Data ToSchema(Vector3 unityPosition)
        {
            return new Vector3Data(unityPosition.x, unityPosition.y, -unityPosition.z);
        }

        public static QuaternionData ToSchema(Quaternion unityRotation)
        {
            return new QuaternionData(-unityRotation.x, -unityRotation.y, unityRotation.z, unityRotation.w);
        }

        public static Vector3 FromSchema(Vector3Data schemaPosition)
        {
            return new Vector3(schemaPosition.X, schemaPosition.Y, -schemaPosition.Z);
        }

        public static Quaternion FromSchema(QuaternionData schemaRotation)
        {
            return new Quaternion(-schemaRotation.X, -schemaRotation.Y, schemaRotation.Z, schemaRotation.W);
        }
    }
}
