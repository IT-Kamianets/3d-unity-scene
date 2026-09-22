# IT-Kamianets 3D Engine — Unity Scene

`3d-unity-scene` is the scene runtime package of the **IT-Kamianets 3D Engine**.

It converts engine-independent scene definitions into live Unity scenes and synchronizes changes back into the shared scene format.

### Logic

The package sits between Unity and `3d-scene-schema`.

Its main responsibility is:

**Scene Schema → Unity Scene → Scene Changes → Scene Schema**

Unity should not be the permanent source of truth. The shared scene definition should be able to later work with Unity, Unreal Engine, web viewers, AI tools, and external services.

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
