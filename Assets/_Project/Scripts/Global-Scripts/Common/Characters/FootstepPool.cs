using System.Collections;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.Pool;

namespace DaftAppleGames.Common.Characters
{
    public class FootstepPool : MonoBehaviour
    {
        [BoxGroup("Pool Settings")] public int maxPoolSize;
        [BoxGroup("Pool Settings")] public int initialPoolSize;
        [BoxGroup("Pool Settings")] public GameObject poolPrefab;
        [BoxGroup("Spawn Settings")] public Transform spawnContainer;
        [BoxGroup("Spawn Settings")] public string spawnNamePrefix;
        [BoxGroup("Spawn Settings")] public bool limitSpawnLifetime;
        [BoxGroup("Spawn Settings")] public float spawnLifetime = 5.0f;

        private ObjectPool<GameObject> _objectPool;

        private const string NewPostfix = "-New";
        private const string TakenPostfix = "-Taken";
        private const string ReturnedPostfix = "-Returned";

        private void Awake()
        {
            _objectPool = new ObjectPool<GameObject>(CreatePoolItem, OnTakeFromPool, OnReturnToPool, OnDestroyPoolObject, false, initialPoolSize, maxPoolSize);
        }

        private void Start()
        {

        }

        /// <summary>
        /// Spawn an instance of the prefab from the pool
        /// </summary>
        /// <param name="spawnTransform"></param>
        /// <returns></returns>
        public GameObject SpawnPoolObject(Transform spawnTransform)
        {
            GameObject newGameObject = _objectPool.Get();
            newGameObject.transform.position = spawnTransform.position;

            if (limitSpawnLifetime)
            {
                StartCoroutine(ReleaseAfterDelayAsync(newGameObject));
            }

            return newGameObject;
        }

        /// <summary>
        /// Create an instance of an item in the pool
        /// </summary>
        /// <returns></returns>
        private GameObject CreatePoolItem()
        {
            GameObject newInstance = Instantiate(poolPrefab, spawnContainer, false);
            newInstance.name = $"{spawnNamePrefix}{NewPostfix}";
            return newInstance;
        }

        /// <summary>
        /// Take an item from the pool
        /// </summary>
        /// <param name="poolGameObject"></param>
        private void OnTakeFromPool(GameObject poolGameObject)
        {
            poolGameObject.name = $"{spawnNamePrefix}{TakenPostfix}";
            poolGameObject.SetActive(true);
        }

        /// <summary>
        /// Return item to pool
        /// </summary>
        /// <param name="poolGameObject"></param>
        private void OnReturnToPool(GameObject poolGameObject)
        {
            if (!poolGameObject)
            {
                Debug.Log("AudioSourceOnReturnToPool: AudioSourceGameObject is null!");
                return;
            }

            poolGameObject.name = $"{spawnNamePrefix}{ReturnedPostfix}";

            if (poolGameObject.transform)
            {
                poolGameObject.transform.position = Vector3.zero;
                poolGameObject.gameObject.transform.rotation = Quaternion.identity;
            }
            if (poolGameObject)
            {
                poolGameObject.SetActive(false);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="poolGameObject"></param>
        private void OnDestroyPoolObject(GameObject poolGameObject)
        {
            Destroy(poolGameObject);
        }

        /// <summary>
        /// Release the pool item after the given delay
        /// </summary>
        /// <param name="poolGameObject"></param>
        /// <returns></returns>
        public IEnumerator ReleaseAfterDelayAsync(GameObject poolGameObject)
        {
            yield return new WaitForSeconds(spawnLifetime);
            _objectPool.Release(poolGameObject);
        }
    }
}