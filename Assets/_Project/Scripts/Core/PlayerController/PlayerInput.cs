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

        private Character _character;

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

            _character = GetComponent<Character>();
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            // Read input values

            Vector2 inputMovement = context.ReadValue<Vector2>();

            // Compose a movement direction vector in world space

            Vector3 movementDirection = Vector3.zero;

            movementDirection += Vector3.forward * inputMovement.y;
            movementDirection += Vector3.right * inputMovement.x;

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

        public void OnAttack(InputAction.CallbackContext context)
        {

        }

        public void OnGather(InputAction.CallbackContext context)
        {

        }

        public void OnSprint(InputAction.CallbackContext context)
        {

        }

        /// <summary>
        /// Jump InputAction event handler.
        /// </summary>

        public void OnJump(InputAction.CallbackContext context)
        {
            if (context.started)
                _character.Jump();
            else if (context.canceled)
                _character.StopJumping();
        }

        public void OnWalk(InputAction.CallbackContext context)
        {

        }

        public void OnRoll(InputAction.CallbackContext context)
        {

        }

        /// <summary>
        /// Crouch InputAction event handler.
        /// </summary>

        public void OnCrouch(InputAction.CallbackContext context)
        {
            if (context.started)
                _character.Crouch();
            else if (context.canceled)
                _character.UnCrouch();
        }
    }
}
