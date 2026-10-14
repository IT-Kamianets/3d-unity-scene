using System.Collections.Generic;
using ITKamianets.Engine.Scene.Model;
using UnityEngine;

namespace ITKamianets.Engine.Scene
{
    /// <summary>
    /// The root of a scene built by <see cref="SceneLoader"/>. Finds its objects by id or tag,
    /// and is what <see cref="SceneSaver"/> turns back into a <see cref="SceneData"/>.
    /// </summary>
    [DisallowMultipleComponent]
    public class LoadedScene : MonoBehaviour
    {
        private readonly Dictionary<string, SceneObject> _byId = new Dictionary<string, SceneObject>();

        /// <summary>The document as it was loaded. Not updated as objects move; capture a new one with <see cref="SceneSaver"/>.</summary>
        public SceneData Data { get; internal set; }

        public string SceneId => Data?.SceneId;

        /// <summary>Every object of the scene, including ones added at runtime with a <see cref="SceneObject"/>.</summary>
        public SceneObject[] Objects => GetComponentsInChildren<SceneObject>(true);

        public SceneObject FindById(string id)
        {
            if (id != null && _byId.TryGetValue(id, out var found) && found != null)
            {
                return found;
            }

            foreach (var sceneObject in Objects)
            {
                if (sceneObject.Id == id)
                {
                    _byId[id] = sceneObject;
                    return sceneObject;
                }
            }

            return null;
        }

        public List<SceneObject> FindByTag(string tag)
        {
            var found = new List<SceneObject>();
            foreach (var sceneObject in Objects)
            {
                if (sceneObject.HasTag(tag))
                {
                    found.Add(sceneObject);
                }
            }

            return found;
        }

        public List<SceneObject> FindByType(SemanticLabel type)
        {
            var found = new List<SceneObject>();
            foreach (var sceneObject in Objects)
            {
                if (sceneObject.Type == type)
                {
                    found.Add(sceneObject);
                }
            }

            return found;
        }

        /// <summary>Destroys the scene's GameObjects.</summary>
        public void Unload()
        {
            Destroy(gameObject);
        }

        internal void Register(SceneObject sceneObject)
        {
            if (!string.IsNullOrEmpty(sceneObject.Id))
            {
                _byId[sceneObject.Id] = sceneObject;
            }
        }
    }
}
