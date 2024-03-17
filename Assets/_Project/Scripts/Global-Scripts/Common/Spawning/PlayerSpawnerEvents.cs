using System.Collections;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.Events;

namespace DaftAppleGames.Common.Spawning
{
    /// <summary>
    /// Helper class to allow gameobjects to listen for Player Spawner events
    /// </summary>
    public class PlayerSpawnerEvents : MonoBehaviour
    {
       // Public serializable properties
        [FoldoutGroup("Player Spawned Events")]
        public UnityEvent<GameObject> onPlayerSpawnedEvent;

        #region UNITY_EVENTS
        private void Start()
        {
            StartCoroutine(HookPlayerSpawnerAsync());
        }

        private void OnDestroy()
        {
            PlayerSpawner.Instance.onSpawnEvent.RemoveListener(CallPlayerSpawned);
        }
        #endregion
        /// <summary>
        /// Invoke events
        /// </summary>
        private void CallPlayerSpawned(GameObject playerGameObject)
        {
            // Debug.Log("PlayerSpawnerEvents: Invoked!");
            onPlayerSpawnedEvent.Invoke(playerGameObject);
        }

        /// <summary>
        /// Waits for the PlayerSpawner to become available, then subscribes
        /// </summary>
        /// <returns></returns>
        private IEnumerator HookPlayerSpawnerAsync()
        {
            while (!PlayerSpawner.Instance)
            {
                yield return null;
            }
            PlayerSpawner.Instance.onSpawnEvent.AddListener(CallPlayerSpawned);
        }
    }
}
