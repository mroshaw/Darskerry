using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DaftAppleGames.Common.CameraTools
{
    [CreateAssetMenu(fileName = "CameraLayerCullingSettings", menuName = "Game/CameraCullingLayers", order = 1)]
    [InlineEditor()]
    public class LayerCullingSettings : ScriptableObject
    {
        [Header("Settings")]
        public LayerCullingDistance[] layerCullingDistances;
        
        /// <summary>
        /// Class to store layer name / distance pairs
        /// </summary>
        [Serializable]
        public class LayerCullingDistance
        {
            public string layerName;
            public float cullingDistance;
        }

        /// <summary>
        /// Pre-populate layer names
        /// </summary>
        private void OnEnable()
        {
            if (layerCullingDistances == null || layerCullingDistances.Length == 0)
            {
                layerCullingDistances = new LayerCullingDistance[32];
                PopulateLayerNames();
            }
        }

        /// <summary>
        /// Refresh Layer Names
        /// </summary>
        [Button("Refresh")]
        public void RefreshLayerNames()
        {
            for (int i = 0; i < 32; i++)
            {
                layerCullingDistances[i].layerName = LayerMask.LayerToName(i);
            }
        }
        
        /// <summary>
        /// Populate the array with known layer names
        /// </summary>
        private void PopulateLayerNames()
        {
            for (int i = 0; i < 32; i++)
            {
                Debug.Log("${i}");
                LayerCullingDistance newEntry = new LayerCullingDistance
                {
                    layerName = LayerMask.LayerToName(i)
                };
                layerCullingDistances[i] = newEntry;
            }
            
        }
        
    }
}
