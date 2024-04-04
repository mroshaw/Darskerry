using Sirenix.OdinInspector;
using UnityEngine;

namespace DaftApplesGames.Common.Spawning
{


    public class ObjectSpawnManager : MonoBehaviour
    {

        [BoxGroup("Settings")] public GameObject ParticleFxContainer;
        [BoxGroup("Settings")] public GameObject SoundFxContainer;
        [BoxGroup("Settings")] public GameObject SoulLinkContainer;

        // Singleton static instance
        private static ObjectSpawnManager _instance;
        public static ObjectSpawnManager Instance { get { return _instance; } }


        /// <summary>
        /// Initialise the Singleton instance
        /// </summary>
        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(this.gameObject);
            }
            else
            {
                _instance = this;
            }
        }
    }
}