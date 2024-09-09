using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;
using RenderingLayerMask = UnityEngine.Rendering.HighDefinition.RenderingLayerMask;

namespace DaftAppleGames.Darskerry.Editor.BuildingTools
{
    /// <summary>
    /// Settings for lights in and outside buildings
    /// </summary>
    [CreateAssetMenu(fileName = "BuildingLightsSettings", menuName = "Daft Apple Games/Buildings/Building lights settings", order = 4)]
    [Serializable]
    public class BuildingLightSettings : ScriptableObject
    {
        [BoxGroup("Light Settings")] public LightmapBakeType lightMode = LightmapBakeType.Mixed;
        [BoxGroup("Light Settings")]public float radius = 0.025f;
        [BoxGroup("Light Settings")]public float intensity = 150.0f;
        [BoxGroup("Light Settings")]public Color temperatureColor = new Color(227, 197, 100, 255);
        [BoxGroup("Light Settings")]public float temperature = 5630.0f;
        [BoxGroup("Light Settings")]public float range = 10.0f;
        [BoxGroup("Light Settings")]public bool enableVolumetrics = false;
        [BoxGroup("Light Settings")]public bool enableShadowMap = true;
        [BoxGroup("Light Settings")]public ShadowUpdateMode shadowMapUpdateMode = ShadowUpdateMode.OnDemand;
        [BoxGroup("Light Settings")]public RenderingLayerMask lightLayers;
        [BoxGroup("Light Settings")]public bool dayTimeState;
        [BoxGroup("Light Settings")]public bool nightTimeState;

        /// <summary>
        /// Configures a light, calling the appropriate Render Pipeline specific method
        /// </summary>
        /// <param name="light"></param>
        public void ConfigureLight(Light light)
        {

        }
        
        /// <summary>
        /// Configure a given light with the settings - HDRP version
        /// </summary>
        /// <param name="light"></param>
        public void ConfigureLightHDRP(Light light)
        {
            HDAdditionalLightData additionalData = light.GetComponent<HDAdditionalLightData>();
            additionalData.lightmapBakeType = lightMode;
            additionalData.shapeRadius = radius;
            light.intensity = intensity;
            additionalData.range = range;
            additionalData.shadowUpdateMode = shadowMapUpdateMode;
            additionalData.SetColor(temperatureColor, temperature);
            additionalData.affectsVolumetric = enableVolumetrics;
            additionalData.lightlayersMask = lightLayers;
        }

        public void ConfigureLightBIRP(Light light)
        {
            
        }

        public void ConfigureLightURP(Light light)
        {
            
        }

    }
}