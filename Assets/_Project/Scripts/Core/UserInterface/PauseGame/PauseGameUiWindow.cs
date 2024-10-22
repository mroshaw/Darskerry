using DaftAppleGames.Darskerry.Core.UserInterface;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace DaftAppleGames.Darskerry.Core.UserInterface.PauseGame
{
    public class PauseGameUiWindow : BaseUiWindow
    {
        [BoxGroup("UI Settings")] [SerializeField] private Button continueButton;
        [BoxGroup("UI Settings")] [SerializeField] private Button optionsButton;
        [BoxGroup("UI Settings")] [SerializeField] private Button loadButton;
        [BoxGroup("UI Settings")] [SerializeField] private Button saveButton;
        [BoxGroup("UI Settings")] [SerializeField] private Button mainMenuButton;
        [BoxGroup("UI Settings")] [SerializeField] private Button exitDesktopButton;
        [BoxGroup("UI Settings")] [SerializeField] private Button debugButton;
        
        [BoxGroup("Proxy UI Events")] [SerializeField] private UnityEvent onContinueButtonClickEvent;
        [BoxGroup("Proxy UI Events")] [SerializeField] private UnityEvent onOptionsButtonClickEvent;
        [BoxGroup("Proxy UI Events")] [SerializeField] private UnityEvent onLoadButtonClickEvent;
        [BoxGroup("Proxy UI Events")] [SerializeField] private UnityEvent onSaveButtonClickEvent;
        [BoxGroup("Proxy UI Events")] [SerializeField] private UnityEvent onMainMenuButtonClickEvent;
        [BoxGroup("Proxy UI Events")] [SerializeField] private UnityEvent onExitToDesktopButtonClickEvent;
        [BoxGroup("Proxy UI Events")] [SerializeField] private UnityEvent onBackButtonClickEvent;
        [BoxGroup("Proxy UI Events")] [SerializeField] private UnityEvent onDebugButtonClickEvent;

        public void ContinueButtonProxy()
        {
            onContinueButtonClickEvent.Invoke();
        }

        public void OptionsButtonProxy()
        {
            onOptionsButtonClickEvent.Invoke();
        }

        public void LoadButtonProxy()
        {
            onLoadButtonClickEvent.Invoke();
        }

        public void SaveButtonProxy()
        {
            onSaveButtonClickEvent.Invoke();
        }

        public void MainMenuButtonProxy()
        {
            onMainMenuButtonClickEvent.Invoke();
        }
        
        public void ExitButtonProxy()
        {
            onExitToDesktopButtonClickEvent.Invoke();
        }

        public void DebugButtonProxy()
        {
            onDebugButtonClickEvent.Invoke();
        }

        public void SetToggleButtonState(bool state)
        {
            debugButton.gameObject.SetActive(state);
        }
    }
}