using DaftAppleGames.Common.GameControllers;
using UnityEngine;
using TheVegetationEngine;

namespace DaftAppleGames.Common.LightingTools
{
    public class AddMainLightToTVEManager : MonoBehaviour
    {
        /// <summary>
        /// Configure the component on awake
        /// </summary>   
        private void Start()
        {
            TVEManager.Instance.globalControl.mainLight = PlayerCameraManager.Instance.MainDirectionalLight;
        }
    }
}
