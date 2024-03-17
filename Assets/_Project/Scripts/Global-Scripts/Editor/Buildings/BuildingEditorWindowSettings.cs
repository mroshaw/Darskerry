using DaftAppleGames.Editor.Common.Performance;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DaftAppleGames.Editor.Buildings
{
    /// <summary>
    /// Scriptable Object to store Editor usable instances of the Player Character Configuration
    /// </summary>
    [CreateAssetMenu(fileName = "BuildingEditorSettings", menuName = "Settings/Buildings/BuildingEditor", order = 1)]
    public class BuildingEditorWindowSettings : ScriptableObject
    {
        [BoxGroup("Preset Settings")]
        [InlineEditor()]
        public DoorEditorSettings doorEditorSettings;
        [BoxGroup("Preset Settings")]
        [InlineEditor()]
        public PropColliderEditorSettings propColliderEditorSettings;
        [BoxGroup("Preset Settings")]
        [InlineEditor()]
        public BuildingPropertiesEditorSettings propertiesEditorSettings;
        [BoxGroup("Preset Settings")]
        [InlineEditor()]
        public LodEditorSettings lodEditorSettings;
        [BoxGroup("Preset Settings")]
        [InlineEditor()]
        public BuildingMeshEditorSettings meshEditorSettings;
        [BoxGroup("Preset Settings")]
        [InlineEditor()]
        public LightingEditorSettings lightingEditorSettings;
    }
}