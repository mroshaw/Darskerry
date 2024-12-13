using DaftAppleGames.Darskerry.Core.UserInterface;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace DaftAppleGames.Darskerry.Core.UserInterface.Settings
{
    public class SettingsUiWindow : BaseUiWindow
    {
        [BoxGroup("UI Settings")] [SerializeField] private Button backButton;
        
        [BoxGroup("Proxy UI Events")] [SerializeField] private UnityEvent backButtonClickEvent;

        public override void Start()
        {
            base.Start();
        }

        public void BackButtonProxy()
        {
            backButtonClickEvent.Invoke();
        }
    }
}