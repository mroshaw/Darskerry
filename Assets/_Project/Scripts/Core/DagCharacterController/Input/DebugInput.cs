using UnityEngine;
using UnityEngine.InputSystem;

namespace DaftAppleGames.Darskerry.Core.DagCharacterController.Input
{
    [DefaultExecutionOrder(-2)]
    public class DebugInput : MonoBehaviour, PlayerControls.IDebugMapActions
    {
        #region Class Variables
        public bool ToggleGPUIPressed { get; private set; }
        #endregion

        #region Startup
        private void Awake()
        {

        }
        private void OnEnable()
        {
            if (PlayerInputManager.Instance?.PlayerControls == null)
            {
                Debug.LogError("Player controls is not initialized - cannot enable");
                return;
            }

            PlayerInputManager.Instance.PlayerControls.DebugMap.Enable();
            PlayerInputManager.Instance.PlayerControls.DebugMap.SetCallbacks(this);
        }

        private void OnDisable()
        {
            if (PlayerInputManager.Instance?.PlayerControls == null)
            {
                Debug.LogError("Player controls is not initialized - cannot disable");
                return;
            }

            PlayerInputManager.Instance.PlayerControls.DebugMap.Disable();
            PlayerInputManager.Instance.PlayerControls.DebugMap.RemoveCallbacks(this);
        }
        #endregion

        #region Update
        private void LateUpdate()
        {
            ToggleGPUIPressed = false;
        }
        #endregion

        public void OnToggleGPUI(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                ToggleGPUIPressed = true;
            }
        }
    }
}