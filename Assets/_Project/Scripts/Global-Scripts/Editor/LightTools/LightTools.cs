using System;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using Object = System.Object;
#if HDRPPACKAGE_EXIST
using UnityEngine.Rendering.HighDefinition;
#endif

namespace DaftAppleGames.Editor.Common.Performance
{
    public class LightTools : MonoBehaviour
    {
        /// <summary>
        /// Find all renderers with matching criteria in parent Game Object and children
        /// </summary>
        /// <param name="parentGameObject"></param>
        /// <returns></returns>
        public static Light[] FindLightsInGameObject(GameObject parentGameObject)
        {
            Light[] allLights = parentGameObject.GetComponentsInChildren<Light>(true);

            return allLights;
        }

        /// <summary>
        /// Bakes all Reflection Probes in open scenes
        /// </summary>
        public static void BakeAllReflectionProbes()
        {
            ReflectionProbe[] allProbes = GameObject.FindObjectsOfType<ReflectionProbe>();
            foreach (ReflectionProbe probe in allProbes)
            {
                probe.RenderProbe();
            }
        }

        /// <summary>
        /// Find all lights in all gameobjects passed in
        /// </summary>
        /// <param name="allGameObjects"></param>
        /// <returns></returns>
        public static Light[] FindLightsInGameObjects(GameObject[] allGameObjects)
        {
            Light[] allLights = Array.Empty<Light>();

            foreach (GameObject currentGo in allGameObjects)
            {
                Light[] gameObjectLights = FindLightsInGameObject(currentGo);
                allLights = allLights.Concat(gameObjectLights).ToArray();
            }

            return allLights;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="light"></param>
        /// <param name="settings"></param>
        private static void ConfigureSpotLight(Light light, LightingEditorSettings settings)
        {
#if HDRPPACKAGE_EXIST
#else
            light.intensity = settings.spotLightIntensity;
            light.cullingMask = settings.spotLightCullingMask;
#endif
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="light"></param>
        /// <param name="settings"></param>
        private static void ConfigurePointLight(Light light, LightingEditorSettings settings)
        {
            Debug.Log($"LightTools: ConfigurePointLight: Configuring {light.gameObject.name}...");
#if HDRPPACKAGE_EXIST
            LensFlare lensFlare = light.GetComponent<LensFlare>();
            if (lensFlare)
            {
                DestroyImmediate(lensFlare);
            }
            HDAdditionalLightData lightData = light.GetComponent<HDAdditionalLightData>();
            if (!lightData)
            {
                lightData = light.AddComponent<HDAdditionalLightData>();
            }

            lightData.shapeRadius = settings.pointLightSettings.Radius;
            // lightData.SetColor(settings.pointLightSettings.Temperature);
            lightData.intensity = settings.pointLightSettings.Intensity;
            lightData.range = settings.pointLightSettings.Range;

            lightData.affectsVolumetric = settings.pointLightSettings.enableVolumetrics;
            lightData.EnableShadows(settings.pointLightSettings.enableShadowMap);
            lightData.shadowUpdateMode = settings.pointLightSettings.shadowUpdateMode;
            // lightData.shadowResolution. = settings.pointLightSettings.shadowResolution;

            lightData.lightlayersMask = settings.pointLightSettings.lightLayers;

#else
#endif
            Debug.Log($"LightTools: ConfigurePointLight: Configuring {light.gameObject.name} done.");
        }

        private static void ConfigureAreaLight(Light light, LightingEditorSettings settings)
        {
        }

        private static void ConfigureDirectionalLight(Light light, LightingEditorSettings settings)
        {
        }

        /// <summary>
        /// Parse all lights, and configure those of the given type
        /// </summary>
        /// <param name="lights"></param>
        /// <param name="lightType"></param>
        /// <param name="settings"></param>
        public static void ConfigureLightsOfType(Light[] lights, LightType lightType, LightingEditorSettings settings)
        {
            foreach (Light light in lights)
            {
                if (light.type == lightType)
                {
                    switch (light.type)
                    {
                        case LightType.Point:
                            ConfigurePointLight(light, settings);
                            break;
                        case LightType.Spot:
                            ConfigureSpotLight(light, settings);
                            break;
                        case LightType.Area:
                            ConfigureAreaLight(light, settings);
                            break;
                        case LightType.Directional:
                            ConfigureDirectionalLight(light, settings);
                            break;
                    }
                }
            }
        }
        
        public static int CalculateBitmask(int currentBitmask, Array enumValues)
        {
#if HDRPPACKAGE_EXIST
            //int originalBitVal = currentBitmask;
            LightLayerEnum targetLayer = LightLayerEnum.LightLayerDefault;
            foreach (LightLayerEnum current in enumValues)
            {
                // if everything is not set, the inverse in SetBitmask
                // will set all bits to 0, as if nothing was selected
                // so we can just ignore it here
                // it would probably also mess with decal layers
                if (current == LightLayerEnum.Everything) continue;
 
                int layerBitVal = (int)current;
 
                bool set = current == targetLayer;
                //if (set) Debug.Log("Set " + current);
                currentBitmask = SetBitmask(currentBitmask, layerBitVal, set);
            }
 
            //Debug.Log("| Bitmask : " + currentBitmask +
            //        "\r\n| Original: " + originalBitVal);
 
            return currentBitmask;
#else
            return 0;
#endif
            
        }

        public static int SetBitmask(int bitmask, int bitVal, bool set)
        {
#if HDRPPACKAGE_EXIST
            if (set)
                // or "|" will add the value, the 1 at the right position
                bitmask |= bitVal;
            else
                // and "&" will multiply the value, but we take the inverse
                // so the bit position is 0 while all others are 1
                // everything stays as it is, except for the one value
                // which will be set to 0
                bitmask &= ~bitVal;
 
            return bitmask;
#else
            return 0;
#endif

        }
    }
}
