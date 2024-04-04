using DaftAppleGames.Common.GameControllers;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.Rendering;

namespace DaftAppleGames.Common.Lighting
{
    public class MainLightController : MonoBehaviour
    {

        private Light _directionalLight;
        private LensFlareComponentSRP _lenFlare;

        /// <summary>
        /// Enables lens flare
        /// </summary>
        public void EnableLenFlare()
        {
            GetDirectionalLight();
            _lenFlare.enabled = true;
        }

        /// <summary>
        /// Disables lens flare
        /// </summary>
        public void DisableLensFlare()
        {
            GetDirectionalLight();
            _lenFlare.enabled = false;
        }

        /// <summary>
        /// Finds and cachaes the Directional Light
        /// </summary>
        private void GetDirectionalLight()
        {
            if (!_directionalLight)
            {
                _directionalLight = PlayerCameraManager.Instance.MainDirectionalLight;
                _lenFlare = _directionalLight.GetComponent<LensFlareComponentSRP>();
            }
        }
    }
}
