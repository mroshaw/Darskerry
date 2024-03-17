using DaftAppleGames.Common.GameControllers;
using UnityEngine;
using TheVegetationEngine;

namespace DaftAppleGames.Common.CameraTools
{
    public class AddCameraToTVEManager : MonoBehaviour
    {
        /// <summary>
        /// Configure the component on awake
        /// </summary>   
        private void Start()
        {
            TVEManager.Instance.globalVolume.mainCamera = PlayerCameraManager.Instance.MainCamera;
        }
    }
}
