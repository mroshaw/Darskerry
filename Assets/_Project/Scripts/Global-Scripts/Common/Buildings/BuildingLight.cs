using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

namespace DaftAppleGames.Common.Buildings
{
    public enum BuildingLightType { Candle, Torch, Fireplace, Cooking, StreetLamp }
    public enum BuildingLightLocation {Interior, Exterior }

    public class BuildingLight : MonoBehaviour
    {
        // Public serializable properties
        [BoxGroup("General Settings")] public BuildingLightType lightType;
        [BoxGroup("General Settings")] public BuildingLightLocation lightLocation;

        public Light Light => GetComponent<Light>();

        [BoxGroup("Debug")] public ParticleSystem[] particleSystems;
        [BoxGroup("Debug")] public LensFlareComponentSRP[] lensFlares;

        /// <summary>
        /// Set up the component
        /// </summary>
        private void Start()
        {
            RefreshVisualEffects();
        }

        /// <summary>
        /// Refresh the particle systems associated to the light
        /// </summary>
        public void RefreshVisualEffects()
        {
            particleSystems = gameObject.transform.parent.GetComponentsInChildren<ParticleSystem>(true);
            lensFlares = gameObject.transform.parent.GetComponentsInChildren<LensFlareComponentSRP>(true);
        }

        /// <summary>
        /// Turn on the light
        /// </summary>
        ///
        [Button("Turn On")]
        public void TurnOn()
        {
            SetLightState(true);
        }

        /// <summary>
        /// Turn off the light
        /// </summary>
        [Button("Turn Off")]
        public void TurnOff()
        {
            SetLightState(false);
        }

        /// <summary>
        /// Sets the state of the light
        /// </summary>
        /// <param name="state"></param>
        public void SetLightState(bool state)
        {
            Light.enabled = state;
            foreach (ParticleSystem currentParticleSystem in particleSystems)
            {
                currentParticleSystem.gameObject.SetActive(state);
            }

            foreach (LensFlareComponentSRP lensFlare in lensFlares)
            {
                lensFlare.gameObject.SetActive(state);
            }

            UpdateShadowMap();
        }

        /// <summary>
        /// Updates the ShadowMap on the light, if it's an HDRP light
        /// </summary>
        private void UpdateShadowMap()
        {
            HDAdditionalLightData additionalData = Light.GetComponent<HDAdditionalLightData>();
            if (!additionalData)
            {
                return;
            }
            additionalData.RequestShadowMapRendering();
        }
    }
}
