using System.Collections.Generic;
using DaftAppleGames.Common.GameControllers;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Unity.AI.Navigation;
using UnityEditor;
using UnityEngine;

namespace DaftAppleGames.Editor.BuildTools
{
    public class BuildToolsEditorWindow : OdinEditorWindow
    {
        // Display Editor Window
        [MenuItem("Daft Apple Games/Tools/Project/Build tool")]
        public static void ShowWindow()
        {
            GetWindow(typeof(BuildToolsEditorWindow));
        }

        [Button("DO EVERYTHING", ButtonSizes.Large), GUIColor(0, 1, 0)]
        private void DoEveryThingButton()
        {
            BakeLighting();
            BakeNavMesh();
            BakeOcclusion();
        }
        [Button("Bake Lighting")]
        private void BakeLightsButton()
        {
            BakeLighting();
        }
        [Button("Bake NavMesh")]
        private void BakeNavmeshButton()
        {
            BakeNavMesh();
        }

        [Button("Bake Occlusion")]
        private void BakeOcclusionButton()
        {
            BakeOcclusion();
        }

        private static void BakeLighting()
        {
            Debug.Log("BuildTools: Bake Lighting...");
            Lightmapping.BakeMultipleScenes(GetScenePaths());
            Debug.Log("BuildTools: Bake Lighting done.");
        }

        private static void BakeNavMesh()
        {
            Debug.Log("BuildTools: Bake NavMesh...");
            // NavMeshBuilder.BuildNavMeshForMultipleScenes(GetScenePaths());
            NavMeshSurface[] allNavMeshSurfaces = (NavMeshSurface[])FindObjectsByType(typeof(NavMeshSurface), FindObjectsSortMode.None);

            foreach(NavMeshSurface surface in allNavMeshSurfaces)
            {
                surface.BuildNavMesh();
            }
            Debug.Log("BuildTools: Bake NavMesh done.");
        }

        private static void BakeOcclusion()
        {
            Debug.Log("BuildTools: Bake Occlusion...");
            StaticOcclusionCulling.Compute();
            Debug.Log("BuildTools: Bake Occlusion done.");
        }

        private static string[] GetScenePaths()
        {
            AdditiveSceneLoadManager loadManager = FindObjectOfType<AdditiveSceneLoadManager>();
            if (!loadManager)
            {
                Debug.Log("BuildTools: BakeLighting: Can't find AdditiveSceneLoader!");
                return null;
            }

            List<string> paths = new();
            foreach (AdditiveScene additiveScene in loadManager.additiveSceneSettings.additiveScenes)
            {
                string fullPath = $"{Application.dataPath}/{loadManager.additiveSceneSettings.scenePath}/{additiveScene.sceneName}.unity";
                paths.Add(fullPath);
            }

            return paths.ToArray();
        }
    }
}