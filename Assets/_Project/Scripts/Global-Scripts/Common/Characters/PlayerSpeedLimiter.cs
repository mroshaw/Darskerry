#if INVECTOR_SHOOTER
using Invector.vCharacterController;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DaftAppleGames.Common.Characters 
{
    public class PlayerSpeedLimiter : MonoBehaviour
    {
        private vThirdPersonController _controller;
        private vShooterMeleeInput _input;
        private float _currentWalkSpeed;
        private float _currentRunSpeed;
        private float _currentSprintSpeed;

        /// <summary>
        /// Configure the component
        /// </summary>
        private void Start()
        {
            _controller = GetComponent<vThirdPersonController>();
            _input = GetComponent<vShooterMeleeInput>();
        }

        /// <summary>
        /// Only allow walking
        /// </summary>
        public void WalkOnlyEnabled(bool allowRunning, bool allowSprinting)
        {
            if (!allowSprinting)
            {
                _input.sprintInput.useInput = false;
            }

            if (!allowRunning)
            {
                _controller.alwaysWalkByDefault = true;
            }
        }

        /// <summary>
        /// Allow any movement speed
        /// </summary>
        public void WalkOnlyDisabled()
        {
            _input.sprintInput.useInput = true;
            _controller.alwaysWalkByDefault = false;
        }
    }
}
#endif