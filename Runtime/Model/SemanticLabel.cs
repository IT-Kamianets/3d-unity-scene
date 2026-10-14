using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace ITKamianets.Engine.Scene.Model
{
    /// <summary>
    /// Mirrors scene-object.schema.json#/$defs/SemanticLabel. Serialized as the exact
    /// string values the schema expects (via StringEnumConverter), not enum indices.
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum SemanticLabel
    {
        ROOM,
        FLOOR,
        CEILING,
        WALL_FACE,
        INVISIBLE_WALL_FACE,
        DOOR_FRAME,
        WINDOW_FRAME,
        TABLE,
        COUCH,
        BED,
        STORAGE,
        SCREEN,
        LAMP,
        PLANT,
        WALL_ART,
        OTHER,

        // Since schema 1.1.0: authored level elements.

        /// <summary>A container with no content of its own.</summary>
        GROUP,

        /// <summary>A point whose meaning is in metadata tags, e.g. a spawn point.</summary>
        MARKER,

        /// <summary>An object shown with its asset.</summary>
        PROP
    }
}
