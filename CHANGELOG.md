# Changelog

All notable changes to this package will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]

### Added

- C# model of 3d-scene-schema, `SceneJson`, `CoordinateConversion`.
- Schema 1.1.0: `SceneObjectData.Asset`, `GROUP`, `MARKER`, `PROP`; writes `schemaVersion` 1.1.0, reads 1.0.0.
- `SceneLoader`, `LoadedScene`, `SceneObject`, `SceneLoadOptions`, `ISceneAssetResolver`, `PolygonTriangulator`, `SceneSaver`.

### Changed

- Model classes are no longer `[Serializable]` (they are JSON-only), which removes two Unity serialization warnings.
