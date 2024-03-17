using DaftAppleGames.Common.Settings;
using DaftAppleGames.Common.Ui;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace DaftAppleGames.Common.PauseGame
{
    public class PauseGameUiWindow : BaseUiWindow
    {
        [Header("Pause Key Settings")]
        public KeyCode pauseKey = KeyCode.Escape;
        public KeyCode altPauseKey = KeyCode.P;

        [Header("UI Settings")]
        public Button continueButton;
        public Button optionsButton;
        public Button loadButton;
        public Button saveButton;
        public Button mainMenuButton;
        public Button exitDesktopButton;
        public Button debugButton;
        
        [Header("Proxy UI Events")]
        public UnityEvent onContinueButtonClickEvent;
        public UnityEvent onOptionsButtonClickEvent;
        public UnityEvent onLoadButtonClickEvent;
        public UnityEvent onSaveButtonClickEvent;
        public UnityEvent onMainMenuButtonClickEvent;
        public UnityEvent onExitToDesktopButtonClickEvent;
        public UnityEvent onBackButtonClickEvent;
        public UnityEvent onDebugButtonClickEvent;
        
        private PauseGameManager _pauseGameManager;
        public GameSettingsUiWindow gameSettingsUiWindow;

        /// <summary>
        /// Initialise the manager
        /// </summary>
        public override void Start()
        {
            _pauseGameManager = GetComponent<PauseGameManager>();

            InitControls();
            base.Start();
        }

        /// <summary>
        /// Continue button proxy event handler
        /// </summary>
        public void ContinueButtonProxy()
        {
            onContinueButtonClickEvent.Invoke();
        }

        /// <summary>
        /// Options button proxy event handler
        /// </summary>
        public void OptionsButtonProxy()
        {
            onOptionsButtonClickEvent.Invoke();
        }

        /// <summary>
        /// Load button proxy event handler
        /// </summary>
        public void LoadButtonProxy()
        {
            onLoadButtonClickEvent.Invoke();
        }

        /// <summary>
        /// Save button proxy event handler
        /// </summary>
        public void SaveButtonProxy()
        {
            onSaveButtonClickEvent.Invoke();
        }

        /// <summary>
        /// Main Menu button proxy event handler
        /// </summary>
        public void MainMenuButtonProxy()
        {
            onMainMenuButtonClickEvent.Invoke();
        }
        
        /// <summary>
        /// Exit button proxy event handler
        /// </summary>
        public void ExitButtonProxy()
        {
            onExitToDesktopButtonClickEvent.Invoke();
        }

        /// <summary>
        /// Debug button UI proxy
        /// </summary>
        public void DebugButtonProxy()
        {
            onDebugButtonClickEvent.Invoke();
        }

        /// <summary>
        /// Toggles the Debug button on and off
        /// </summary>
        public void SetToggleButtonState(bool state)
        {
            debugButton.gameObject.SetActive(state);
        }
        
        /// <summary>
        /// Set up button control event listeners
        /// </summary>
        private void InitControls()
        {

        }
        
        /// <summary>
        /// Wait for the pause key
        /// </summary>
        private void Update()
        {
            if(Input.GetKeyDown(pauseKey) || Input.GetKeyDown(altPauseKey))
            {
                // Debug.Log("Pause Key Pressed!");
                bool isPaused = _pauseGameManager.isPaused;
                // Debug.Log($"Paused State: {isPaused}");
                // Debug.Log($"UiWindowsOpen: {UiController.Instance.GetOpenUiName()}");
                
                // Only pause if not paused and no other UI Window is open
                if(!isPaused && !UiController.Instance.AnyUiOpen)
                {
                    _pauseGameManager.PauseGame();
                    ShowUi();
                    return;
                }

                // Only unpause if not in settings UI
                if (isPaused && !gameSettingsUiWindow.isUiOpen)
                {
                    _pauseGameManager.UnPauseGame();
                    HideUi();
                }
            }
        }
    }
}