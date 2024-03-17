using System.Collections;
using DaftAppleGames.Common.Characters;
using DaftAppleGames.Common.GameControllers;
using Sirenix.OdinInspector;
#if INVECTOR_SHOOTER
using Invector.vCharacterController;
#endif
#if PIXELCRUSHERS
using PixelCrushers.Wrappers;
#endif

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace DaftAppleGames.Common.Spawning
{
    public enum PlayerRespawnLocation { Start, LastSpawn, Death }
    public class PlayerSpawner : MonoBehaviour
    {
        [BoxGroup("Player Settings")] public bool spawnOnStart = true;

        [BoxGroup("Player Settings")] public GameObject femaleCharacterPrefab;
        [BoxGroup("Player Settings")] public GameObject maleCharacterPrefab;
        [BoxGroup("Player Settings")] public Transform playerSpawnTarget;

        [BoxGroup("Respawn Settings")] public PlayerRespawnLocation respawnLocation;
        [BoxGroup("Respawn Settings")] public float respawnDelay = 5.0f;
        
        [BoxGroup("Debug")] public GameObject playerGameObject;
        
        [FoldoutGroup("Events")] public UnityEvent<GameObject> onSpawnEvent;

        [BoxGroup("Delayed Event Settings")] public float eventDelay = 5.0f;
        [FoldoutGroup("Delayed Events")] public UnityEvent<GameObject> onSpawnDelayEvent;

        private Transform _lastSpawnTarget;
        private CharSelection _selectedCharacter;

        private bool _isLoadingFromSave = false;

        private bool _hasSpawned;
        
        // Singleton static instance
        private static PlayerSpawner _instance;
        public static PlayerSpawner Instance => _instance;

        /// <summary>
        /// Listen for the SaveData applied event.
        /// </summary>
        private void OnEnable()
        {
#if PIXELCRUSHERS
            SaveSystem.saveDataApplied -= SpawnPlayerFromLoad;
            SaveSystem.saveDataApplied += SpawnPlayerFromLoad;
#endif
        }

        /// <summary>
        /// Remove the SaveData event
        /// </summary>
        private void OnDisable()
        {
#if PIXELCRUSHERS
            SaveSystem.saveDataApplied -= SpawnPlayerFromLoad;
#endif
        }
        
        /// <summary>
        /// Set component properties on awake. Spawn player, if selected
        /// </summary>
        private void Awake()
        {
            // Set up singleton
            if (_instance != null && _instance != this)
            {
                Destroy(this.gameObject);
            }
            else
            {
                _instance = this;
            }
            
            _hasSpawned = false;
        }

        /// <summary>
        /// Spawn player on start, if selected
        /// </summary>
        private void Start()
        {
            _selectedCharacter = GameController.Instance.SelectedCharacter;
            _isLoadingFromSave = GameController.Instance.IsLoadingFromSave;

            if (spawnOnStart && !_isLoadingFromSave)
            {
                Debug.Log($"Player Spawner Start: Spawning...");
                SpawnPlayer(playerSpawnTarget);
            }
        }

        /// <summary>
        /// Wait until the save game data has been applied to the transforms, before spawning
        /// </summary>
        private void SpawnPlayerFromLoad()
        {
            if (!_hasSpawned)
            {
                Debug.Log($"Player Spawner SaveGame Load Detected... Spawning...");
                SpawnPlayer(playerSpawnTarget);
            }
        }

        /// <summary>
        /// Simple public method, usable from events
        /// </summary>
        public void SpawnPlayer()
        {
            SpawnPlayer(playerSpawnTarget);
        }

        /// <summary>
        /// Spawn the selected player game object in the the given transform location
        /// </summary>
        private void SpawnPlayer(Transform spawnTransform)
        {
            Debug.Log($"Spawning Player at: {(spawnTransform.position)} in Scene {SceneManager.GetActiveScene().name}");
            
            _hasSpawned = true;

            if(_selectedCharacter == CharSelection.Emily)
            {
                playerGameObject = Instantiate(femaleCharacterPrefab, spawnTransform.position, spawnTransform.rotation);
            }
            else
            {
                playerGameObject = Instantiate(maleCharacterPrefab, spawnTransform.position, spawnTransform.rotation);
            }

            playerGameObject.SetActive(true);
            
            // Update the GameController with the new player instance
            PlayerCameraManager.Instance.PlayerGameObject = playerGameObject;

            // Attach the new player death event back to the spawner
#if INVECTOR_SHOOTER
            vThirdPersonController controller = playerGameObject.GetComponent<vThirdPersonController>();
            controller.onDead.AddListener(Respawn);
#endif            
            // Start the delayed event spawn async coroutine
            StartCoroutine(InvokeEventsAfterDelay(eventDelay, playerGameObject));
            
            // Record this as the last spawn target
            _lastSpawnTarget = spawnTransform;
            
            // Tell the player what spawned it
            PlayerCharacter player = playerGameObject.GetComponent<PlayerCharacter>();
            player.spawner = this;

            // Apply any savers on the spawned player
            ApplySaveData();
            
            // Call the spawn event
            if(onSpawnEvent != null)
            {
                onSpawnEvent.Invoke(playerGameObject);
            }
        }

        /// <summary>
        /// Handle re-spawn of the player when dead
        /// </summary>
        /// <param name="playerDeadGameObject"></param>
        public void Respawn(GameObject playerDeadGameObject)
        {
            // Clean up the cameras, ready to respawn
            PlayerCameraManager.Instance.MainCamera.transform.SetParent(null);
#if INVECTOR_SHOOTER
            PlayerCameraManager.Instance.InvectorMainCamera.gameObject.SetActive(false);
#endif
            switch (respawnLocation)
            {
                case PlayerRespawnLocation.Death:
                    StartCoroutine(SpawnAfterDelay(playerDeadGameObject.transform, playerDeadGameObject));
                    break;
                case PlayerRespawnLocation.Start:
                    StartCoroutine( SpawnAfterDelay(playerSpawnTarget, playerDeadGameObject));
                    break;
                
                case PlayerRespawnLocation.LastSpawn:
                    StartCoroutine(SpawnAfterDelay(_lastSpawnTarget, playerDeadGameObject));
                    break;
            }
        }

        /// <summary>
        /// Async method to spawn after a given delay
        /// </summary>
        /// <param name="spawnTransform"></param>
        /// <returns></returns>
        public IEnumerator SpawnAfterDelay(Transform spawnTransform, GameObject deadPlayer)
        {
            yield return new WaitForSeconds(respawnDelay);
            SpawnPlayer(spawnTransform);
            Destroy(deadPlayer);
        }
        
        /// <summary>
        /// Fires events after the specified delay in seconds
        /// </summary>
        /// <param name="delay"></param>
        /// <param name="playerGameObject"></param>
        /// <returns></returns>
        private IEnumerator InvokeEventsAfterDelay(float delay, GameObject playerGameObject)
        {
            yield return new WaitForSeconds(delay);
            if (onSpawnDelayEvent != null)
            {
                onSpawnDelayEvent.Invoke(playerGameObject);
            }
        }
        
        /// <summary>
        /// Apply Savers on the spawned player
        /// </summary>
        private void ApplySaveData()
        {
#if PIXELCRUSHERS
            SaveSystem.RecursivelyApplySavers(playerGameObject.transform);
#endif
        }
    }
}