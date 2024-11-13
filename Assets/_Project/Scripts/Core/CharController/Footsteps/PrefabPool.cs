using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

namespace DaftAppleGames.Darskerry.Core.CharController.AiController.FootSteps
{
    public enum PrefabPoolType { FootstepMarks, FootstepParticles };

    public class PrefabPool : MonoBehaviour
    {
        #region Class Variables
        [BoxGroup("Prefab Settings")][SerializeField] private PrefabPoolType prefabPoolType;
        [BoxGroup("Prefab Settings")][SerializeField] private GameObject poolPrefab;
        [BoxGroup("Prefab Settings")][SerializeField] private Transform instanceContainer;
        [BoxGroup("Prefab Settings")][SerializeField] private float lifeTimeInSeconds;
        [BoxGroup("Pool Settings")][SerializeField] private int poolMaxSize = 5;
        [BoxGroup("Pool Settings")][SerializeField] private int poolInitialSize = 10;
        /*
        [BoxGroup("DEBUG")][SerializeField] private int poolActiveCountDebug;
        [BoxGroup("DEBUG")][SerializeField] private int poolInactiveCountDebug;
        */
        public PrefabPoolType PrefabPoolType => prefabPoolType;

        private ObjectPool<GameObject> _prefabInstancePool;
        #endregion

        #region Startup
        /// <summary>
        /// Configure the component on awake
        /// </summary>   
        private void Start()
        {
            _prefabInstancePool = new ObjectPool<GameObject>(CreatePrefabInstancePoolItem, PrefabInstanceOnTakeFromPool,
                PrefabInstanceOnReturnToPool, PrefabInstanceOnDestroyPoolObject, false,
                poolInitialSize, poolMaxSize);
        }
        #endregion

        #region Update
        /*
        private void Update()
        {
            poolActiveCountDebug = _prefabInstancePool.CountActive;
            poolInactiveCountDebug = _prefabInstancePool.CountInactive;
        }

        */
        #endregion
        #region Class methods
        public GameObject SpawnInstance(Vector3 spawnPosition, Quaternion spawnRotation)
        {
            GameObject prefabInstance = _prefabInstancePool.Get();
            prefabInstance.transform.position = spawnPosition;
            prefabInstance.transform.rotation = spawnRotation;
            StartCoroutine(ReturnToPoolAfterDelay(prefabInstance));
            return prefabInstance;
        }

        private IEnumerator ReturnToPoolAfterDelay(GameObject prefabInstance)
        {
            yield return new WaitForSeconds(lifeTimeInSeconds);
            _prefabInstancePool.Release(prefabInstance);
        }

        private GameObject CreatePrefabInstancePoolItem()
        {
            GameObject prefabInstance = Instantiate(poolPrefab, instanceContainer, false);
            prefabInstance.name = $"{poolPrefab.name}-New";
            return prefabInstance;
        }

        private void PrefabInstanceOnTakeFromPool(GameObject prefabInstance)
        {
            prefabInstance.name = $"{poolPrefab.name}-Taken";
            prefabInstance.SetActive(true);
        }

        private void PrefabInstanceOnReturnToPool(GameObject prefabInstance)
        {
            prefabInstance.name = $"{poolPrefab.name}-Returned";
            prefabInstance.transform.position = Vector3.zero;
            prefabInstance.transform.rotation = Quaternion.identity;
            prefabInstance.SetActive(false);
        }

        private void PrefabInstanceOnDestroyPoolObject(GameObject prefabInstance)
        {
            Destroy(prefabInstance);
        }
        #endregion
    }
}