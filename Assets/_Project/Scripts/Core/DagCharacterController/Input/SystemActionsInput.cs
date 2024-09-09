using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace DaftAppleGames.Darskerry.Core.DagCharacterController.Input
{
    [DefaultExecutionOrder(-2)]
    public class SystemActionsInput : MonoBehaviour, PlayerControls.ISystemActionsMapActions
    {
        #region Class Variables

        [BoxGroup("Events")] [SerializeField] public UnityEvent pausePressedEvent;
        #endregion

        #region Startup
        private void OnEnable()
        {
            if (PlayerInputManager.Instance?.PlayerControls == null)
            {
                Debug.LogError("Player controls is not initialized - cannot enable");
                return;
            }

            PlayerInputManager.Instance.PlayerControls.SystemActionsMap.Enable();
            PlayerInputManager.Instance.PlayerControls.SystemActionsMap.SetCallbacks(this);
        }

        private void OnDisable()
        {
            if (PlayerInputManager.Instance?.PlayerControls == null)
            {
                Debug.LogError("Player controls is not initialized - cannot disable");
                return;
            }

            PlayerInputManager.Instance.PlayerControls.SystemActionsMap.Disable();
            PlayerInputManager.Instance.PlayerControls.SystemActionsMap.RemoveCallbacks(this);
        }
        #endregion

        #region Input Callbacks
        public void OnPause(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                pausePressedEvent.Invoke();
            }
        }
        #endregion

    }
}