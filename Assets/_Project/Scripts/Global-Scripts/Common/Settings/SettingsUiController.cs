using UnityEngine;

namespace DaftAppleGames.Common.Settings
{
    public class SettingsUiController : MonoBehaviour
    {

        [Header("UI Configuration")]
        public GameObject uiPanel;
        public bool isUiOpen = false;

        /// <summary>
        /// Init the UI
        /// </summary>
        private void Start()
        {
            // Start with UI closed
            HideUi();
        }

        /// <summary>
        /// Display the options UI
        /// </summary>
        public virtual void OpenUi()
        {
            uiPanel.SetActive(true);
            isUiOpen = true;
        }

        /// <summary>
        /// Hide the options UI
        /// </summary>
        public virtual void HideUi()
        {
            uiPanel.SetActive(false);
            isUiOpen = false;
        }
    }
}