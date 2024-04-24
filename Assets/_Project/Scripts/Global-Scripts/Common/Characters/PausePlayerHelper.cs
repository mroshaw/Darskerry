#if PIXELCRUSHERS
#if INVECTOR_SHOOTER
using DaftAppleGames.Common.GameControllers;
using MalbersAnimations.HAP;
using PixelCrushers.InvectorSupport;
using Invector.vCharacterController;
using MalbersAnimations;
using UnityEngine;

namespace DaftAppleGames.Common.Characters
{
    /// <summary>
    /// Helper component to call the PixelCrushers pause and unpause player functions
    /// </summary>
    public class PausePlayerHelper : MonoBehaviour
    {

        private bool _isPlayerPaused = false;
        private bool _isHorsePaused = false;

        private RigidbodyConstraints _playerConstraints;

        /// <summary>
        /// Pause Player
        /// </summary>
        public void PausePlayer()
        {
            if (!PlayerCameraManager.Instance)
            {
                return;
            }

            GameObject playerGameObject = PlayerCameraManager.Instance.PlayerGameObject;

            PlayerCameraManager.Instance.InvectorMainCamera.FreezeCamera();
            playerGameObject.GetComponent<vThirdPersonController>().StopCharacter();
            _playerConstraints = playerGameObject.GetComponent<Rigidbody>().constraints;
            playerGameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            InvectorPlayerUtility.PausePlayer();
            PauseHorse();
            _isPlayerPaused = true;
            // Debug.Log("Player Paused");

        }

        /// <summary>
        /// Unpause Player
        /// </summary>
        public void UnpausePlayer()
        {
            if (!PlayerCameraManager.Instance)
            {
                return;
            }

            if (_isPlayerPaused)
            {
                GameObject playerGameObject = PlayerCameraManager.Instance.PlayerGameObject;

                PlayerCameraManager.Instance.InvectorMainCamera.UnFreezeCamera();
                playerGameObject.GetComponent<Rigidbody>().constraints = _playerConstraints;
                InvectorPlayerUtility.UnpausePlayer();
                UnpauseHorse();
                _isPlayerPaused = false;
                // Debug.Log("Player Unpaused");

            }
        }

        /// <summary>
        /// If player is riding, pause horse
        /// </summary>
        public void PauseHorse()
        {
            MRider rider = PlayerCameraManager.Instance.PlayerGameObject.GetComponent<MRider>();

            if (rider.IsRiding)
            {
                Mount mount = rider.Montura;
                MalbersInput mInput = mount.GetComponentInParent<MalbersInput>();
                mInput.enabled = false;
                _isHorsePaused = true;
                // Debug.Log("Horse Paused");
            }

        }

        /// <summary>
        /// If player is riding, unpause horse
        /// </summary>
        public void UnpauseHorse()
        {
            if (_isHorsePaused)
            {
                MRider rider = PlayerCameraManager.Instance.PlayerGameObject.GetComponent<MRider>();

                Mount mount = rider.Montura;
                MalbersInput mInput = mount.GetComponentInParent<MalbersInput>();
                mInput.enabled = true;
                _isHorsePaused = false;
                // Debug.Log("Horse Unpaused");

            }
        }
    }
}
#endif
#endif