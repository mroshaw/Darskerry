using System.Collections.Generic;
using DaftAppleGames.Common.GameControllers;
using Unity.AI.Navigation;
using UnityEditor;
using UnityEngine;
using Lightmapping = UnityEditor.Lightmapping;

namespace DaftAppleGames.Editor
{
    public class BuildTools : MonoBehaviour
    {
        public static void BakeLighting()
        {
            Debug.Log("BuildTools: Bake Lighting...");
            Lightmapping.BakeMultipleScenes(GetScenePaths());
            Debug.Log("BuildTools: Bake Lighting done.");
        }

        public static void BakeNavMesh()
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

        public static void BakeOcclusion()
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
