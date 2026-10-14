using System.Collections.Generic;
using ITKamianets.Engine.Scene.Model;
using UnityEngine;

namespace ITKamianets.Engine.Scene
{
    /// <summary>
    /// Marks a GameObject as an object of a loaded scene: its schema id, type, asset and tags.
    /// Added by <see cref="SceneLoader"/>; add it yourself to an object created at runtime so
    /// <see cref="SceneSaver"/> writes it back into the scene.
    /// </summary>
    [DisallowMultipleComponent]
    public class SceneObject : MonoBehaviour
    {
        [SerializeField] private string id;
        [SerializeField] private SemanticLabel type;
        [SerializeField] private string asset;
        [SerializeField] private List<string> tags = new List<string>();

        public string Id
        {
            get => id;
            set => id = value;
        }

        public SemanticLabel Type
        {
            get => type;
            set => type = value;
        }

        /// <summary>Asset id (3d-asset-schema) this object is shown with, or null.</summary>
        public string Asset
        {
            get => asset;
            set => asset = value;
        }

        /// <summary>Roles a game reads, e.g. "spawn", "team:a" (from metadata.tags).</summary>
        public List<string> Tags => tags;

        /// <summary>The object as it was loaded; null for objects created at runtime. Not updated as the object moves.</summary>
        public SceneObjectData Data { get; set; }

        public bool HasTag(string tag)
        {
            return tags.Contains(tag);
        }

        /// <summary>Reads a "key:value" tag: TryGetTag("team", out var team) gives "a" for "team:a".</summary>
        public bool TryGetTag(string key, out string value)
        {
            var prefix = key + ":";
            foreach (var tag in tags)
            {
                if (tag.StartsWith(prefix))
                {
                    value = tag.Substring(prefix.Length);
                    return true;
                }
            }

            value = null;
            return false;
        }
    }
}
