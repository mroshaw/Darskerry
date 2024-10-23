using UnityEngine;

namespace DaftAppleGames.Darskerry.Core.PlayerController
{
    [DefaultExecutionOrder(-3)]
    public class PlayerCharacterInputManager : MonoBehaviour
    {
        public static PlayerCharacterInputManager Instance;
        public PlayerControls PlayerControls {  get; private set; }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        private void OnEnable()
        {
            InitPlayerControls();
        }

        private void InitPlayerControls()
        {
            if (PlayerControls == null)
            {
                PlayerControls = new PlayerControls();
                PlayerControls.Enable();
            }

        }

        private void OnDisable()
        {
            PlayerControls.Disable();
        }

        public void PauseInput()
        {
            Debug.Log("Pausing input...");
            InitPlayerControls();
            PlayerControls.CharacterControls.Disable();
            PlayerControls.CameraControls.Disable();
        }

        public void UnpauseInput()
        {
            Debug.Log("Unpausing input...");
            InitPlayerControls();
            PlayerControls.CharacterControls.Enable();
            PlayerControls.CameraControls.Enable();
        }
    }
}