using System;
using System.Collections.Generic;
using ITKamianets.Engine.Scene.Model;
using UnityEngine;

namespace ITKamianets.Engine.Scene
{
    /// <summary>
    /// Turns a <see cref="LoadedScene"/> back into a <see cref="SceneData"/>: the objects as they
    /// are now. Moved objects get their new transforms, removed ones are left out, and objects
    /// added at runtime with a <see cref="SceneObject"/> are included; inactive objects are left out. Boundaries, volumes and
    /// metadata properties come from the loaded document, since they are not edited in Unity.
    /// </summary>
    public static class SceneSaver
    {
        public static SceneData Capture(LoadedScene scene)
        {
            var source = scene.Data ?? SceneData.CreateEmpty(scene.name);
            var root = scene.transform;

            var data = new SceneData
            {
                SceneId = source.SceneId,
                CreatedAt = source.CreatedAt,
                UpdatedAt = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ"),
                Origin = ToTransformData(root, includeScale: false),
                Metadata = source.Metadata,
                Objects = new List<SceneObjectData>(),
            };

            foreach (var sceneObject in scene.Objects)
            {
                // Inactive objects are not part of the scene (e.g. deleted in an editor, kept for undo).
                if (!sceneObject.gameObject.activeInHierarchy)
                {
                    continue;
                }

                if (string.IsNullOrEmpty(sceneObject.Id))
                {
                    sceneObject.Id = Guid.NewGuid().ToString();
                }

                var loaded = sceneObject.Data;
                var parent = sceneObject.transform.parent;
                var parentObject = parent != null && parent != root ? parent.GetComponentInParent<SceneObject>() : null;

                data.Objects.Add(new SceneObjectData
                {
                    Id = sceneObject.Id,
                    // Unnamed objects got a placeholder name when loaded; keep them unnamed.
                    Name = loaded != null && loaded.Name == null && sceneObject.name == $"{sceneObject.Type} {sceneObject.Id}" ? null : sceneObject.name,
                    Type = sceneObject.Type,
                    ParentId = parentObject != null ? parentObject.Id : null,
                    Transform = ToTransformData(sceneObject.transform, includeScale: true),
                    Boundary = loaded?.Boundary,
                    Volume = loaded?.Volume,
                    Asset = string.IsNullOrEmpty(sceneObject.Asset) ? null : sceneObject.Asset,
                    Metadata = MetadataFor(sceneObject, loaded),
                });
            }

            return data;
        }

        public static string CaptureJson(LoadedScene scene)
        {
            return SceneJson.Serialize(Capture(scene));
        }

        private static TransformData ToTransformData(Transform transform, bool includeScale)
        {
            var data = new TransformData
            {
                Position = CoordinateConversion.ToSchema(transform.localPosition),
                Rotation = CoordinateConversion.ToSchema(transform.localRotation),
            };

            var scale = transform.localScale;
            if (includeScale && scale != Vector3.one)
            {
                data.Scale = new Vector3Data(scale.x, scale.y, scale.z);
            }

            return data;
        }

        private static MetadataData MetadataFor(SceneObject sceneObject, SceneObjectData loaded)
        {
            var properties = loaded?.Metadata?.Properties;
            if (sceneObject.Tags.Count == 0 && properties == null)
            {
                return null;
            }

            return new MetadataData
            {
                Tags = sceneObject.Tags.Count > 0 ? new List<string>(sceneObject.Tags) : null,
                Properties = properties,
            };
        }
    }
}
