using UnityEngine;
using Sirenix.OdinInspector;

namespace DaftAppleGames.Darskerry.Core.Spawning
{
    [RequireComponent(typeof(Collider))]
    internal class AreaSpawner : Spawner
    {
        #region Class Variables
        [BoxGroup("Behaviour")] [SerializeField] private bool fillPoolsOnStart = true;
        [BoxGroup("Prefabs")] [SerializeField] private SpawnPool[] spawnPools;
        [BoxGroup("Trigger Settings")] [SerializeField] private float triggerRange = 15.0f;
        [BoxGroup("Spawn Settings")] [SerializeField] private float minSpawnDistance = 1.5f;
        [BoxGroup("Spawn Settings")] [SerializeField] private float maxSpawnDistance = 5f;
        [BoxGroup("Respawn Settings")] [SerializeField] private bool respawn = true;
        [BoxGroup("Respawn Settings")] [SerializeField] private float respawnDelay = 60.0f;

#if UNITY_EDITOR
        [FoldoutGroup("Gizmos")][SerializeField] private bool drawGizmos = true;
        [FoldoutGroup("Gizmos")][SerializeField] private Color minRangeGizmoColor = Color.red;
        [FoldoutGroup("Gizmos")][SerializeField] private Color maxRangeGizmoColor = Color.green;
        [FoldoutGroup("Gizmos")][SerializeField] private Color triggerRangeGizmoColor = Color.blue;
#endif

        #endregion

        #region Startup
        /// <summary>
        /// Subscribe to events
        /// </summary>   
        private void OnEnable()
        {
            
        }

        /// <summary>
        /// Configure the component on awake
        /// </summary>   
        private void Awake()
        {
            RefreshPools();
            ResizeTrigger();
        }
    
        /// <summary>
        /// Configure the component on start
        /// </summary>
        private void Start()
        {
            if (fillPoolsOnStart)
            {
                FillPrefabPools();
            }
        }
        #endregion
        #region Class methods

        private void OnTriggerEnter(Collider other)
        {
            // Spawn();
        }

        private void OnTriggerExit(Collider other)
        {
            // Despawn();
        }

        [Button("Spawn Now")]
        private void Spawn()
        {
            foreach (SpawnPool spawnPool in spawnPools)
            {
                spawnPool.SpawnAll(transform.position, minSpawnDistance, maxSpawnDistance, true);
            }
        }

        [Button("Despawn Now")]
        private void Despawn()
        {
            foreach (SpawnPool spawnPool in spawnPools)
            {
                spawnPool.DespawnAll();
            }
        }

        [Button("Resize Trigger")]
        private void ResizeTrigger()
        {
            Collider triggerCollider = GetComponent<Collider>();

            if (!triggerCollider)
            {
                return;
            }

            if (triggerCollider is SphereCollider sphereCollider)
            {
                sphereCollider.radius = triggerRange;
            }
        }

        private void FillPrefabPools()
        {
            foreach (SpawnPool spawnPool in spawnPools)
            {
                spawnPool.PreSpawnAll(false);
            }
        }

        [Button("Refresh Pools")]
        private void RefreshPools()
        {
            spawnPools = GetComponentsInChildren<SpawnPool>();
        }
        #endregion
        #region Editor methods

#if UNITY_EDITOR
        protected void OnDrawGizmosSelected()
        {
            if (drawGizmos)
            {
                // Draw a sphere for the 'min' range
                Gizmos.color = minRangeGizmoColor;
                Gizmos.DrawSphere(transform.position, minSpawnDistance);

                // Draw a sphere for the 'max' range
                Gizmos.color = maxRangeGizmoColor;
                Gizmos.DrawSphere(transform.position, maxSpawnDistance);

                // Draw a sphere for the 'trigger' range
                Gizmos.color = triggerRangeGizmoColor;
                Gizmos.DrawSphere(transform.position, triggerRange);
            }
        }
#endif
        #endregion
    }
}