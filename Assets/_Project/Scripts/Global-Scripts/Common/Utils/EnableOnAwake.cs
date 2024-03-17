using UnityEngine;

namespace DaftAppleGames.Common.Utils
{

    public class EnableOnAwake : MonoBehaviour
    {
        /// <summary>
        /// Enable on Awake
        /// </summary>
        public void Awake()
        {
            this.gameObject.SetActive(true);
        }
    }
}