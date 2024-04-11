using Sirenix.OdinInspector;
using UnityEngine;

namespace DaftAppleGames.Editor.BuildingTools
{
    /// <summary>
    /// Building layer settings
    /// </summary>
    [CreateAssetMenu(fileName = "BuildingLayerSettings", menuName = "Daft Apple Games/Buildings/Building layer settings", order = 3)]
    public class BuildingLayerSettings : ScriptableObject
    {
        // Public serializable properties
        [BoxGroup("Setting Name")] public string settingName;
        
        [BoxGroup("Exterior Layer Settings")] public string exteriorLayer;
        [BoxGroup("Exterior Layer Settings")] public string exteriorPropsLayer;
        [BoxGroup("Interior Layer Settings")] public string interiorLayer;
        [BoxGroup("Interior Layer Settings")] public string interiorPropsLayer;
    }
}