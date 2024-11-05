using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace DaftAppleGames.Darskerry.Core.PlayerController
{
    [DefaultExecutionOrder(-2)]
    public class SystemActionsInput : MonoBehaviour, PlayerControls.ISystemControlsActions
    {
        #region Class Variables

        [BoxGroup("Events")][SerializeField] public UnityEvent pausePressedEvent;
        #endregion

        #region Startup
        private void OnEnable()
        {
            // StartCoroutine(RegisterWithInputManagerAsync());

            if (PlayerCharacterInputManager.Instance?.PlayerControls == null)
            {
                Debug.LogError("Player controls is not initialized - cannot enable");
                return;
            }

            PlayerCharacterInputManager.Instance.PlayerControls.SystemControls.Enable();
            PlayerCharacterInputManager.Instance.PlayerControls.SystemControls.SetCallbacks(this);
        }

        private void OnDisable()
        {
            if (PlayerCharacterInputManager.Instance?.PlayerControls == null)
            {
                Debug.LogError("Player controls is not initialized - cannot disable");
                return;
            }

            PlayerCharacterInputManager.Instance.PlayerControls.SystemControls.Disable();
            PlayerCharacterInputManager.Instance.PlayerControls.SystemControls.RemoveCallbacks(this);
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

        private IEnumerator RegisterWithInputManagerAsync()
        {
            while (PlayerCharacterInputManager.Instance == null || PlayerCharacterInputManager.Instance.PlayerControls == null)
            {
                yield return null;
            }
            Debug.Log("SystemActionsInpt: Found PlayerCharacterInputManager!");
            PlayerCharacterInputManager.Instance.PlayerControls.SystemControls.Enable();
            PlayerCharacterInputManager.Instance.PlayerControls.SystemControls.SetCallbacks(this);
        }

        #endregion

    }
}