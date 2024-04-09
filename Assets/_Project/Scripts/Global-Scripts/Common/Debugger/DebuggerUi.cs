using System.Collections;
using DaftAppleGames.Common.Characters;
using UnityEngine;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine.UI;

namespace DaftAppleGames.Common.Debugger
{
    public class DebuggerUi : MonoBehaviour
    {
        [BoxGroup("Settings")] public KeyCode debugToggleModifierKey = KeyCode.LeftControl;
        [BoxGroup("Settings")] public KeyCode debugToggleKey = KeyCode.D;
        [BoxGroup("Settings")] public bool pausePlayer;
        [BoxGroup("UI Config")] public GameObject debugCanvas;
        [BoxGroup("UI Config")] public GameObject controlPanel;
        [BoxGroup("UI Config")] public GameObject mainPanel;
        [BoxGroup("UI Config")] public Slider transparencySlider;
        [BoxGroup("UI Config")] public float defaultTransparency = 0.2f;
        [BoxGroup("Log Config")] public TMP_Text debuggerLogText;
        [BoxGroup("Log Config")] public float logShowDuration = 3.0f;
        [BoxGroup("Log Config")] public float logFadeDuration = 3.0f;
        
        private bool _isUiOpen = false;
        private CanvasGroup _mainPanelCanvasGroup;
        private Coroutine _logFadeCoroutine;
        private bool _logFadeIsRunning = false;
        private PausePlayerHelper _pausePlayerHelper;
        private bool _playerPaused;

        public void Start()
        {
            _mainPanelCanvasGroup = mainPanel.GetComponent<CanvasGroup>();
            SetTransparencyDefault();
            _pausePlayerHelper = GetComponent<PausePlayerHelper>();
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
        /// Display the given log text
        /// </summary>
        /// <param name="logText"></param>
        public void ShowLog(string logText)
        {
            debuggerLogText.text = logText;

            // Check if fade already running and if so, kill it
            if (_logFadeIsRunning)
            {
                StopCoroutine(_logFadeCoroutine);
            }
            _logFadeCoroutine = StartCoroutine(FadeLogAsync());
        }

        /// <summary>
        /// Fades out the log text over time
        /// </summary>
        /// <returns></returns>
        private IEnumerator FadeLogAsync()
        {
            _logFadeIsRunning = true;
            debuggerLogText.color = new Color(debuggerLogText.color.r, debuggerLogText.color.g, debuggerLogText.color.b,
                1.0f);

            // Wait while the text is displayed
            float currentTime = 0f;
            while (currentTime < logShowDuration)
            {
                currentTime += Time.deltaTime;
                yield return null;
            }

            // After that, fade out
            currentTime = 0f;
            while (currentTime < logFadeDuration)
            {
                float alpha = Mathf.Lerp(1f, 0f, currentTime / logFadeDuration);
                debuggerLogText.color = new Color(debuggerLogText.color.r, debuggerLogText.color.g, debuggerLogText.color.b, alpha);
                currentTime += Time.deltaTime;
                yield return null;
            }

            _logFadeIsRunning = false;
        }
        
        /// <summary>
        /// Show the Debug UI
        /// </summary>
        public void ShowUi()
        {
            if (pausePlayer)
            {
                _pausePlayerHelper.PausePlayer();
                _playerPaused = true;
            }
            debugCanvas.SetActive(true);
            _isUiOpen = true;
        }

        /// <summary>
        /// Hide the Debug UI
        /// </summary>
        public void HideUi()
        {
            if (_playerPaused)
            {
                _pausePlayerHelper.UnpausePlayer();
                _playerPaused = false;
            }
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