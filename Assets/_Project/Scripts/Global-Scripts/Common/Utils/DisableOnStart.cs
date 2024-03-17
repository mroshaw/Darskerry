using UnityEngine;

namespace DaftAppleGames.Common.Utils
{

    public class DisableOnStart : MonoBehaviour
    {
        /// <summary>
        /// De-activate on start
        /// </summary>
        public void Start()
        {
            this.gameObject.SetActive(false);
        }
    }
}