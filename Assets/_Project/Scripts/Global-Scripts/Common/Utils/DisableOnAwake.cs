using UnityEngine;

namespace DaftAppleGames.Common.Utils
{

    public class DisableOnAwake : MonoBehaviour
    {
        /// <summary>
        /// Disable on Awake
        /// </summary>
        public void Awake()
        {

            this.gameObject.SetActive(false);
        }
    }
}