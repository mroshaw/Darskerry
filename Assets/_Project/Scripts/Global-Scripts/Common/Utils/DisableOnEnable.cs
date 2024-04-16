using Sirenix.OdinInspector;
using UnityEngine;

namespace DaftAppleGames.Common.Utils
{

    public class DisableOnEnable : MonoBehaviour
    {
        [BoxGroup("Settings")] public bool includeThisGameObject = true;
        [BoxGroup("Settings")] public GameObject[] gameObjects;

        /// <summary>
        /// Disable on Awake
        /// </summary>
        public void OnEnable()
        {
            if (includeThisGameObject)
            {
                this.gameObject.SetActive(false);
            }
        }
    }
}