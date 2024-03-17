#if BIRDS
using UnityEngine;

namespace DaftAppleGames.Common.Characters
{
    public class LivingBirdsHelper : MonoBehaviour
    {
        // Public serializable properties
        // Private fields
        private BirdController[] _allBirdControllers;

        #region UNITY_EVENTS
        /// <summary>
        /// Configure the component on start
        /// </summary>
        private void Start()
        {
            _allBirdControllers = FindObjectsByType<BirdController>(FindObjectsSortMode.None);
        }
        #endregion
        
        #region Public methods

        /// <summary>
        /// Hide all bird controllers
        /// </summary>
        public void HideBirds()
        {
            SetBirdsState(false);
        }

        /// <summary>
        /// Show all bird controllers
        /// </summary>
        public void ShowBirds()
        {
            SetBirdsState(true);
        }

        /// <summary>
        /// Sets the state of all bird controllers
        /// </summary>
        /// <param name="isEnabled"></param>
        private void SetBirdsState(bool isEnabled)
        {
            foreach (BirdController controller in _allBirdControllers)
            {
                if (isEnabled)
                {
                    controller.gameObject.SetActive(true);
                    controller.AllUnPause();
                }
                else
                {
                    controller.AllPause();
                    controller.gameObject.SetActive(false);
                }
            }
        }
        #endregion
    }
}
#endif