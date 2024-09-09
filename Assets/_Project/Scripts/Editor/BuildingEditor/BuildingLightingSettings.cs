using System;
using Sirenix.OdinInspector;
using UnityEngine;


namespace DaftAppleGames.Darskerry.Editor.BuildingTools
{
    /// <summary>
    /// Settings for lights in and outside buildings
    /// </summary>
    [CreateAssetMenu(fileName = "BuildingLightingSettings", menuName = "Daft Apple Games/Buildings/Building lighting settings", order = 4)]
    [Serializable]
    public class BuildingLightingSettings : ScriptableObject
    {
        [BoxGroup("Interior Light Settings")] public BuildingLightSettings interiorCandleSettings;
        [BoxGroup("Interior Light Settings")] public BuildingLightSettings interiorFireplaceSettings;
        [BoxGroup("Interior Light Settings")] public BuildingLightSettings interiorCookingFireSettings;
        [BoxGroup("Interior Light Settings")] public BuildingLightSettings interiorTorchSettings;
        
        [BoxGroup("Exterior Light Settings")] public BuildingLightSettings exteriorTorchSettings;
        [BoxGroup("Exterior Light Settings")] public BuildingLightSettings exteriorStreetLightSettings;
    }
}