using DaftAppleGames.Common.Buildings;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DaftAppleGames.Common.Lighting
{
    public class LightingManager : MonoBehaviour
    {
        // Singleton static instance
        private static LightingManager _instance;
        public static LightingManager Instance => _instance;

        [BoxGroup("Debug")] [SerializeField] private BuildingLights[] _buildingLightControllers;

        /// <summary>
        /// Initialise the LightingManager Singleton instance
        /// </summary>
        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(this.gameObject);
            }
            else
            {
                _instance = this;
            }
        }

        /// <summary>
        /// Updates the list of building lights. Should be called when all scenes are loaded
        /// </summary>
        public void InitBuildingLightList()
        {
            // Init the list of BuildingLightControllers
            _buildingLightControllers = FindObjectsOfType<BuildingLights>(true);
        }

        /// <summary>
        /// Refresh all Building Light probes
        /// </summary>
        public void RefreshReflectionProbes()
        {
            foreach (BuildingLights buildingLightController in _buildingLightControllers)
            {
                buildingLightController.RefreshInteriorProbes();
            }
        }
    }
}