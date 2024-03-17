using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DaftAppleGames.Common.Buildings
{
    public class BuildingMeshes : MonoBehaviour
    {
        [FoldoutGroup("Interior Mesh Settings")] public GameObject[] intGameObjects;
        [FoldoutGroup("Interior Mesh Settings")] public GameObject[] intPropGameObjects;
        [InlineEditor][FoldoutGroup("Interior Mesh Settings")] public BuildingMeshSettings intMeshSettings;
        [InlineEditor][FoldoutGroup("Interior Mesh Settings")] public BuildingMeshSettings intPropMeshSettings;

        [FoldoutGroup("Exterior Mesh Settings")] public GameObject[] extGameObjects;
        [FoldoutGroup("Exterior Mesh Settings")] public GameObject[] extPropGameObjects;
        [InlineEditor][FoldoutGroup("Exterior Mesh Settings")] public BuildingMeshSettings extMeshSettings;
        [InlineEditor][FoldoutGroup("Exterior Mesh Settings")] public BuildingMeshSettings extPropMeshSettings;

        [FoldoutGroup("Interior Meshes")] public List<MeshRenderer> intMeshes;
        [FoldoutGroup("Interior Meshes")] public List<MeshRenderer> intPropMeshes;
        [FoldoutGroup("Exterior Meshes")] public List<MeshRenderer> extMeshes;
        [FoldoutGroup("Exterior Meshes")] public List<MeshRenderer> extPropMeshes;

        /// <summary>
        /// Set up the component
        /// </summary>
        private void Start()
        {
    
        }

        [Button("Refresh Mesh Lists")]
        private void RefreshMeshLists()
        {
            intMeshes = new List<MeshRenderer>();
            intPropMeshes = new List<MeshRenderer>();
            extMeshes = new List<MeshRenderer>();
            extPropMeshes = new List<MeshRenderer>();

            // Interior Meshes
            if (intGameObjects.Length > 0)
            {
                foreach (GameObject intGameObject in intGameObjects)
                {
                    MeshRenderer[] allIntMeshRenderers = intGameObject.GetComponentsInChildren<MeshRenderer>(true);
                    List<MeshRenderer> allIntMeshRenderersList = new List<MeshRenderer>(allIntMeshRenderers);

                    if (intPropGameObjects.Length > 0)
                    {
                        List<MeshRenderer> allIntProprMeshRenderersList = new List<MeshRenderer>();
                        foreach (GameObject intPropGameObject in intPropGameObjects)
                        {
                            MeshRenderer[] allIntPropMeshRenderers = intPropGameObject.GetComponentsInChildren<MeshRenderer>(true);
                            allIntProprMeshRenderersList.AddRange(allIntPropMeshRenderers);
                        }
                        // Remove prop meshes from full list
                        intMeshes = allIntMeshRenderersList.Except(allIntProprMeshRenderersList).ToList();
                        intPropMeshes = allIntProprMeshRenderersList;
                    }
                    else
                    {
                        intMeshes = allIntMeshRenderersList;
                    }


                }
            }

            if (extGameObjects.Length > 0)
            {
                foreach (GameObject extGameObject in extGameObjects)
                {
                    MeshRenderer[] allExtMeshRenderers = extGameObject.GetComponentsInChildren<MeshRenderer>(true);
                    List<MeshRenderer> allExtMeshRenderersList = new List<MeshRenderer>(allExtMeshRenderers);


                    if (extPropGameObjects.Length > 0)
                    {
                        List<MeshRenderer> allExtPropMeshRenderersList = new List<MeshRenderer>();
                        foreach (GameObject extPropGameObject in extPropGameObjects)
                        {
                            MeshRenderer[] allExtPropMeshRenderers = extPropGameObject.GetComponentsInChildren<MeshRenderer>(true);
                            allExtPropMeshRenderersList.AddRange(allExtPropMeshRenderers);
                        }

                        // Remove overlapping prop meshes
                        extMeshes = allExtMeshRenderersList.Except(allExtPropMeshRenderersList).ToList();
                        extPropMeshes = allExtPropMeshRenderersList;
                    }
                    else
                    {
                        extMeshes = allExtMeshRenderersList;
                    }
                }

            }

        }

        /// <summary>
        /// Populates lists of all meshes
        /// </summary>
        [Button("Apply Settings")]
        private void ApplyMeshSettings()
        {
            RefreshMeshLists();

            // Apply internal mesh settings
            foreach (MeshRenderer meshRenderer in intMeshes)
            {
                intMeshSettings.ConfigureMesh(meshRenderer);
            }

            // Apply external mesh settings
            foreach (MeshRenderer meshRenderer in extMeshes)
            {
                extMeshSettings.ConfigureMesh(meshRenderer);
            }

            // Apply interior prop settings
            foreach (MeshRenderer meshRenderer in intPropMeshes)
            {
                intPropMeshSettings.ConfigureMesh(meshRenderer);

            }

            // Apply exterior prop settings
            foreach (MeshRenderer meshRenderer in extPropMeshes)
            {
                extPropMeshSettings.ConfigureMesh(meshRenderer);
            }
        }
    }
}