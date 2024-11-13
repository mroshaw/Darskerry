using UnityEngine;

namespace DaftAppleGames.Darskerry.Core.CharController.AiController
{
    [DefaultExecutionOrder(-3)]
    public class PlayerCharacterInputManager : Singleton<PlayerCharacterInputManager>
    {
        public PlayerControls PlayerControls { get; private set; }

        protected override void Awake()
        {
            base.Awake();
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
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