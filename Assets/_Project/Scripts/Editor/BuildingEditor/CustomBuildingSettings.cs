using Sirenix.OdinInspector;
using UnityEngine;

namespace DaftAppleGames.Darskerry.Editor.BuildingTools
{
    /// <summary>
    /// Settings for all customisation of various components, applied via the Editor Tools
    /// </summary>
    [CreateAssetMenu(fileName = "CustomBuildingSettings", menuName = "Daft Apple Games/Buildings/Custom building settings", order = 2)]
    public class CustomBuildingSettings : ScriptableObject
    {
        // Public serializable properties
        [BoxGroup("Setting Name")] public string settingName;

        [BoxGroup("Behaviour Settings")] [Tooltip("These are the layers that will trigger interior volume functionality. Typically, this will be your 'Player' layer.")] public LayerMask TriggerLayerMask;
        [BoxGroup("Behaviour Settings")] [Tooltip("These are optional tags that can be added to the checks when a GameObject collider triggers volume functionality. For example, if your Player has a main collider with the 'Player' tag.")] public string[] TriggerTags;
        [BoxGroup("Behaviour Settings")] [Tooltip("This is the layer in which all trigger Colliders will be added.")] public string triggerLayerName;
        
        [BoxGroup("Exterior Layer Settings")] [Tooltip("This is the layer that will be applied to exterior Mesh Renderer Game Objects.")] public string exteriorLayerName;
        [BoxGroup("Exterior Layer Settings")] [Tooltip("This is the layer that will be applied to exterior prop Mesh Renderer Game Objects.")]  public string exteriorPropsLayerName;
        [BoxGroup("Interior Layer Settings")] [Tooltip("This is the layer that will be applied to interior Mesh Renderer Game Objects.")] public string interiorLayerName;
        [BoxGroup("Interior Layer Settings")] [Tooltip("This is the layer that will be applied to interior prop Mesh Renderer Game Objects.")] public string interiorPropsLayerName;
        
        [BoxGroup("Interior Light Settings")] [Tooltip("These settings will be applied to all interior candle lights.")] public BuildingLightSettings interiorCandleSettings;
        [BoxGroup("Interior Light Settings")] [Tooltip("These settings will be applied to all interior fire place lights.")] public BuildingLightSettings interiorFireplaceSettings;
        [BoxGroup("Interior Light Settings")] [Tooltip("These settings will be applied to all interior cooking fire lights.")] public BuildingLightSettings interiorCookingFireSettings;
        [BoxGroup("Interior Light Settings")] [Tooltip("These settings will be applied to all interior torch lights.")] public BuildingLightSettings interiorTorchSettings;
        
        [BoxGroup("Exterior Light Settings")] [Tooltip("These settings will be applied to all exterior candle lights.")] public BuildingLightSettings exteriorCandleSettings;
        [BoxGroup("Exterior Light Settings")] [Tooltip("These settings will be applied to all exterior torch lights.")] public BuildingLightSettings exteriorTorchSettings;
        [BoxGroup("Exterior Light Settings")] [Tooltip("These settings will be applied to all exterior street lights.")] public BuildingLightSettings exteriorStreetLightSettings;

        /// <summary>
        /// Return the LayerName as a Layer
        /// </summary>
        public LayerMask TriggerLayer => LayerMask.NameToLayer(triggerLayerName);
        public LayerMask ExteriorLayer => LayerMask.NameToLayer(exteriorLayerName);
        public LayerMask InteriorLayer => LayerMask.NameToLayer(interiorLayerName);
        public LayerMask ExteriorPropsLayer => LayerMask.NameToLayer(exteriorPropsLayerName);
        public LayerMask InteriorPropsLayer => LayerMask.NameToLayer(interiorPropsLayerName);
    }
}