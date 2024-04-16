using System.Collections.Generic;
using GPUInstancer;
using UnityEngine;

namespace DaftAppleGames.Common.Terrains
{
    public class TerrainHelper : MonoBehaviour
    {
        private List<GPUInstancerManager> _detailManagers;
        private List<GPUInstancerManager> _treeManagers;

        /// <summary>
        /// Setup terrain components
        /// </summary>
        private void Start()
        {
            _detailManagers = new List<GPUInstancerManager>();
            _treeManagers = new List<GPUInstancerManager>();

            foreach (GPUInstancerManager gpuInstancerManager in GPUInstancerAPI.GetActiveManagers())
            {
                if (gpuInstancerManager.GetType() == typeof(GPUInstancerDetailManager))
                {
                    _detailManagers.Add(gpuInstancerManager);
                }

                if (gpuInstancerManager.GetType() == typeof(GPUInstancerTreeManager))
                {
                    _treeManagers.Add(gpuInstancerManager);
                }
            }
        }

        /// <summary>
        /// Enables or disables detail rendering
        /// </summary>
        /// <param name="state"></param>
        public void SetDetailState(bool state)
        {
            foreach (GPUInstancerManager manager in _detailManagers)
            {
                manager.enabled = state;
            }
        }

        /// <summary>
        /// Enables or disables tree rendering
        /// </summary>
        /// <param name="state"></param>
        public void SetTreeState(bool state)
        {
            foreach (GPUInstancerManager manager in _treeManagers)
            {
                manager.enabled = state;
            }
        }
    }
}