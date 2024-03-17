using DaftAppleGames.Common.Ui;
using Sirenix.OdinInspector;
using UnityEngine.Events;

namespace DaftAppleGames.Common.MainMenu
{
    public class CharSelectUiWindow : BaseUiWindow
    {
        [FoldoutGroup("Female Button Events")]
        public UnityEvent onEmilyButtonClickEvent;
        [FoldoutGroup("Male Button Events")]
        public UnityEvent onCallumButtonClickEvent;
        [FoldoutGroup("Back Button Events")]
        public UnityEvent onBackButtonClickEvent;
        [FoldoutGroup("Start Button Events")]
        public UnityEvent onStartGameButtonClickEvent;

        /// <summary>
        /// Emily select button proxy
        /// </summary>
        public void EmilyCharProxy()
        {
            onEmilyButtonClickEvent.Invoke();
        }

        /// <summary>
        /// Callum select button proxy
        /// </summary>
        public void CallumCharProxy()
        {
            onCallumButtonClickEvent.Invoke();
        }

        /// <summary>
        /// Back button proxy
        /// </summary>
        public void BackProxy()
        {
            onBackButtonClickEvent.Invoke();
        }

        /// <summary>
        /// Start button proxy
        /// </summary>
        public void StartGameProxy()
        {
            onStartGameButtonClickEvent.Invoke();
        }
    }
}
