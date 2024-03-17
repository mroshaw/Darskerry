using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEditor;

using UnityEngine;

namespace DaftAppleGames.Scripts.Editor.Buildings
{
    public class CleanUp3DForgeEditorWindow : OdinEditorWindow
    {
        [MenuItem("Window/Buildings/Clean Up 3D Forge Assets")]
        public static void ShowWindow()
        {
            GetWindow(typeof(CleanUp3DForgeEditorWindow));
        }
    
        // UI layout
        [BoxGroup("Settings")] public string assetPath = "Assets/3DForge";
        [BoxGroup("Settings")] public string shaderToFindPath = "Legacy Shaders/Transparent/Diffuse";
        [BoxGroup("Settings")] public bool simulate = true;
        [BoxGroup("Settings")] public bool countOnly = true;
        [BoxGroup("Settings")] public bool detailReport = true;
        [BoxGroup("Settings")] public bool changeFirstOnly = true;

        private int _assetsUpdated;
        private int _gameObjectsDestroyed;

        [Button("Clear Up Mesh Colliders")]
        private void CleanMeshColliders()
        {
            _assetsUpdated = 0;
            _gameObjectsDestroyed = 0;

            if (detailReport)
            {
                Debug.Log("CleanUp3DForge.CleanMeshColliders: Looking for shader...");
            }

            Shader shaderToFind = Shader.Find(shaderToFindPath);
            if (shaderToFind == null)
            {
                Debug.Log($"CleanUp3DForge.CleanMeshColliders: Couldn't find shader {shaderToFindPath}. Aborting!");
                return;
            }

            if (detailReport)
            {
                Debug.Log($"CleanUp3DForge.CleanMeshColliders: Found shader {shaderToFind.name}");
            }

            string[] allAssetPaths = GetPrefabAssetPaths();


            if (allAssetPaths.Length == 0)
            {
                Debug.Log($"CleanUp3DForge.CleanMeshColliders: No prefab assets found! Check the path! Aborting!");
                return;
            }

            Debug.Log($"CleanUp3DForge.CleanMeshColliders: Processing {allAssetPaths.Length} assets.");
            if (countOnly)
            {
                return;
            }

            foreach (string assetPath in allAssetPaths)
            {
                if (changeFirstOnly && _assetsUpdated > 0)
                {
                    break;
                }

                if (detailReport)
                {
                    Debug.Log($"CleanUp3DForge.CleanMeshColliders: Checking prefab at {assetPath}...");
                }
                
                GameObject prefabAsset = AssetDatabase.LoadAssetAtPath(assetPath, typeof(GameObject)) as GameObject;
                RemoveUnwantedMeshColliders(prefabAsset, shaderToFind);
                if (detailReport)
                {
                    Debug.Log($"CleanUp3DForge.CleanMeshColliders: Checking prefab at {assetPath}. Done.");
                }
            }

            Debug.Log($"CleanUp3DForge.CleanMeshColliders: Processed {allAssetPaths.Length} assets. Updated {_assetsUpdated} assets, destroyed {_gameObjectsDestroyed} Game Objects.");
        }

        /// <summary>
        /// Removes GameObjects with unwanted mesh colliders (e.g. stair colliders)
        /// </summary>
        /// <param name="prefabGameObject"></param>
        private void RemoveUnwantedMeshColliders(GameObject prefabGameObject, Shader shaderToFind)
        {
            bool assetUpdated = false;

            MeshCollider[] allMeshColliders = prefabGameObject.GetComponentsInChildren<MeshCollider>(true);
            if (allMeshColliders.Length == 0)
            {
                if (detailReport)
                {
                    Debug.Log($"CleanUp3DForge.RemoveUnwantedMeshColliders: No MeshColliders found. Skipping {prefabGameObject.name}");
                }
                return;

            }
            foreach (MeshCollider collider in allMeshColliders)
            {
                MeshRenderer meshRenderer = collider.GetComponent<MeshRenderer>();
                if (meshRenderer != null)
                {
                    if (detailReport)
                    {
                        Debug.Log($"CleanUp3DForge.RemoveUnwantedMeshColliders: Looking for shader on MeshRenderer...");
                    }
                    if (meshRenderer.sharedMaterial.shader.name.Equals(shaderToFind.name, StringComparison.OrdinalIgnoreCase))
                    {
                        GameObject childGameObject = meshRenderer.gameObject;
                        Debug.Log($"CleanUp3DForge.RemoveUnwantedMeshColliders: Found unwanted collider shader on {prefabGameObject.name} in child {childGameObject.name}.");
                        if (!simulate)
                        {
                            Debug.Log($"CleanUp3DForge.RemoveUnwantedMeshColliders: Removing child GameObject {childGameObject.name}");
                            // DestroyImmediate(childGameObject);
                            childGameObject.SetActive(false);
                            EditorUtility.SetDirty(prefabGameObject);
                            Debug.Log($"CleanUp3DForge.RemoveUnwantedMeshColliders: Removing child GameObject {childGameObject.name}. Done.");
                        }
                        _gameObjectsDestroyed++;
                        assetUpdated = true;
                    }
                }
            }

            if (assetUpdated)
            {
                _assetsUpdated++;
            }
        }

        /// <summary>
        /// Get all 3D Forge Prefab assets paths
        /// </summary>
        /// <returns></returns>
        private string[] GetPrefabAssetPaths()
        {
            List<string> assetPathList = new List<string>(); 
            string[] assetGuids = AssetDatabase.FindAssets("t:prefab", new string[] { assetPath });
            foreach (string guid in assetGuids)
            {
                assetPathList.Add(AssetDatabase.GUIDToAssetPath(guid));
            }

            return assetPathList.ToArray();
        }
    }
}
