using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace DaftAppleGames.Darskerry.Core.PlayerController
{
    public class CharacterSpawnManager : Singleton<CharacterSpawnManager>
    {
        #region Class Variables
        [BoxGroup("Spawners")][SerializeField] private List<CharacterSpawner> spawners;
        [FoldoutGroup("Events")] public UnityEvent playerSpawnedEvent;
        #endregion

        #region Startup
        public void RegisterNewSpawner(CharacterSpawner spawner)
        {
            if (spawners != null && !spawners.Contains(spawner))
            {
                spawners.Add(spawner);
                if (spawner is PlayerSpawner playerSpawner)
                {
                    playerSpawner.CharacterSpawnedEvent.AddListener(PlayerSpawnerSpawnedEventProxy);
                }
            }
        }

        public void DeregisterSpawner(CharacterSpawner spawner)
        {
            spawners.Remove(spawner);
        }

        private void PlayerSpawnerSpawnedEventProxy()
        {
            playerSpawnedEvent.Invoke();
        }

        #endregion
    }
}