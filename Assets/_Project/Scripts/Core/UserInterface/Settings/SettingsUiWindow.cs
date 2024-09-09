using UnityEngine;

namespace DaftAppleGames.Darskerry.Core.UserInterface.Settings
{
    public abstract class SettingsUiWindow : MonoBehaviour
    {
        private bool _controlsInitialised = false;

        public bool ControlsInitialised
        {
            get => _controlsInitialised;
            set => _controlsInitialised = value;
        }

        public virtual void InitControls()
        {
            _controlsInitialised = true;
        }

        public virtual void RefreshControlState()
        {
            if (!_controlsInitialised)
            {
                InitControls();
            }
        }
    }
}