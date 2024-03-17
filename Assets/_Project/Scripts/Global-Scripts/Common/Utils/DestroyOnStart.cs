using UnityEngine;

namespace DaftAppleGames.Common.Utils
{

    public class DestroyOnStart : MonoBehaviour
    {
        /// <summary>
        /// Destroy on start
        /// </summary>
        void Start()
        {

            Destroy(this.gameObject);
        }
    }
}