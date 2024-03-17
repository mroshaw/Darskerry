#if HAP
using System;
using DaftAppleGames.Common.Spawning;
using MalbersAnimations.HAP;
using PixelCrushers;
using UnityEngine;

namespace DaftAppleGames.Common.LoadSaveGame 
{
    public class HorseSpawnerSaver : Saver
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
            _spawnTarget = GetComponent<HorseSpawner>().horseSpawnTarget;
            base.Start();
        }
        
        /// <summary>
        /// Serialize and save
        /// </summary>
        /// <returns></returns>
        public override string RecordData()
        {
            Debug.Log($"Spawner: current position of {gameObject.name} is ({_spawnTarget.position})");
            // First, move the Spawn Target to the horse location
            MoveSpawnTargetToHorse();
            
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
            Debug.Log("Spawner: Applying game save data...{gameObject.name}");
        
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
        /// Moves the horse spawn transform to current location
        /// </summary>
        private void MoveSpawnTargetToHorse()
        {
            // Get curent horse location
            HorseSpawner spawner = GetComponent<HorseSpawner>();
            GameObject horseGameObject = spawner.horseGameObject;
            Transform currentHorseTransform = spawner.horseGameObject.transform;
            
            // Move spawner to position
            _spawnTarget.position = currentHorseTransform.position;
            _spawnTarget.rotation = currentHorseTransform.rotation;
            
            // If player is riding, spawn the horse to the side, to prevent spawning in same location
            MRider rider = horseGameObject.GetComponentInChildren<MRider>();
            if (rider && rider.IsRiding)
            {
                // Find a spot 2 units to the left
                Vector3 targetPosition = currentHorseTransform.position + (Vector3.left * 2);
                
                // Sample the terrain, so we can spawn at the right height
                float terrainHeight =
                    Terrain.activeTerrain.SampleHeight(targetPosition);
                
                // Set the spawn target
                _spawnTarget.position = new Vector3(targetPosition.x, terrainHeight, targetPosition.z);
                Debug.Log($"Spawner: Moved Horse {horseGameObject.name} to ({_spawnTarget.position}");
            }
        }
    }
}
#endif