using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.Rendering.HighDefinition;

namespace DaftAppleGames.Common.Lighting
{
    /// <summary>
    /// Class to manage update of Shadow Cascades on a Light
    /// </summary>
    public class ShadowCascadeUpdater : MonoBehaviour
    {
        [BoxGroup("Settings")] public int numberOfCascades;

        private Light _light;
        private HDAdditionalLightData _additionalLightData;

        // Keep track of which cascade to refresh
        private int _currentCascadeIndex = 0;

        /// <summary>
        /// Configure the component
        /// </summary>
        private void Awake()
        {
            _light = GetComponent<Light>();
            _additionalLightData = GetComponent<HDAdditionalLightData>();

            // Set the shadow update mode to 'OnDemand'
            _additionalLightData.SetShadowUpdateMode(ShadowUpdateMode.OnDemand);
        }

        /// <summary>
        /// Update one cascade at a time per frame
        /// </summary>
        private void FixedUpdate()
        {
            _additionalLightData.RequestSubShadowMapRendering(_currentCascadeIndex);
            if (_currentCascadeIndex == numberOfCascades-1 )
            {
                _currentCascadeIndex = 0;
            }
            else
            {
                _currentCascadeIndex++;
            }
        }
    }
}
