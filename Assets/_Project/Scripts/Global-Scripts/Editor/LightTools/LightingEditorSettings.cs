using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

#if HDPipeline
using UnityEngine.Rendering.HighDefinition;
#else
using UnityEngine.Rendering;
#endif
namespace DaftAppleGames.Editor.Common.Performance
{
    public enum LightMode
    {
        Baked,
        Mixed,
        Realtime
    };

    /// <summary>
    /// Scriptable Object to store Editor usable instances of the Player Character Configuration
    /// </summary>
    [CreateAssetMenu(fileName = "LightSettings", menuName = "Settings/Lighting/LightSettings", order = 1)]
    public class LightingEditorSettings : EditorToolSettings
    {
        [BoxGroup("General Settings")]
        public bool createIfMissing = false;

        [BoxGroup("Point Light")]
#if HDPipeline
        public LightSettingsHd pointLightSettings;
#else
    public LightSettings pointLightSettings;
#endif
        [BoxGroup("Spot Light")]
#if HDPipeline
        public LightSettingsHd spotLightSettings;
#else
    public LightSettings spotLightSettings;
#endif
        [BoxGroup("Area Light")]
#if HDPipeline
        public LightSettingsHd areaLightSettings;
#else
    public LightSettings areaLightSettings;
#endif

        [Serializable]
        public class LightSettingsHd
        {
            public float Radius;
            public float Temperature;
            public float Intensity;
            public float Range;
            public float IndirectMultiplier;
            public bool enableVolumetrics;
            // public float volumetricsMultiplier;
            // public float volumetricsShadowDimmer;
            // public float volumetricsFadeDistance;
            public bool enableShadowMap;
            public ShadowUpdateMode shadowUpdateMode;
            // public float shadowResolution;
            public LightLayerEnum lightLayers;
        }

        public class LightSettings
        {
            public float Radius;
            public float Temperature;
            public float Intensity;
            public float Range;
            public float IndirectMultiplier;
        }
    }
}