# IT-Kamianets 3D Engine: Unity Scene

`3d-unity-scene` is the scene runtime package of the **IT-Kamianets 3D Engine**.

It converts engine-independent scene definitions into live Unity scenes and synchronizes changes back into the shared scene format.

### Logic

The package sits between Unity and `3d-scene-schema`.

Its main responsibility is:

**Scene Schema → Unity Scene → Scene Changes → Scene Schema**

Unity should not be the permanent source of truth. The shared scene definition should be able to later work with Unity, Unreal Engine, web viewers, AI tools, and external services.

### Status

Version 0.1, for `3d-scene-schema` 1.1.0 (reads 1.0.0 documents too).

* **Model and JSON:** C# mirror of the schema under `Runtime/Model/`, `SceneJson` for reading and writing (Newtonsoft.Json, exact schema field names). `CoordinateConversion` between the schema's right-handed space and Unity's.
* **Loading** (`SceneLoader.Load(json or SceneData, parent, options)` → `LoadedScene`):
  * one GameObject per scene object, in the document's hierarchy, with the scene's `origin` on the root;
  * a `SceneObject` component on each: id, type, asset, tags (`HasTag("spawn")`, `TryGetTag("team", out var team)` for `team:a`);
  * surfaces from boundaries (double-sided mesh, concave outlines triangulated) with a renderer when a material is given, and a collider; invisible walls get the collider only;
  * box colliders from volumes, triggers for markers (objective zones);
  * asset ids handed to an `ISceneAssetResolver` (implemented by `3d-unity-assets`), so this package does not know how assets are stored.
* **Finding:** `LoadedScene.FindById`, `FindByTag`, `FindByType`.
* **Saving** (`SceneSaver.Capture` / `CaptureJson`): the objects as they are now. Moved objects get their new transforms, removed ones are left out, objects added at runtime with a `SceneObject` are included.

Checked in Play mode with `3d-scene-schema/examples`: the arena level (spawns with team tags facing the centre, a 4×2×4 m objective trigger, a floor facing up with a collider) and both room scans load; saving after moving one crate, deleting another and adding a marker gives JSON that validates against the schema, with zero position error on unchanged objects.

```csharp
var level = SceneLoader.Load(json, null, new SceneLoadOptions { SurfaceMaterial = floorMaterial, AssetResolver = assets });
foreach (var spawn in level.FindByTag("spawn"))
{
    spawn.TryGetTag("team", out var team);   // "a" or "b"
}
string saved = SceneSaver.CaptureJson(level);
```

### Roadmap

* Scene loader
* Scene saver
* Scene graph
* Object creation/removal
* Transform synchronization
* Hierarchical objects
* Cameras
* Lights
* Materials references
* Asset references
* Runtime scene updates
* Partial scene updates
* Scene validation
* Undo-compatible scene operations
* Scene serialization
* Example scenes
* Stable v1 scene runtime
