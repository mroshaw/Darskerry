using DaftAppleGames.Editor.Common;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace DaftAppleGames.Editor.Buildings
{
    /// <summary>
    /// Scriptable Object to store Editor usable instances of the Player Character Configuration
    /// </summary>
    [CreateAssetMenu(fileName = "BuildingPropertiesEditorSettings", menuName = "Settings/Buildings/BuildingPropertiesEditor", order = 1)]
    public class BuildingPropertiesEditorSettings : EditorToolSettings
    {
        [BoxGroup("Building Component Settings")]
        [AssetsOnly]
        public GameObject buildingComponentPrefab;
        [BoxGroup("Layer Settings")]
        public string[] exteriorSearchStrings;
        [BoxGroup("Layer Settings")]
        public string[] interiorSearchStrings;
        [BoxGroup("Layer Settings")]
        public string[] propsSearchStrings;
        [BoxGroup("Layer Settings")]
        public string exteriorLayer;
        [BoxGroup("Layer Settings")]
        public string interiorLayer;
        [BoxGroup("Layer Settings")]
        public string propsLayer;
        [BoxGroup("Layer Settings")]
        public string[] layerExcludeSearchStrings;
        [BoxGroup("Static Flag Settings")]
        public StaticEditorFlags staticFlags = StaticEditorFlags.BatchingStatic | 
                                               StaticEditorFlags.OccludeeStatic | StaticEditorFlags.OccluderStatic |
                                               StaticEditorFlags.ContributeGI | StaticEditorFlags.ReflectionProbeStatic;
        [BoxGroup("Static Flag Settings")]
        public string[] staticIgnoreSearchStrings;

        [BoxGroup("Interior Collider Settings")]
        public float interiorCullingMargin = 10.0f;
        [BoxGroup("Interior Collider Settings")]
        public float interiorTriggerMargin = 0.0f;

        [BoxGroup("NavMesh Settings")]
        public float navMeshMargin = 5.0f;
        [BoxGroup("NavMesh Settings")]
        public string navMeshArea = "Building";
        
        [BoxGroup("Terrain Align Settings")]
        public string[] propNamesSearchStrings;
        [BoxGroup("Terrain Align Settings")]
        public float propAlignOffset;
        [BoxGroup("Terrain Settings")]
        public Texture2D terrainBrush;
        public float flattenMargin;
        public float clearDetailMargin;
        public float flattenHeightModifier;
        public float buildingHeightOffset;
        public string[] boundsCheckIncludeNames;
        public string boundsCheckIncludeLayer;
    }
    
}