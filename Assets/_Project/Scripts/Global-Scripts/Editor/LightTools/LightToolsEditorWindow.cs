using System;
using DaftAppleGames.Editor.Common.Performance;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.Serialization;
using LightType = UnityEngine.LightType;

namespace DaftAppleGames.Editor.Buildings
{
    public class LightToolsEditorWindow : OdinEditorWindow
    {
        public enum ToolMode
        {
            Selection,
            SceneObjects,
            Prefabs
        }
        
        // Display Editor Window
        [MenuItem("Window/Lighting/Light Tools")]
        public static void ShowWindow()
        {
            GetWindow(typeof(LightToolsEditorWindow));
        }

        [BoxGroup("Source Objects", centerLabel: true)]
        [FoldoutGroup("Prefabs")]
        [Tooltip("Add prefabs to update")]
        [AssetsOnly]
        [EnableIf("applyMode", ToolMode.Prefabs)]
        public GameObject[] prefabs;
        [BoxGroup("Source Objects", centerLabel: true)]
        [FoldoutGroup("Scene Objects")]
        [Tooltip("Add scene Game Objects to update")]
        [SceneObjectsOnly]
        [EnableIf("applyMode", ToolMode.SceneObjects)]
        public GameObject[] sceneGameObjects;
        [BoxGroup("Source Objects", centerLabel: true)]
        public ToolMode applyMode;

        [FormerlySerializedAs("lightSettings")]
        [FoldoutGroup("Settings")]
        [InlineEditor()]
        public LightingEditorSettings lightingEditorSettings;
        
        // Button UI Setup
        [Button("DO EVERYTHING", ButtonSizes.Large), GUIColor(0, 1, 0)]
        private void DoEveryThingButton()
        {
            ConfigureLights(LightType.Point);
            ConfigureLights(LightType.Area);
            ConfigureLights(LightType.Spot);
            ConfigureLights(LightType.Directional);
        }
        [Button("Configure Point Lights")]
        private void PointLightsButton()
        {
            ConfigureLights(LightType.Point);
        }
        [Button("Configure Area Lights")]
        private void AreaLightButton()
        {
            ConfigureLights(LightType.Area);
        }
       
        [Button("Configure Spot Lights")]
        private void SpotLightButton()
        {
            ConfigureLights(LightType.Spot);
        }

        [Button("Configure Directional Lights")]
        private void DirectionLightButton()
        {
            ConfigureLights(LightType.Directional);
        }

        [Button("Refresh Reflection Probes")]
        private void RefreshReflectionProbes()
        {
            LightTools.BakeAllReflectionProbes();
        }

        private void ConfigureLights(LightType type)
        {
            LightTools.ConfigureLightsOfType(LightTools.FindLightsInGameObjects(GetObjectsToProcess()), type, lightingEditorSettings);
        }

        /// <summary>
        /// Get objects to process, based on type of apply selected
        /// </summary>
        /// <returns></returns>
        private GameObject[] GetObjectsToProcess()
        {
            switch (applyMode)
            {
                case ToolMode.Selection:
                    return Selection.gameObjects;
                case ToolMode.SceneObjects:
                    return sceneGameObjects;
                case ToolMode.Prefabs:
                    return prefabs;
                default:
                    return Array.Empty<GameObject>();
            }
        }
    }
}
