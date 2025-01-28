using System;
using DaftAppleGames.Darskerry.Core.Buildings;
using DaftAppleGames.Extensions;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;
using Object = UnityEngine.Object;

namespace DaftAppleGames.Darskerry.Editor.BuildingTools
{
    [Serializable]
    internal class LightSettings
    {
        [SerializeField] internal BuildingLightType lightType;
        [SerializeField] internal string[] lightParentObjectNames;
        [SerializeField] internal string[] lightObjectLayers;
        [SerializeField] internal LightmapBakeType lightMode;
        [SerializeField] internal float radius;
        [SerializeField] internal float intensityLumens = 150.0f;
        [SerializeField] internal Color filterColor = new(227, 197, 100, 255);
        [SerializeField] internal float temperature = 5630.0f;
        [SerializeField] internal float range = 10.0f;
        [SerializeField] internal bool enableVolumetrics;
        [SerializeField] internal bool enableShadowMap = true;

        [SerializeField] internal ShadowUpdateMode shadowMapUpdateMode = ShadowUpdateMode.OnDemand;

        [SerializeField] internal UnityEngine.Rendering.HighDefinition.RenderingLayerMask lightLayers;
        [SerializeField] internal bool dayTimeState;
        [SerializeField] internal bool nightTimeState;

        internal void AddToBuilding(Building building)
        {
            LightingController lightingController = building.gameObject.EnsureComponent<LightingController>();

            foreach (Light currLight in building.GetComponentsInChildren<Light>(true))
            {
                GameObject parentGameObject = currLight.transform.parent.gameObject;
                string parentName = parentGameObject.name;
                if (lightParentObjectNames.ItemInString(parentName) && lightObjectLayers.ItemInString(LayerMask.LayerToName(currLight.gameObject.layer)))
                {
                    Light newLight = ReplaceLight(currLight);
                    BuildingLight buildingLight = parentGameObject.EnsureComponent<BuildingLight>();
                    buildingLight.buildingLightType = lightType;
                    buildingLight.UpdateLights();
                }
            }

            lightingController.UpdateLights();
        }

        private Light ReplaceLight(Light oldLight)
        {
            HDAdditionalLightData hdLight = oldLight.gameObject.GetComponent<HDAdditionalLightData>();
            LightType oldLightType = oldLight.type;
            GameObject oldLightGameObject = oldLight.gameObject;
            Object.DestroyImmediate(hdLight);
            Object.DestroyImmediate(oldLight);
            Light newLight = oldLightGameObject.AddComponent<Light>();
            oldLightGameObject.AddComponent<HDAdditionalLightData>();
            newLight.type = oldLightType;
            return newLight;
        }

        internal void ApplyToBuilding(Building building)
        {
            foreach (BuildingLight buildingLight in building.GetComponentsInChildren<BuildingLight>())
            {
                if (buildingLight.buildingLightType == lightType)
                {
                    foreach (Light currLight in buildingLight.GetLights())
                    {
                        Apply(currLight);
                    }
                }
            }
        }

        private void Apply(Light light)
        {
            HDAdditionalLightData additionalData = light.GetComponent<HDAdditionalLightData>();
            additionalData.lightmapBakeType = lightMode;
            // additionalData.shapeRadius = radius;
            // additionalData.range = range;
            light.intensity = LightUnitUtils.ConvertIntensity(light, intensityLumens, LightUnit.Lumen, LightUnit.Candela);
            // additionalData.shadowUpdateMode = shadowMapUpdateMode;
            additionalData.EnableColorTemperature(true);
            // light.color = Mathf.CorrelatedColorTemperatureToRGB(temperature);
            light.colorTemperature = temperature;
            light.color = filterColor;
            // additionalData.affectsVolumetric = enableVolumetrics;
            additionalData.lightlayersMask = lightLayers;
        }
    }
}