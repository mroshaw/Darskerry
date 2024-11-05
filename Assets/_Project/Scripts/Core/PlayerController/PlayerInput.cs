using ECM2;
using UnityEngine;
using UnityEngine.InputSystem;

namespace DaftAppleGames.Darskerry.Core.PlayerController
{
    /// <summary>
    /// This example shows how to make use of the new Input System,
    /// in particular, the PlayerInput component to control a character.
    /// </summary>

    public class PlayerInput : MonoBehaviour, PlayerControls.ICharacterControlsActions
    {
        /// <summary>
        /// Cached controlled character
        /// </summary>

        private PlayerCharacter _character;

        private Vector2 _lookInput;
        [SerializeField] private Vector2 _moveInput;

        private void OnEnable()
        {
            PlayerCharacterInputManager.Instance.PlayerControls.CharacterControls.Enable();
            PlayerCharacterInputManager.Instance.PlayerControls.CharacterControls.SetCallbacks(this);
        }

        private void OnDisable()
        {
            PlayerCharacterInputManager.Instance.PlayerControls.CharacterControls.Disable();
            PlayerCharacterInputManager.Instance.PlayerControls.CharacterControls.RemoveCallbacks(this);
        }

        /// <summary>
        /// Movement InputAction event handler.
        /// </summary>
        private void Awake()
        {
            //
            // Cache controlled character.

            _character = GetComponent<PlayerCharacter>();
        }

        private void Update()
        {
            // Compose a movement direction vector in world space
            Vector3 movementDirection = Vector3.zero;
            movementDirection += Vector3.forward * _moveInput.y;
            movementDirection += Vector3.right * _moveInput.x;

            // If character has a camera assigned,
            // make movement direction relative to this camera view direction
            if (_character.camera)
            {
                movementDirection
                    = movementDirection.relativeTo(_character.cameraTransform);
            }

            // Set character's movement direction vector
            _character.SetMovementDirection(movementDirection);
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            if (context.started || context.performed)
            {
                // Read input values
                _moveInput = context.ReadValue<Vector2>();
            }
            else if (context.canceled)
            {
                _moveInput = Vector2.zero;
            }
        }

        /// <summary>
        /// Specific move implementation for GamePad, to allow for sensitivity settings
        /// </summary>
        /// <param name="context"></param>
        public void OnMoveGamepad(InputAction.CallbackContext context)
        {
            if (context.started || context.performed)
            {
                // Read input values
                _moveInput = context.ReadValue<Vector2>();
            }
            else if (context.canceled)
            {
                _moveInput = Vector2.zero;
            }
        }

        public void OnAttack(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                _character.Attack();
            }
            else if (context.canceled)
            {
                _character.StopAttacking();
            }
        }

        public void OnGather(InputAction.CallbackContext context)
        {

        }

        public void OnSprint(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                _character.Sprint();
            }
            else if (context.canceled)
            {
                _character.StopSprinting();
            }
        }

        /// <summary>
        /// Jump InputAction event handler.
        /// </summary>

        public void OnJump(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                _character.Jump();
            }
            else if (context.canceled)
            {
                _character.StopJumping();
            }
        }

        public void OnWalk(InputAction.CallbackContext context)
        {

        }

        public void OnRoll(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                _character.Roll();
            }
            else if (context.canceled)
            {
                _character.StopRollingInput();
            }
        }

        /// <summary>
        /// Crouch InputAction event handler.
        /// </summary>

        public void OnCrouch(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                _character.Crouch();
            }
            else if (context.canceled)
            {
                _character.UnCrouch();
            }
        }
    }
}
