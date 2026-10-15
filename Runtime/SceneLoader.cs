using System.Collections.Generic;
using ITKamianets.Engine.Core;
using ITKamianets.Engine.Scene.Model;
using UnityEngine;

namespace ITKamianets.Engine.Scene
{
    /// <summary>
    /// Builds live Unity objects from a <see cref="SceneData"/>: one GameObject per scene object,
    /// in the scene's hierarchy, with each one's <see cref="SceneObject"/> id, type, asset and
    /// tags. Optionally builds surfaces from boundaries and colliders from volumes, and asks an
    /// <see cref="ISceneAssetResolver"/> to show assets. The document's right-handed coordinates
    /// are converted to Unity's.
    /// </summary>
    public static class SceneLoader
    {
        private const string LogCategory = "Scene";

        public static LoadedScene Load(string json, Transform parent = null, SceneLoadOptions options = null)
        {
            return Load(SceneJson.Deserialize(json), parent, options);
        }

        public static LoadedScene Load(SceneData data, Transform parent = null, SceneLoadOptions options = null)
        {
            options ??= SceneLoadOptions.Default;

            var root = new GameObject(string.IsNullOrEmpty(data.SceneId) ? "Scene" : data.SceneId);
            root.transform.SetParent(parent, false);
            ApplyTransform(root.transform, data.Origin);

            var loaded = root.AddComponent<LoadedScene>();
            loaded.Data = data;

            // Create everything first, then parent: a child may come before its parent in the list.
            var created = new Dictionary<string, SceneObject>();
            foreach (var objectData in data.Objects)
            {
                var sceneObject = Create(objectData);
                if (!string.IsNullOrEmpty(objectData.Id) && !created.ContainsKey(objectData.Id))
                {
                    created.Add(objectData.Id, sceneObject);
                }
                else
                {
                    EngineLog.Warning(LogCategory, $"Scene '{data.SceneId}': object without a unique id ({objectData.Id ?? "null"}).");
                }

                sceneObject.transform.SetParent(root.transform, false);
                loaded.Register(sceneObject);
            }

            foreach (var objectData in data.Objects)
            {
                if (string.IsNullOrEmpty(objectData.ParentId) || !created.TryGetValue(objectData.Id ?? string.Empty, out var child))
                {
                    continue;
                }

                if (created.TryGetValue(objectData.ParentId, out var parentObject) && !parentObject.transform.IsChildOf(child.transform))
                {
                    child.transform.SetParent(parentObject.transform, false);
                }
                else
                {
                    EngineLog.Warning(LogCategory, $"Scene '{data.SceneId}': parent '{objectData.ParentId}' of '{objectData.Id}' not found; placed at the root.");
                }
            }

            // Transforms are local to the parent, so apply them once the hierarchy is final.
            foreach (var sceneObject in created.Values)
            {
                var objectData = sceneObject.Data;
                ApplyTransform(sceneObject.transform, objectData.Transform);
                BuildContent(sceneObject, objectData, options);

                if (!string.IsNullOrEmpty(objectData.Asset))
                {
                    options.AssetResolver?.Resolve(sceneObject);
                }
            }

            return loaded;
        }

        private static SceneObject Create(SceneObjectData data)
        {
            var go = new GameObject(string.IsNullOrEmpty(data.Name) ? $"{data.Type} {data.Id}" : data.Name);
            var sceneObject = go.AddComponent<SceneObject>();
            sceneObject.Id = data.Id;
            sceneObject.Type = data.Type;
            sceneObject.Asset = data.Asset;
            sceneObject.Data = data;
            if (data.Metadata?.Tags != null)
            {
                sceneObject.Tags.AddRange(data.Metadata.Tags);
            }

            return sceneObject;
        }

        internal static void ApplyTransform(Transform target, TransformData data)
        {
            if (data == null)
            {
                return;
            }

            target.localPosition = CoordinateConversion.FromSchema(data.Position);
            target.localRotation = CoordinateConversion.FromSchema(data.Rotation);
            if (data.Scale.HasValue)
            {
                var scale = data.Scale.Value;
                target.localScale = new Vector3(scale.X, scale.Y, scale.Z);
            }
        }

        private static void BuildContent(SceneObject sceneObject, SceneObjectData data, SceneLoadOptions options)
        {
            var go = sceneObject.gameObject;

            if (options.BuildSurfaces && data.Boundary != null && data.Boundary.Count >= 3)
            {
                var mesh = BuildSurfaceMesh(data.Boundary, data.Id);
                var material = options.SurfaceMaterialFor?.Invoke(sceneObject) ?? options.SurfaceMaterial;
                if (material != null && data.Type != SemanticLabel.INVISIBLE_WALL_FACE)
                {
                    go.AddComponent<MeshFilter>().sharedMesh = mesh;
                    go.AddComponent<MeshRenderer>().sharedMaterial = material;
                }

                if (options.BuildColliders)
                {
                    go.AddComponent<MeshCollider>().sharedMesh = mesh;
                }
            }

            if (options.BuildColliders && data.Volume != null)
            {
                // Schema bounds are right-handed: flip z, and keep the size positive.
                var min = data.Volume.Min;
                var max = data.Volume.Max;
                var box = go.AddComponent<BoxCollider>();
                box.center = new Vector3((min.X + max.X) * 0.5f, (min.Y + max.Y) * 0.5f, -(min.Z + max.Z) * 0.5f);
                box.size = new Vector3(Mathf.Abs(max.X - min.X), Mathf.Abs(max.Y - min.Y), Mathf.Abs(max.Z - min.Z));
                box.isTrigger = options.MarkerVolumesAsTriggers && data.Type == SemanticLabel.MARKER;
            }
        }

        // A boundary lies in the object's local X/Y plane and faces local +Z in Unity. The mesh
        // gets both faces, so it shows and collides from either side.
        private static Mesh BuildSurfaceMesh(List<Vector2Data> boundary, string id)
        {
            var points = new List<Vector2>(boundary.Count);
            foreach (var point in boundary)
            {
                points.Add(new Vector2(point.X, point.Y));
            }

            var front = PolygonTriangulator.Triangulate(points);
            var count = points.Count;

            var vertices = new Vector3[count * 2];
            var uv = new Vector2[count * 2];
            for (var i = 0; i < count; i++)
            {
                vertices[i] = vertices[i + count] = new Vector3(points[i].x, points[i].y, 0f);
                uv[i] = uv[i + count] = points[i];
            }

            var triangles = new int[front.Count * 2];
            for (var i = 0; i < front.Count; i += 3)
            {
                // Counter-clockwise in X/Y is clockwise seen from +Z: Unity's front face.
                triangles[i] = front[i];
                triangles[i + 1] = front[i + 1];
                triangles[i + 2] = front[i + 2];

                triangles[front.Count + i] = front[i] + count;
                triangles[front.Count + i + 1] = front[i + 2] + count;
                triangles[front.Count + i + 2] = front[i + 1] + count;
            }

            var mesh = new Mesh { name = $"Surface {id}", vertices = vertices, uv = uv, triangles = triangles };
            mesh.RecalculateNormals();
            mesh.RecalculateBounds();
            return mesh;
        }
    }
}
