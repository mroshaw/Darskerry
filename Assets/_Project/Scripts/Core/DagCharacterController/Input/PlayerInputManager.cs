using UnityEngine;

namespace DaftAppleGames.Darskerry.Core.DagCharacterController.Input
{
    [DefaultExecutionOrder(-3)]
    public class PlayerInputManager : MonoBehaviour
    {
        public static PlayerInputManager Instance;
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
            PlayerControls.PlayerLocomotionMap.Disable();
            PlayerControls.ThirdPersonMap.Disable();
            PlayerControls.PlayerActionsMap.Disable();
        }

        public void UnpauseInput()
        {
            Debug.Log("Unpausing input...");
            InitPlayerControls();
            PlayerControls.PlayerLocomotionMap.Enable();
            PlayerControls.ThirdPersonMap.Enable();
            PlayerControls.PlayerActionsMap.Enable();
        }
    }
}