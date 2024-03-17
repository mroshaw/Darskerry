#if INVECTOR_SHOOTER
using Invector.vCharacterController;
using UnityEngine;

namespace DaftAppleGames.Common.Characters 
{
    public class PlayerSpeedLimiter : MonoBehaviour
    {
        [Header("Speed Settings")] 
        public bool allowRunning = false;
        public bool allowSprinting = false;
        [Header("Zone Settings")]
        public string zoneTag = "SlowSpeedZone";
        
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
        /// Set speed limits
        /// </summary>
        /// <param name="other"></param>
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(zoneTag))
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
        }

        /// <summary>
        /// Restore speed
        /// </summary>
        /// <param name="other"></param>
        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag(zoneTag))
            {
                _input.sprintInput.useInput = true;
                _controller.alwaysWalkByDefault = false;

            }
        }
    }
}
#endif