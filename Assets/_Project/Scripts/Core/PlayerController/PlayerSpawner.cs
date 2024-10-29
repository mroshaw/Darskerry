using Sirenix.OdinInspector;
using Unity.Cinemachine;
using UnityEngine;

namespace DaftAppleGames.Darskerry.Core.PlayerController
{
    public class PlayerSpawner : CharacterSpawner
    {
        #region Class Variables
        [PropertyOrder(2)][BoxGroup("Behaviour")][SerializeField] protected bool disableExistingMainCamera;
        [PropertyOrder(3)][BoxGroup("Player")][SerializeField] private string cameraInstanceName = "Player Main Camera";
        [PropertyOrder(3)][BoxGroup("Player")][SerializeField] private string cinemachineRigInstanceName = "Player CM Camera Rig";
        [PropertyOrder(3)][BoxGroup("Player")][SerializeField] private string inputManagerInstanceName = "Player Input Manager";

        [PropertyOrder(99)][FoldoutGroup("DEBUG")][SerializeField] private GameObject cameraGameObjectInstance;
        [PropertyOrder(99)][FoldoutGroup("DEBUG")][SerializeField] private GameObject inputManagerGameObjectInstance;
        [PropertyOrder(99)][FoldoutGroup("DEBUG")][SerializeField] private GameObject cmRigGameObjectInstance;

        private PlayerSpawnerSettings _playerSpawnerSettings;

        private Camera _camera;
        private PlayerCharacterInputManager _inputManager;
        private CinemachineCamera _cinemachine;
        private PlayerCamera _playerCamera;

        #endregion

        #region Startup
        protected override void Awake()
        {
            base.Awake();
            _playerSpawnerSettings = spawnSettings as PlayerSpawnerSettings;
        }
        #endregion

        #region Class Methods

        protected override bool SpawnInstances()
        {
            base.SpawnInstances();
            if (disableExistingMainCamera)
            {
                DisableMainCamera();
            }

            _playerCamera = character.GetComponent<PlayerCamera>();
            if (!_playerCamera)
            {
                Debug.LogError($"PlayerSpawner: Failed to find PlayerCCameracomponent on PrefabInstance {_playerSpawnerSettings.characterPrefab.name}");
                IsValidSpawn = false;
            }

            _inputManager = FindAnyObjectByType<PlayerCharacterInputManager>();
            if (_inputManager)
            {
                Debug.Log("PlayerInputManager already exists in scene.");
            }
            else
            {
                inputManagerGameObjectInstance = SpawnPrefab(_playerSpawnerSettings.inputManagerPrefab, Vector3.zero, Quaternion.identity, null, inputManagerInstanceName, false);
                _inputManager = inputManagerGameObjectInstance.GetComponent<PlayerCharacterInputManager>();
                if (!_inputManager)
                {
                    Debug.LogError($"PlayerSpawner: Failed to find PlayerCharacterInputManager component on PrefabInstance {_playerSpawnerSettings.inputManagerPrefab.name}");
                    IsValidSpawn = false;
                }
            }

            cameraGameObjectInstance = SpawnPrefab(_playerSpawnerSettings.cameraPrefab, Vector3.zero, Quaternion.identity, null, cameraInstanceName, false);
            _camera = cameraGameObjectInstance.GetComponent<Camera>();
            if (!_camera)
            {
                Debug.LogError($"PlayerSpawner: Failed to find Camera component on PrefabInstance {_playerSpawnerSettings.cameraPrefab.name}");
                IsValidSpawn = false;
            }

            cmRigGameObjectInstance = SpawnPrefab(_playerSpawnerSettings.cmCameraRigPrefab, Vector3.zero, Quaternion.identity, null, cinemachineRigInstanceName, false);
            _cinemachine = cmRigGameObjectInstance.GetComponent<CinemachineCamera>();
            if (!_cinemachine)
            {
                Debug.LogError($"PlayerSpawner: Failed to find CinemachineCamera component on PrefabInstance {_playerSpawnerSettings.cmCameraRigPrefab.name}");
                IsValidSpawn = false;
            }
            return IsValidSpawn;
        }

        private void DisableMainCamera()
        {
            Camera mainCamera = Camera.main;
            if (mainCamera)
            {
                mainCamera.gameObject.SetActive(false);
            }
            /*
            Camera[] allCameras = FindObjectsByType<Camera>(FindObjectsSortMode.None);
            foreach (Camera currCamera in allCameras)
            {
                if (currCamera.CompareTag("MainCamera"))
                {
                    currCamera.gameObject.SetActive(false);
                    return;
                }
            }
            */
        }

        protected override void Configure()
        {
            base.Configure();
            PlayerCharacter playerCharacter = character as PlayerCharacter;

            playerCharacter.camera = _camera;
            _cinemachine.Target.TrackingTarget = _playerCamera.followTarget;
            _playerCamera.virtualCamera = _cinemachine;
        }

        protected override void SetSpawnsActive()
        {
            SetSpawnActive(inputManagerGameObjectInstance);
            base.SetSpawnsActive();
            SetSpawnActive(cameraGameObjectInstance);
            SetSpawnActive(cmRigGameObjectInstance);


        }
        #endregion
    }
}