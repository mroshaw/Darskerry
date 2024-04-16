using System.Collections.Generic;
using System.Linq;
using DaftAppleGames.Common.Buildings;
using UnityEngine;

namespace DaftAppleGames.Editor.BuildingTools
{
    /// <summary>
    /// Static class methods for operating on buildings and structures
    /// </summary>
    public static class BuildingEditorTools
    {
        public static bool IsValidAssetBuilding(GameObject buildingGameObject, BuildingAssetSettings settings)
        {
            string[] nameCheckPattern = settings.assetBuildingIdentifier.GetNamePattern();
            return nameCheckPattern.Any(buildingGameObject.name.Contains);
        }

        /// <summary>
        /// Determines if this GameObject has been initialised with the editor tools
        /// </summary>
        /// <param name="buildingGameObject"></param>
        /// <returns></returns>
        public static bool IsValidBuilding(GameObject buildingGameObject)
        {
            bool isBuilding = buildingGameObject.GetComponent<Building>() != null;
            return isBuilding;
        }

        /// <summary>
        /// Validates the settings, depending on the function being called
        /// </summary>
        /// <param name="function"></param>
        /// <param name="buildingAssetSettings"></param>
        /// <param name="customBuildingSettings"></param>
        /// <param name="activityLog"></param>
        /// <returns></returns>
        public static bool IsValidFunctionSettings(BuildingToolsEditorWindow.ConfigurationFunction function, BuildingAssetSettings buildingAssetSettings, CustomBuildingSettings customBuildingSettings, out List<string> activityLog)
        {
            activityLog = new List<string>();
            switch (function)
            {
                case BuildingToolsEditorWindow.ConfigurationFunction.Layers:
                    if (!customBuildingSettings.interiorCandleSettings)
                    {
                        activityLog.Add($"No LayerSettings found in CustomBuildingSettings named: {customBuildingSettings.name}!");
                        return false;
                    }
                    break;
                case BuildingToolsEditorWindow.ConfigurationFunction.Lighting:
                    if (!customBuildingSettings.interiorCandleSettings)
                    {
                        activityLog.Add($"No LightsSettings found in CustomBuildingSettings named: {customBuildingSettings.name}!");
                        return false;
                    }
                    break;
            }

            return true;
        }
        
        /// <summary>
        /// Initialise the given Game Object as a building
        /// </summary>
        /// <param name="buildingGameObject"></param>
        /// <param name="settings"></param>
        /// <param name="logEntries"></param>
        public static void InitialiseBuilding(GameObject buildingGameObject, BuildingAssetSettings settings, out List<string> logEntries)
        {
            logEntries = new List<string>();

            Vector3 newDimensions = CalculateBuildingDimensions(buildingGameObject, settings.dimensionIncludeLayerMask,
                settings.dimensionExcludeIdentifier.GetNamePattern());

            if (buildingGameObject.GetComponent<Building>())
            {
                logEntries.Add($"Building component already exists on {buildingGameObject.name}. Skipping.");
            }
            else
            {
                buildingGameObject.AddComponent<Building>();
                logEntries.Add($"Added Building component to {buildingGameObject.name}.");
            }
            
            logEntries.Add($"Dimensions calculated for {buildingGameObject.name}: {newDimensions}");

            logEntries.Add("Initialisation complete!");
        }

        /// <summary>
        /// Configure the Layers on the given building
        /// </summary>
        /// <param name="buildingGameObject"></param>
        /// <param name="buildingAssetSettings"></param>
        /// <param name="customBuildingSettings"></param>
        /// <param name="logEntries"></param>
        public static void ConfigureBuildingLayers(GameObject buildingGameObject, BuildingAssetSettings buildingAssetSettings,
            CustomBuildingSettings customBuildingSettings, out List<string> logEntries)
        {
            logEntries = new List<string>();

            logEntries.Add( "Updating layers...");
            logEntries.Add("Updating layers... Done");
        }
        
        public static Vector3 CalculateBuildingDimensions(GameObject mainGameObject, LayerMask includeLayerMask, string[] excludeGameObjects)
        {
            Bounds bounds = new Bounds();
            foreach(Renderer currRenderer in mainGameObject.GetComponentsInChildren<Renderer>(true))
            {
                if (( includeLayerMask & (1 << currRenderer.gameObject.layer)) == 0 &&
                    !excludeGameObjects.Any(x => currRenderer.gameObject.name.Contains(x)))
                {
                    bounds.Encapsulate(currRenderer.localBounds);
                }
            }
            return bounds.size;
        }
    }
}