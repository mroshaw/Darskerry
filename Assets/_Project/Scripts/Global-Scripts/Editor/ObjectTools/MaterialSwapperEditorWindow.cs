using DaftAppleGames.Editor.Meshes;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;

namespace DaftApplesGames.Editor.ObjectTools
{
    public class MaterialSwapperEditorWindow : OdinEditorWindow
    {
        // Display Editor Window
        [MenuItem("Daft Apple Games/Tools/Objects/Material swapper")]
        public static void ShowWindow()
        {
            GetWindow(typeof(MaterialSwapperEditorWindow));
        }

        [BoxGroup("Source Objects", centerLabel: true)]
        public GameObject[] selectedGameObjects;

        [BoxGroup("Settings")] public MaterialSwapperSettings swapperSettings;

        /// <summary>
        /// Refresh the selected list when scene object selection changes
        /// </summary>
        private void OnSelectionChange()
        {
            selectedGameObjects = Selection.gameObjects;
        }


        [Button("Swap Mesh Materials")]
        public void SwapMeshMaterials()
        {
            swapperSettings.GetMaterials(out Material[] sourceMaterials, out Material[] targetMaterials,
                out string[] sourceMeshNames, out string[] targetMeshNames);

            foreach (GameObject meshGameObject in selectedGameObjects)
            {
                MeshRenderer[] allMeshRenderers = meshGameObject.GetComponentsInChildren<MeshRenderer>(true);
                foreach (MeshRenderer meshRenderer in allMeshRenderers)
                {
                    Material meshMaterial = meshRenderer.sharedMaterial;
                    foreach (MaterialSwapperSettings.MaterialPair materialPair in swapperSettings.materialPairs)
                    {
                        if (materialPair.sourceMaterial == meshMaterial)
                        {
                            meshRenderer.sharedMaterial = materialPair.targetMaterial;
                            Debug.Log(
                                $"SwapMeshMaterials: Swapped material on {meshGameObject.name} from {meshMaterial.name} to {materialPair.targetMaterial.name}");
                        }
                    }
                }

            }
        }

    }
}