using System.Collections.Generic;
using DaftAppleGames.Darskerry.Core.CharController.AiController.FootSteps;
using DaftAppleGames.Darskerry.Core.Extensions;
using UnityEngine;

namespace DaftAppleGames.Darskerry.Core.Spawning
{
    public class SpawnPool : PrefabPool
    {
        #region Class Variables

        private List<GameObject> _activePrefabs;
        #endregion
        #region Startup

        protected override void Awake()
        {
            base.Awake();
            _activePrefabs = new List<GameObject>();
        }
        #endregion
        #region Class Methods

        internal GameObject PreSpawn(bool spawnState)
        {
            GameObject spawnedPrefab = SpawnInstance(Vector3.zero, Quaternion.identity);
            spawnedPrefab.SetActive(spawnState);
            if (spawnedPrefab.TryGetComponent <ISpawnable>(out ISpawnable spawnable))
            {
                spawnable.PreSpawn();
            }
            _activePrefabs.Add(spawnedPrefab);
            return spawnedPrefab;
        }

        internal GameObject Spawn(Vector3 center, float minDistance, float maxDistance, bool spawnState)
        {
            // Get random position on terrain
            Vector3 spawnPosition = Terrain.activeTerrain.GetRandomLocation(center, minDistance, maxDistance);
            GameObject spawnedPrefab = SpawnInstance(spawnPosition, QuaternionExtensions.RandomRotationX());
            if (spawnedPrefab.TryGetComponent <ISpawnable>(out ISpawnable spawnable))
            {
                spawnable.Spawn();
            }
            Terrain.activeTerrain.AlignObject(spawnedPrefab, true, true, false, false, true);
            spawnedPrefab.SetActive(spawnState);
            _activePrefabs.Add(spawnedPrefab);
            return spawnedPrefab;
        }

        internal void Despawn(GameObject spawnedPrefab)
        {
            if (spawnedPrefab.TryGetComponent <ISpawnable>(out ISpawnable spawnable))
            {
                spawnable.PreSpawn();
            }

            DespawnInstance(spawnedPrefab);
            _activePrefabs.Remove(spawnedPrefab);
        }

        internal void PreSpawnAll(bool spawnState)
        {
            for (int currSpawnIndex = 0; currSpawnIndex < poolInitialSize; currSpawnIndex++)
            {
                GameObject spawnedPrefab = PreSpawn(spawnState);
            }
            DespawnAll();
        }

        internal void SpawnAll(Vector3 center, float minDistance, float maxDistance, bool spawnState)
        {
            for (int currSpawnIndex = 0; currSpawnIndex < poolInitialSize; currSpawnIndex++)
            {
                Spawn(center, minDistance, maxDistance, spawnState);
            }
        }

        internal void DespawnAll()
        {
            foreach (GameObject spawnedObject in _activePrefabs.ToArray())
            {
                Despawn(spawnedObject);
            }
        }

        #endregion
    }
}