using System.Collections.Generic;
using DaftAppleGames.Darskerry.Core.Buildings;
using DaftAppleGames.Extensions;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;

namespace DaftAppleGames.Darskerry.Editor.BuildingTools
{
    public enum LogLevel
    {
        Debug,
        Info,
        Warning,
        Error,
        Fatal,
        None,
        All
    }

    public class BuildingEditorWindow : OdinEditorWindow
    {
        [BoxGroup("Selected Objects")] [SerializeField] private List<Building> selectedBuildings;
        [BoxGroup("Building Settings")] [SerializeField] [InlineEditor] private BuildingEditorSettings buildingSettings;
        [BoxGroup("Settings")] [SerializeField] private LogLevel logLevel = LogLevel.All;

        #region UI Methods

        private void OnSelectionChange()
        {
            selectedBuildings = new List<Building>();
            foreach (GameObject selectedObject in Selection.gameObjects)
            {
                Building building = selectedObject.GetComponent<Building>();
                if (building)
                {
                    selectedBuildings.Add(building);
                }
            }
        }

        [MenuItem("Daft Apple Games/Buildings/Simple Building Editor")]
        public static void ShowWindow()
        {
            BuildingEditorWindow wnd = GetWindow<BuildingEditorWindow>();
            wnd.titleContent = new GUIContent("Simple Building Editor");
        }

        [BoxGroup("Main Setup")] [Button("Add Components to Selected")]
        private void AddBuildingToSelected()
        {
            GameObject selectedObject = Selection.activeGameObject;
            if (selectedObject)
            {
                selectedObject.EnsureComponent<Building>();
                selectedObject.EnsureComponent<LightingController>();
            }
        }

        [BoxGroup("Colliders")] [Button("Configure Colliders")]
        private void ConfigureColliders()
        {
            Log("Configuring colliders...");
            foreach (Building building in selectedBuildings)
            {
                buildingSettings.colliderSettings.ApplyToBuilding(building);
            }

            Log("Configuring colliders... Done!");
        }

        [BoxGroup("Layout")] [Button("Configure Layout")]
        private void ConfigureLayout()
        {
            Log("Configuring layout...");
            foreach (Building building in selectedBuildings)
            {
                buildingSettings.layoutSettings.ApplyToBuilding(building);
            }

            Log("Configuring layout... Done!");
        }

        [BoxGroup("Meshes")] [Button("Configure Meshes")]
        private void ConfigureMeshes()
        {
            Log("Configuring meshes...");
            foreach (Building building in selectedBuildings)
            {
                buildingSettings.meshSettings.ApplyToBuilding(building);
            }

            Log("Configuring meshes... Done!");
        }

        [BoxGroup("Lights")] [Button("Add Lights")]
        private void AddLights()
        {
            Log("Adding lights...");
            foreach (Building building in selectedBuildings)
            {
                foreach (LightSettings currLightSettings in buildingSettings.lightingSettings)
                {
                    currLightSettings.AddToBuilding(building);
                }
            }

            Log("Adding volumes... Done!");
        }

        [BoxGroup("Lights")] [Button("Configure Lights")]
        private void ConfigureLights()
        {
            Log("Configuring lights...");
            foreach (Building building in selectedBuildings)
            {
                foreach (LightSettings currLightSettings in buildingSettings.lightingSettings)
                {
                    currLightSettings.ApplyToBuilding(building);
                }
            }

            Log("Configuring Lights... Done!");
        }

        [BoxGroup("Volumes")] [Button("Configure Volumes")]
        private void ConfigureVolumes()
        {
            foreach (Building building in selectedBuildings)
            {
                buildingSettings.volumeSettings.ApplyToBuilding(building);
            }

            Log("Configuring Volumes... Done!");
        }

        [BoxGroup("Volumes")] [Button("Configure Doors")]
        private void ConfigureDoors()
        {
            foreach (Building building in selectedBuildings)
            {
                buildingSettings.doorSettings.ApplyToBuilding(building);
            }

            Log("Configuring Doors... Done!");
        }

        private static void Log(string message, LogLevel messageLogLevel = LogLevel.Debug)
        {
            switch (messageLogLevel)
            {
                case LogLevel.Error:
                    Debug.LogError(message);
                    break;
                default:
                    Debug.Log(message);
                    break;
            }
        }

        #endregion
    }
}