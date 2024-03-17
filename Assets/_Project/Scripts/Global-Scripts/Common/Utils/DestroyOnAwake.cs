using UnityEngine;

namespace DaftAppleGames.Common.Utils
{

    public class DestroyOnAwake : MonoBehaviour
    {
        /// <summary>
        /// Destroy parent object on Awake
        /// </summary>
        public void Awake()
        {
            Destroy(this.gameObject);
        }
    }
}