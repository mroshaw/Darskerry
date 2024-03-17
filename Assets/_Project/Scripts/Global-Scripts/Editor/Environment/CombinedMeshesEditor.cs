#if MCS
using DaftAppleGames.Common.Environment;
using MeshCombineStudio;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;

namespace DaftAppleGames.Editor.Environment
{
    [CustomEditor(typeof(CombinedMeshes))]
    public class CombinedMeshesEditor : OdinEditor
    {
        public CombinedMeshes combinedMeshes;
        public override void OnInspectorGUI()
        {
            combinedMeshes = target as CombinedMeshes;
            
            DrawDefaultInspector();
            
            // Update all combined meshes
            if (GUILayout.Button("Update Combined Meshes"))
            {
                UpdateCombinedMeshes();
                SceneView.RepaintAll();
            }

            // Activate originals
            if (GUILayout.Button("Enable Originals"))
            {
                ActivateOriginal();
                SceneView.RepaintAll();
            }

            // Activate combined
            if (GUILayout.Button("Enable Merged"))
            {
                ActivateCombined();
                SceneView.RepaintAll();
            }
            
        }
        
        /// <summary>
        /// Activate all the Original objects
        /// </summary>
        private void ActivateOriginal()
        {
            foreach(MeshCombiner combiner in combinedMeshes.GetComponentsInChildren<MeshCombiner>())
            {
                combiner.activeOriginal = true;
                combiner.ExecuteHandleObjects(true, MeshCombiner.HandleComponent.Disable, MeshCombiner.HandleComponent.Disable);
            }
            Debug.Log($"Done activating original meshes on");
        }

        /// <summary>
        /// Activate all the combined objects
        /// </summary>
        private void ActivateCombined()
        {
            foreach(MeshCombiner combiner in combinedMeshes.GetComponentsInChildren<MeshCombiner>())
            {
                combiner.activeOriginal = false;
                combiner.ExecuteHandleObjects(false, MeshCombiner.HandleComponent.Disable, MeshCombiner.HandleComponent.Disable);
            }
            Debug.Log($"Done activating original meshes on");
        }

                
        private void RestoreAllOriginals()
        {
            
        }
        
        /// <summary>
        /// Is there objects in the Combiner
        /// </summary>
        /// <param name="combiner"></param>
        /// <returns></returns>
        private bool HasFoundObjects(MeshCombiner combiner)
        {
            bool hasFoundObjects = false;
            if (combiner.data != null)
            {
                hasFoundObjects = (combiner.data.foundObjects.Count > 0 || combiner.data.foundLodObjects.Count > 0);
            }

            return hasFoundObjects;
        }

        /// <summary>
        /// Does object contain combined objects
        /// </summary>
        /// <param name="combiner"></param>
        /// <returns></returns>
        private bool HasCombinedObject(MeshCombiner combiner)
        {
            if (!combiner || !combiner.data)
            {
                return false;
            }
            bool hasCombinedChildren = (combiner.transform.childCount > 0) || (combiner.combineMode == CombineMode.DynamicObjects && combiner.data.combinedGameObjects.Count > 0);
            return hasCombinedChildren;
        }
        
        /// <summary>
        /// Update all Mesh Studio components
        /// </summary>
        private void UpdateCombinedMeshes()
        {
            MeshCombiner[] combiners = combinedMeshes.GetComponentsInChildren<MeshCombiner>();

            foreach (MeshCombiner combiner in combiners)
            {
                Debug.Log($"Applying Mesh Combiner: {combiner.gameObject.name}");
                
                Debug.Log($"... Updating search results");
                combiner.AddObjectsAutomatically();
                Debug.Log("... Combining meshes");
                MeshCombineJobManager.ResetMeshCache();
                combiner.CombineAll();
                
                Debug.Log($"Done combining meshes on {combiner.gameObject.name}");
            }
        }
    }
}
#endif