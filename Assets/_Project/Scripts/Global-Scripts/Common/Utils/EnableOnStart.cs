using UnityEngine;

namespace DaftAppleGames.Common.Utils
{

    public class EnableOnStart : MonoBehaviour
    {
        /// <summary>
        /// Enable on Start
        /// </summary>
        public void Start()
        {
            gameObject.SetActive(true);
        }
    }
}