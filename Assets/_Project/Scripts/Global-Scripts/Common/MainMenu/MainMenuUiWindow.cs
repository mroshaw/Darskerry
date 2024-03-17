using DaftAppleGames.Common.Settings;
using DaftAppleGames.Common.Ui;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace DaftAppleGames.Common.MainMenu
{
    public class MainMenuUiWindow : BaseUiWindow, IUiWindow
    {
        [BoxGroup("UI Configuration")]
        public TextMeshProUGUI versionText;
        [BoxGroup("UI Configuration")]
        public GameObject mainMenuFirstSelected;

        [BoxGroup("Game Settings Menu")]
        public GameSettingsUiWindow gameSettingsUiWindow;
        [FoldoutGroup("Start Button Events")]
        public UnityEvent onStartGameButtonClickEvent;
        [FoldoutGroup("Load Button Events")]
        public UnityEvent onLoadButtonClickEvent;
        [FoldoutGroup("Settings Button Events")]
        public UnityEvent onSettingsButtonClickEvent;
        [FoldoutGroup("Credits Button Event")]
        public UnityEvent onCreditsButtonClickEvent;
        [FoldoutGroup("Exit Button Events")]
        public UnityEvent onExitToDesktopButtonClickEvent;
        /// <summary>
        /// Init the UI
        /// </summary>
        public override void Start()
        {
            base.Start();
            InitControls();
            SetVersion();
            EventSystem.current.firstSelectedGameObject = mainMenuFirstSelected;
            ShowUi();
        }

        /// <summary>
        /// Proxy to manage New Game button click
        /// </summary>
        public void StartButtonProxy()
        {
            onStartGameButtonClickEvent.Invoke();
        }

        /// <summary>
        /// Proxy to manage the Settings button click
        /// </summary>
        public void SettingsButtonProxy()
        {
            onSettingsButtonClickEvent.Invoke();
        }

        public void CreditsButtonProxy()
        {
            onCreditsButtonClickEvent.Invoke();
        }
        
        /// <summary>
        /// Proxy to manage Load button click
        /// </summary>
        public void LoadButtonProxy()
        {
            onLoadButtonClickEvent.Invoke();
        }

        /// <summary>
        /// Proxy to Exit To Desktop button
        /// </summary>
        public void ExitButtonProxy()
        {
            onExitToDesktopButtonClickEvent.Invoke();
        }
        
        /// <summary>
        /// Implementation of InitControls
        /// </summary>
        public void InitControls()
        {
        }

        /// <summary>
        /// ShowUi override
        /// </summary>
        public override void ShowUi()
        {
            base.ShowUi();
            EventSystem.current.SetSelectedGameObject(mainMenuFirstSelected);
        }

        /// <summary>
        /// HideUi override
        /// </summary>
        public override void HideUi()
        {
            base.HideUi();
        }
        
        /// <summary>
        /// Sets the version control in the Ui
        /// </summary>
        private void SetVersion()
        {
            string version = Application.version;
            versionText.text = version;
        }
    }
}