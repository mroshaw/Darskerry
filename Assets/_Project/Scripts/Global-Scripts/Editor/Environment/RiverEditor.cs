using DaftAppleGames.Common.Environment;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;

namespace DaftAppleGames.Editor.Environment
{
    [CustomEditor(typeof(River))]
    public class RiverEditor : OdinEditor
    {
        [Header("Settings")]
        public string waterLayer = "Water";
        public string waterTag = "Water";
        
        public River river;
        
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            river = target as River;

            // Force show debug button
            if (GUILayout.Button("Update"))
            {
                UpdateWaterComponents();
            }
        }

        private void UpdateWaterComponents()
        {
            // Get RamSpline GameObject
            GameObject ramSpline = river.transform.Find("RamSpline").gameObject;
            if (ramSpline)
            {
                // Iterate over meshes
                foreach (Transform childMesh in ramSpline.transform)
                {
                    GameObject childGo = childMesh.gameObject;
                    
                    Debug.Log($"Processing child: {childGo.name}");
                    
                    // Set the layer and tag
                    childGo.layer = LayerMask.NameToLayer(waterLayer);
                    childGo.tag = waterTag;
                    
                    // Add Mesh Collider and configure
                    MeshCollider collider = childGo.GetComponent<MeshCollider>();
                    if (!collider)
                    {
                        collider = childGo.AddComponent<MeshCollider>();
                    }
                    collider.convex = true;
                    collider.isTrigger = true;
                    
                    // Add Water component
                    // Add Mesh Collider and configure
                    Water water = childGo.GetComponent<Water>();
                    if (!water)
                    {
                        water = childGo.AddComponent<Water>();
                    }
                    
                    Debug.Log($"Done processing child: {childGo.name}!");
                }
            }
        }
    }

}
