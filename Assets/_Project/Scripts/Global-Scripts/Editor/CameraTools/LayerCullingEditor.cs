using DaftAppleGames.Common.CameraTools;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;

namespace DaftAppleGames.Editor.CameraTools
{
    [CustomEditor(typeof(LayerCulling))]
    public class LayerCullingEditor : OdinEditor
    {
        public LayerCulling layerCulling;

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            layerCulling = target as LayerCulling;
            if (GUILayout.Button("Apply To Camera"))
            {
                ApplyToCamera();
            }
            
            if (GUILayout.Button("Refresh Layers"))
            {
                RefreshLayerNames();
            }
            
            if (GUILayout.Button("Reset On Camera"))
            {
                ResetCameraSettings();
            }
        }

        /// <summary>
        /// Refresh the layer names in the array
        /// </summary>
        private void RefreshLayerNames()
        {
            layerCulling.cullingSettings.RefreshLayerNames();
        }

        /// <summary>
        /// Remove culling settings from camera
        /// </summary>
        private void ResetCameraSettings()
        {
            Camera camera = layerCulling.gameObject.GetComponent<Camera>();
            if (camera)
            {
                float[] zeroArray = new float[32];
                for (int i = 0; i < 32; i++)
                {
                    zeroArray[i] = 0.0f;
                }
                camera.layerCullDistances = zeroArray;
            }
            Debug.Log("Camera reset!");
        }
        
        /// <summary>
        /// Apply culling layer distances to camera
        /// </summary>
        private void ApplyToCamera()
        {
            Camera camera = layerCulling.gameObject.GetComponent<Camera>();
            if (camera)
            {
                float[] layerCullDistances = new float[32]; 
                for (int i = 0; i < 32; i++)
                {
                    layerCullDistances[i] = layerCulling.cullingSettings.layerCullingDistances[i].cullingDistance;
                }

                camera.layerCullDistances = layerCullDistances;
                Debug.Log("Camera Layer Culling Applied Successfully!");
            }
        }
    }
}