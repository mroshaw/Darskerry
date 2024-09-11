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
            PlayerControls = new PlayerControls();
            PlayerControls.Enable();
        }

        private void OnDisable()
        {
            PlayerControls.Disable();
        }

        public void PauseInput()
        {
            PlayerControls.PlayerLocomotionMap.Disable();
            PlayerControls.PlayerActionsMap.Disable();
        }

        public void UnpauseInput()
        {
            PlayerControls.PlayerLocomotionMap.Enable();
            PlayerControls.PlayerActionsMap.Enable();
        }
    }
}