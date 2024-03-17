using DaftAppleGames.Common.Ui;
using Sirenix.OdinInspector;
using UnityEngine.Events;

namespace DaftAppleGames.Common.Settings
{
    public class GameSettingsUiWindow : BaseUiWindow
    {
        [BoxGroup("Main UI")]
        [FoldoutGroup("UI Proxy Events")]
        public UnityEvent onCancelButtonClickEvent;
        [BoxGroup("Main UI")]
        [FoldoutGroup("UI Proxy Events")]
        public UnityEvent onSaveButtonClickEvent;

        [BoxGroup("Settings UI")]
        [FoldoutGroup("UI Proxy Events")]
        public UnityEvent onAudioButtonProxyEvent;
        [BoxGroup("Settings UI")]
        [FoldoutGroup("UI Proxy Events")]
        public UnityEvent onDisplayButtonProxyEvent;
        [BoxGroup("Settings UI")]
        [FoldoutGroup("UI Proxy Events")]
        public UnityEvent onGameplayButtonProxyEvent;
        [BoxGroup("Settings UI")]
        [FoldoutGroup("UI Proxy Events")]
        public UnityEvent onPerformanceButtonProxyEvent;
        
        /// <summary>
        /// Proxy method for use in Event
        /// </summary>
        public void AudioButtonProxy()
        {
            onAudioButtonProxyEvent.Invoke();
        }

        /// <summary>
        /// Proxy method for use in Event
        /// </summary>
        public void DisplayButtonProxy()
        {
            onDisplayButtonProxyEvent.Invoke();
        }

        /// <summary>
        /// Proxy method for use in Event
        /// </summary>
        public void GameplayButtonProxy()
        {
            onGameplayButtonProxyEvent.Invoke();
        }

        /// <summary>
        /// Proxy method for use in Event
        /// </summary>
        public void PerformanceButtonProxy()
        {
            onPerformanceButtonProxyEvent.Invoke();
        }
        
        /// <summary>
        /// Proxy for Cancel button event handlers
        /// </summary>
        public void CancelButtonProxy()
        {
            onCancelButtonClickEvent.Invoke();
        }

        /// <summary>
        /// Proxy for Save button
        /// </summary>
        public void SaveButtonProxy()
        {
            onSaveButtonClickEvent.Invoke();
        }
    }
}