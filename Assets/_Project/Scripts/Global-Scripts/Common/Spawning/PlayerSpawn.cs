#if ASMDEF
#if INVECTOR_SHOOTER
using Invector.vCamera;
#endif
#if WORLDSTREAMER
using WorldStreamer2;
#endif
#endif


namespace DaftAppleGames.Common.Spawning
{
    public class PlayerSpawn : SpawnObject, ISpawnObject
    {
#if ASMDEF
#if INVECTOR_SHOOTER
#if WORLDSTREAMER
        [Header("Player Spawn Settings")]
        public GameObject femalePlayerPrefab;
        public GameObject malePlayerPrefab;
        public Streamer worldStream;

        private GameObject _cameraGameObject;

        /// <summary>
        /// Configure the component
        /// </summary>
        public void Start()
        {
            // Determine which prefab to spawn
            if(GameController.Instance.SelectedCharacter == CharSelection.Emily)
            {
                spawnPrefab = femalePlayerPrefab;
            }
            else
            {
                spawnPrefab = malePlayerPrefab;
            }

            // Find the Main Camera
            _cameraGameObject = GameUtils.FindMainCameraGameObject();

            if(spawnAtStart)
            {
                base.Spawn();
                // Perform post instantiation config
                PostSpawnConfigure();
            }
        }

        /// <summary>
        /// Make post instantiated config changes
        /// </summary>
        public override void PostSpawnConfigure()
        {
            // Make sure Camera is all set up
            ConfigureCamera();

            // Make sure all references to the player are updated
            ConfigurePlayer();
        }

        /// <summary>
        /// Ensure the Camera is aligned to the spawned player
        /// </summary>
        private void ConfigureCamera()
        {
            // Find the vCamera and re-parent
            vThirdPersonCamera vCamera = spawnedObject.GetComponent<vThirdPersonCamera>();
            if(!vCamera)
            {
                Debug.LogError("PlayerSpawn: Couldn't find vThirdPersonCamera!");
                return;
            }

            // Reparent to camera

        }

        /// <summary>
        /// Set any component references to the "Player" gameobject
        /// </summary>
        private void ConfigurePlayer()
        {
            worldStream.player = spawnedObject.transform;
        }
#endif
#endif
#endif
    }
}