using DaftAppleGames.Editor.Common;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DaftAppleGames.Editor.Buildings
{
    /// <summary>
    /// Scriptable Object to store Editor usable instances of the Player Character Configuration
    /// </summary>
    [CreateAssetMenu(fileName = "BuildingMeshPropertiesEditorSettings", menuName = "Settings/Buildings/BuildingMeshEditor", order = 1)]
    public class BuildingMeshEditorSettings : EditorToolSettings
    {
        [BoxGroup("Building Mesh Settings")]
        public MeshSettings exterior;
        [BoxGroup("Building Mesh Settings")]
        public MeshSettings interior;
        [BoxGroup("Building Mesh Settings")]
        public MeshSettings interiorProps;
        [BoxGroup("Building Mesh Settings")]
        public MeshSettings exteriorProps;
    }
}