#if PIXELCRUSHERS
#if HAP
using PixelCrushers;
using System;
using DaftAppleGames.Common.Spawning;
using MalbersAnimations.HAP;
using UnityEngine;

namespace DaftAppleGames.Common.LoadSaveGame 
{
    public class PlayerSpawnerSaver : Saver
    {
        /// <summary>
        /// Class to store serialised Spawner targets
        /// </summary>
        [Serializable]
        public class SpawnerData
        {
            public Vector3 spawnPosition;
            public Quaternion spawnRotation;
        }

        private Transform _spawnTarget;

        public override void Start()
        {
            _spawnTarget = GetComponent<PlayerSpawner>().playerSpawnTarget;
            base.Start();
        }
        
        /// <summary>
        /// Serialize and save
        /// </summary>
        /// <returns></returns>
        public override string RecordData()
        {
            Debug.Log($"Spawner: current position of {gameObject.name} is ({_spawnTarget.position})");
            // First, move the Spawn Target to the player location
            MoveSpawnTargetToPlayer();

            Debug.Log($"Spawner: Saving game data to {gameObject.name} ... ({_spawnTarget.position})");
            
            SpawnerData spawnerData = new SpawnerData
            {
                spawnPosition = _spawnTarget.position,
                spawnRotation = _spawnTarget.rotation
            };
            return SaveSystem.Serialize(spawnerData);
        }

        /// <summary>
        /// Deserialize and load
        /// </summary>
        /// <param name="saveDataString"></param>
        public override void ApplyData(string saveDataString)
        {
            Debug.Log($"Spawner: Applying game save data...{gameObject.name}");
        
            // Deserialize
            if (string.IsNullOrEmpty(saveDataString))
            {
                return; // No data to apply.
            }
            
            SpawnerData spawnerData = SaveSystem.Deserialize<SpawnerData>(saveDataString);
            if (spawnerData == null)
            {
                return;
            } 

            // Update selected character
            _spawnTarget.position = spawnerData.spawnPosition;
            _spawnTarget.transform.rotation = spawnerData.spawnRotation;
            Debug.Log($"Spawner: Applied position game save data to {gameObject.name} ... ({spawnerData.spawnPosition})");
        }

        /// <summary>
        /// Moves the Spawn Target to the player's location
        /// </summary>
        private void MoveSpawnTargetToPlayer()
        {
            // First move the spawn transform to the current location
            PlayerSpawner spawner = GetComponent<PlayerSpawner>();
            GameObject playerGameObject = spawner.playerGameObject;
            Transform currentPlayerTransform = playerGameObject.transform;
            
            // Move the spawn target to players current position
            _spawnTarget.position = currentPlayerTransform.position;
            _spawnTarget.rotation = currentPlayerTransform.rotation;
            
            // If player is riding, we'll spawn on the ground. Horse will be spawned to the left
            MRider rider = playerGameObject.GetComponent<MRider>();
            if (rider && rider.IsRiding)
            {
                // Sample the terrain, so we can spawn at the right height
                float terrainHeight =
                    Terrain.activeTerrain.SampleHeight(currentPlayerTransform.position);
                Vector3 newTarget = new Vector3(currentPlayerTransform.position.x, terrainHeight,
                    currentPlayerTransform.position.z);
                _spawnTarget.position = newTarget;
                Debug.Log($"Spawner: Moved Player {playerGameObject.name} to ({_spawnTarget.position}");
            }
        }
    }
}
#endif
#endif