using System;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.Rendering.HighDefinition;

namespace DaftAppleGames.Common.Buildings
{
    /// <summary>
    /// Settings for lights in and outside buildings
    /// </summary>
    [CreateAssetMenu(fileName = "BuildingLightSettings", menuName = "Buildings/Building Light Settings", order = 1)]
    [Serializable]
    public class BuildingLightSettings : ScriptableObject
    {
        [BoxGroup("Light Settings")] public LightmapBakeType lightMode = LightmapBakeType.Mixed;
        [BoxGroup("Light Settings")] public float Radius = 0.025f;
        [BoxGroup("Light Settings")] public float Intensity = 150.0f;
        [BoxGroup("Light Settings")] public float Temperature = 5630.0f;
        [BoxGroup("Light Settings")] public float Range = 10.0f;
        [BoxGroup("Light Settings")] public bool EnableVolumetrics = false;
        [BoxGroup("Light Settings")] public bool EnableShadowMap = true;
        [BoxGroup("Light Settings")] public ShadowUpdateMode ShadowMapUpdateMode = ShadowUpdateMode.OnDemand;
        [BoxGroup("Light Settings")] public LightLayerEnum lightLayers;
        [BoxGroup("Behaviour")] public bool dayTimeState;
        [BoxGroup("Behaviour")] public bool nightTimeState;

        /// <summary>
        /// Configure a given light with the settings
        /// </summary>
        /// <param name="light"></param>
        public void ConfigureLight(Light light)
        {
            HDAdditionalLightData additionalData = light.GetComponent<HDAdditionalLightData>();
#if UNITY_EDITOR
            additionalData.lightmapBakeType = lightMode;
#endif
            additionalData.shapeRadius = Radius;
            additionalData.intensity = Intensity;
            additionalData.range = Range;
            additionalData.shadowUpdateMode = ShadowMapUpdateMode;
            additionalData.SetColor(Color.white, Temperature);
            additionalData.affectsVolumetric = EnableVolumetrics;
            additionalData.lightlayersMask = lightLayers;
        }

    }
}
