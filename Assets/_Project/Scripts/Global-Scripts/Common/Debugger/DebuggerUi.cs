using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.UI;

namespace DaftAppleGames.Common.Debugger
{
    public class DebuggerUi : MonoBehaviour
    {
        [BoxGroup("Settings")] public KeyCode debugToggleModifierKey = KeyCode.LeftControl;
        [BoxGroup("Settings")] public KeyCode debugToggleKey = KeyCode.D;
        [BoxGroup("UI Config")] public GameObject debugCanvas;
        [BoxGroup("UI Config")] public GameObject controlPanel;
        [BoxGroup("UI Config")] public GameObject mainPanel;
        [BoxGroup("UI Config")] public Slider transparencySlider;
        [BoxGroup("UI Config")] public float defaultTransparency = 0.2f;

        private bool _isUiOpen = false;

        private CanvasGroup _mainPanelCanvasGroup;

        public void Start()
        {
            _mainPanelCanvasGroup = mainPanel.GetComponent<CanvasGroup>();
            HideUi();
        }

        /// <summary>
        /// Look for debug keycode, keep cursor visible when open
        /// </summary>
        public void Update()
        {
            // Check for keypress combo
            if (Input.GetKey(debugToggleModifierKey) && Input.GetKeyDown(debugToggleKey))
            {
                if (_isUiOpen)
                {
                    HideUi();
                }
                else
                {
                    ShowUi();
                }
            }

            // Keep the cursor visible when Debug UI is open
            if (_isUiOpen)
            {
                ForceCursor();
            }
        }

        /// <summary>
        /// Set the default control panel transparency
        /// </summary>
        private void SetTransparencyDefault()
        {
            transparencySlider.SetValueWithoutNotify(defaultTransparency);
            SetMainPanelTransparency(defaultTransparency);
        }

        /// <summary>
        /// Show the Debug UI
        /// </summary>
        public void ShowUi()
        {
            debugCanvas.SetActive(true);
            _isUiOpen = true;
        }

        /// <summary>
        /// Hide the Debug UI
        /// </summary>
        public void HideUi()
        {
            debugCanvas.SetActive(false);
            _isUiOpen = false;
        }

        /// <summary>
        /// Force the cursor to be active and visible
        /// </summary>
        private void ForceCursor()
        {
            if (!Cursor.visible)
            {
                Cursor.visible = true;
            }

            if (Cursor.lockState != CursorLockMode.None)
            {
                Cursor.lockState = CursorLockMode.None;
            }
        }

        /// <summary>
        /// Update the control panel transparency
        /// </summary>
        /// <param name="transparencyValue"></param>
        public void SetMainPanelTransparency(float transparencyValue)
        {
            _mainPanelCanvasGroup.alpha = transparencyValue;
        }
    }
}
