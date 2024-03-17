using UnityEngine;
#if ASMDEF
#if WORLDSTREAMER
using WorldStreamer2;
#endif
#endif
namespace DaftAppleGames.Common.Spawning
{
    public class SpawnManager : MonoBehaviour
    {
#if ASMDEF
#if WORLDSTREAMER
        [Header("Spawn Manager Settings")]
        public UILoadingStreamer loadingStreamer;

        public bool waitUntilStreamerLoader = true;
        [SerializeField]
        public SpawnObject[] allSpawnObjects;

        [Header("Events")]
        public UnityEvent onSpawnStart;
        public UnityEvent onSpawnDone;

        private bool _spawned = false;

        /// <summary>
        /// Configure the component
        /// </summary>
        public void Awake()
        {
            if(waitUntilStreamerLoader)
            {
                RegisterLoadEvent();
            }
        }

        /// <summary>
        /// Handle the OnLoadDone event
        /// </summary>
        private void OnLoaderDone()
        {
            if(!_spawned)
            {
                SpawnAll();
            }
        }

        /// <summary>
        /// Register listender on the LoaderGUI
        /// </summary>
        private void RegisterLoadEvent()
        {
            loadingStreamer.onDone.AddListener(OnLoaderDone);
        }

        /// <summary>
        /// Spawn all of the associated spawn objects
        /// </summary>
        private void SpawnAll()
        {
            _spawned = true;
            foreach (SpawnObject spawnObject in allSpawnObjects)
            {
                spawnObject.Spawn();
            }
        }
#endif
#endif
    }
}