using UnityEngine;

namespace ITKamianets.Engine.Scene
{
    /// <summary>
    /// Shows an object's asset. Implemented by 3d-unity-assets; this package only calls it, so
    /// scenes load without knowing how assets are stored. May finish later (e.g. a download):
    /// the object exists at once and its content appears when the asset is ready.
    /// </summary>
    public interface ISceneAssetResolver
    {
        void Resolve(SceneObject target);
    }

    /// <summary>What <see cref="SceneLoader"/> builds besides the objects themselves.</summary>
    public class SceneLoadOptions
    {
        /// <summary>Mesh and collider from each planar object's boundary (floors, walls, ...).</summary>
        public bool BuildSurfaces = true;

        /// <summary>Material for surface meshes. Null builds them without a renderer: colliders only.</summary>
        public Material SurfaceMaterial;

        /// <summary>Colliders for surfaces and volumes. Invisible walls get colliders without renderers.</summary>
        public bool BuildColliders = true;

        /// <summary>A MARKER with a volume (e.g. an objective zone) gets a trigger collider instead of a solid one.</summary>
        public bool MarkerVolumesAsTriggers = true;

        /// <summary>Shows objects that have an asset id. Null leaves them empty.</summary>
        public ISceneAssetResolver AssetResolver;

        public static SceneLoadOptions Default => new SceneLoadOptions();
    }
}
