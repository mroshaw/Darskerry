using System;
using DaftAppleGames.Darskerry.Core.Buildings;
using DaftAppleGames.Darskerry.Core.Extensions;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;
using RenderingLayerMask = UnityEngine.RenderingLayerMask;

namespace DaftAppleGames.Darskerry.Editor.BuildingTools
{
    [CreateAssetMenu(fileName = "BuildingEditorSetting", menuName = "Daft Apple Games/Building Editor Settings", order = 1)]
    public class BuildingEditorSettings : ScriptableObject
    {
        #region Properties

        [BoxGroup("Layout Settings")] [SerializeField] internal LayoutSettings layoutSettings;
        [BoxGroup("Layout Settings")] [SerializeField] internal ColliderSettings colliderSettings;
        [BoxGroup("Layout Settings")] [SerializeField] internal LightSettings[] lightingSettings;

        #endregion

        #region Light Settings

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
                DestroyImmediate(hdLight);
                DestroyImmediate(oldLight);
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

        #endregion

        #region Collider Settings

        [Serializable]
        internal class ColliderSettings
        {
            [SerializeField] internal string[] boxObjectNames;
            [SerializeField] internal string[] capsuleObjectNames;

            internal void ApplyToBuilding(Building building)
            {
                foreach (Transform childTransform in building.GetComponentsInChildren<Transform>())
                {
                    string itemName = childTransform.gameObject.name;

                    if (boxObjectNames.ItemInString(itemName))
                    {
                        Apply(childTransform.gameObject, typeof(BoxCollider));
                    }

                    if (capsuleObjectNames.ItemInString(itemName))
                    {
                        Apply(childTransform.gameObject, typeof(CapsuleCollider));
                    }
                }
            }

            private void Apply(GameObject itemGameObject, Type colliderType)
            {
                // Box Colliders
                if (colliderType == typeof(BoxCollider))
                {
                    BoxCollider boxCollider = itemGameObject.EnsureComponent<BoxCollider>();
                    ConfigureBoxCollider(boxCollider);
                }

                // Capsule Colliders
                if (colliderType == typeof(CapsuleCollider))
                {
                    CapsuleCollider capsuleCollider = itemGameObject.EnsureComponent<CapsuleCollider>();
                    ConfigureCapsuleCollider(capsuleCollider);
                }
            }

            private void ConfigureBoxCollider(BoxCollider boxCollider)
            {
            }

            private void ConfigureCapsuleCollider(CapsuleCollider capsuleCollider)
            {
            }
        }

        #endregion

        #region Layout Settings

        [Serializable]
        internal class LayoutSettings
        {
            [SerializeField] internal string interiorLayer = "BuildingInterior";
            [SerializeField] internal string exteriorLayer = "BuildingExterior";
            [SerializeField] internal string interiorPropsLayer = "PropsInterior";
            [SerializeField] internal string exteriorPropsLayer = "PropsExterior";

            internal void ApplyToBuilding(Building building)
            {
                Apply(building.interiors, interiorLayer);
                Apply(building.exteriors, exteriorLayer);
                Apply(building.interiorProps, interiorPropsLayer);
                Apply(building.exteriorProps, exteriorPropsLayer);
            }

            private void Apply(GameObject[] itemGameObjects, string layer)
            {
                foreach (GameObject itemGameObject in itemGameObjects)
                {
                    itemGameObject.layer = LayerMask.NameToLayer(layer);
                    foreach (Transform childTransform in itemGameObject.GetComponentsInChildren<Transform>(true))
                    {
                        childTransform.gameObject.layer = LayerMask.NameToLayer(layer);
                    }
                }
            }
        }

        #endregion
    }
}