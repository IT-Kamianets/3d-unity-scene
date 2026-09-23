# IT-Kamianets 3D Engine: Unity Scene

`3d-unity-scene` is the scene runtime package of the **IT-Kamianets 3D Engine**.

It converts engine-independent scene definitions into live Unity scenes and synchronizes changes back into the shared scene format.

### Logic

The package sits between Unity and `3d-scene-schema`.

Its main responsibility is:

**Scene Schema → Unity Scene → Scene Changes → Scene Schema**

Unity should not be the permanent source of truth. The shared scene definition should be able to later work with Unity, Unreal Engine, web viewers, AI tools, and external services.

### Status (version A)

Scoped to exactly what `vroom-scanner`'s first scan-and-upload loop needs: a C# data model of `3d-scene-schema` (v1), under `Runtime/Model/`, and `SceneJson` for converting it to/from the schema's JSON shape (via Newtonsoft.Json, since `JsonUtility` can't serialize the `Dictionary<string, object>` metadata bag or nullable structs the schema needs). Field names are mapped with `[JsonProperty]` to match the schema exactly (camelCase, e.g. `sceneId`, `parentId`).

Deliberately not built yet: instantiating a live Unity scene from `SceneData` (the "Scene Schema → Unity Scene" loader direction) and syncing edits back out. `vroom-scanner`'s version A only needs the saver direction (MRUK scan → `SceneData` → JSON → Firestore); the loader is Phase 2+ once rescanning/comparison is needed.

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
