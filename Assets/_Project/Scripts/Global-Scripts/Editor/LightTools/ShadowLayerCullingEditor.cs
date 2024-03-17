using DaftAppleGames.Common.CameraTools;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;

namespace DaftAppleGames.Editor.Lighting
{
    [CustomEditor(typeof(ShadowLayerCulling))]
    public class ShadowLayerCullingEditor : OdinEditor
    {
        public ShadowLayerCulling shadowCulling;

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            shadowCulling = target as ShadowLayerCulling;
            if (GUILayout.Button("Apply To Directional Light"))
            {
                ApplyToLight();
            }
            
            if (GUILayout.Button("Refresh Layers"))
            {
                RefreshLayerNames();
            }
            
            if (GUILayout.Button("Reset On Light"))
            {
                ResetLightSettings();
            }
        }

        /// <summary>
        /// Refresh the layer names in the array
        /// </summary>
        private void RefreshLayerNames()
        {
            shadowCulling.cullingSettings.RefreshLayerNames();
        }
        
        /// <summary>
        /// Remove culling settings from camera
        /// </summary>
        private void ResetLightSettings()
        {
            Light light = shadowCulling.gameObject.GetComponent<Light>();
            if (light)
            {
                light.layerShadowCullDistances = null;
            }
        }
        
        /// <summary>
        /// Apply shadow culling layer distances to light
        /// </summary>
        private void ApplyToLight()
        {
            Light light = shadowCulling.gameObject.GetComponent<Light>();
            if (light)
            {
                float[] layerCullDistances = new float[32]; 
                for (int i = 0; i < 32; i++)
                {
                    layerCullDistances[i] = shadowCulling.cullingSettings.layerCullingDistances[i].cullingDistance;
                }

                light.layerShadowCullDistances = layerCullDistances;
                Debug.Log("Light Shadow Culling Applied Successfully!");
            }
        }
    }
}