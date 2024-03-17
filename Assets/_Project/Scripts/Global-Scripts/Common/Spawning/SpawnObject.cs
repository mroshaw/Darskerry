using UnityEngine;
using UnityEngine.Events;

namespace DaftAppleGames.Common.Spawning
{
    public class SpawnObject : MonoBehaviour, ISpawnObject
    {
        [Header("Spawn Settings")]
        public bool spawnAtStart;
        public bool activateOnSpawn = true;
        public bool deactivateOnSpawn = false;
        public Transform spawnTransformOverride;
        public GameObject spawnPrefab;

        [SerializeField]
        public GameObject spawnedObject;

        public UnityEvent onSpawnStart;
        public UnityEvent onSpawnEnd;
        public UnityEvent onRespawn;

        /// <summary>
        /// Spawn the prefab at the location
        /// </summary>
        public virtual void Spawn()
        {
            // Invoke start events
            onSpawnStart.Invoke();

            Transform spawnTransform = transform;

            if(spawnTransformOverride)
            {
                spawnTransform = spawnTransformOverride;
            }

            // Instantiate an instance of the prefabs
            spawnedObject = Instantiate(spawnPrefab, spawnTransform);
            spawnedObject.transform.SetParent(null);
            if(activateOnSpawn)
            {
                spawnedObject.SetActive(true);
            }

            if(deactivateOnSpawn)
            {
                spawnedObject.SetActive(false);
            }

            // Invoke end events
            onSpawnEnd.Invoke();
        }

        public virtual void Despawn()
        {
            throw new System.NotImplementedException();
        }

        public virtual void ReSpawn()
        {
            throw new System.NotImplementedException();
        }

        public virtual void PreSpawnConfigure()
        {
            throw new System.NotImplementedException();
        }

        public virtual void PostSpawnConfigure()
        {
            throw new System.NotImplementedException();
        }
    }
}