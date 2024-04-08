#if INVECTOR_SHOOTER
using Invector.vCharacterController;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DaftAppleGames.Common.Characters 
{
    public class PlayerSpeedLimiterHelper : MonoBehaviour
    {
        [BoxGroup("Speed Settings")] public bool allowRunning = false;
        [BoxGroup("Speed Settings")] public bool allowSprinting = false;

        /// <summary>
        /// Event Handler to enable Walking Only
        /// </summary>
        /// <param name="other"></param>
        public void EnableWalkingOnly(Collider other)
        {
            SetWalkingOnlyState(other, true);
        }

        /// <summary>
        /// Event Handler to disable Walking Only
        /// </summary>
        /// <param name="other"></param>
        public void DisableWalkingOnly(Collider other)
        {
            SetWalkingOnlyState(other, false);
        }

        /// <summary>
        /// Sets the Walking Only state, if the other collider has the Speed Limiter
        /// </summary>
        /// <param name="other"></param>
        /// <param name="state"></param>
        private void SetWalkingOnlyState(Collider other, bool state)
        {
            PlayerSpeedLimiter playerSpeedLimiter = other.GetComponent<PlayerSpeedLimiter>();
            if (playerSpeedLimiter)
            {
                if (state)
                {
                    playerSpeedLimiter.WalkOnlyEnabled(allowRunning, allowSprinting);
                }
                else
                {
                    playerSpeedLimiter.WalkOnlyDisabled();
                }
            }
        }
    }
}
#endif