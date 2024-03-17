using MalbersAnimations.HAP;
using UnityEngine;
using Sirenix.OdinInspector;

namespace DaftAppleGames.Common.Characters
{
    public class RiderHelper : MonoBehaviour
    {
        private MRider _rider;

        /// <summary>
        /// Init the rider component
        /// </summary>
        private void Start()
        {
            _rider = GetComponent<MRider>();
        }

        /// <summary>
        /// Sets the call state without whistling
        /// </summary>
        /// <param name="state"></param>
        public void SetCallState(bool state)
        {
            _rider.ToggleCall = false;
        }
    }
}
