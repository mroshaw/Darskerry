using UnityEngine;

namespace DaftAppleGames.Common.GameControllers
{
    public class PlayerCameraMethods : MonoBehaviour
    {
        /// <summary>
        /// Get the MainCamera camera
        /// </summary>
        /// <returns></returns>
        public Camera GetMainCamera()
        {
            return PlayerCameraManager.Instance.MainCamera;
        }

        /// <summary>
        /// Pause any attached rotator
        /// </summary>
        public void PauseCameraRotation()
        {
            PlayerCameraManager.Instance.PauseCameraRotation();
        }

        /// <summary>
        /// Resume any attached rotator
        /// </summary>
        public void ResumeCameraRotation()
        {
            PlayerCameraManager.Instance.ResumeCameraRotation();
        }
    }
}
