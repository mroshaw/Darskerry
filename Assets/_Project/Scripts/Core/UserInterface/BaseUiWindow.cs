using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace DaftAppleGames.Darskerry.Core.UserInterface
{
    /// <summary>
    /// Class to underpin all UI window components
    /// </summary>
    public class BaseUiWindow : MonoBehaviour
    {
        [BoxGroup("Window UI Settings")] [SerializeField] private Canvas uiCanvas;
        [BoxGroup("Window UI Settings")] [SerializeField] private bool defaultUiState;
        [BoxGroup("Window UI Settings")] [SerializeField] private GameObject startSelectedGameObject;
        [BoxGroup("Window UI Settings")] [SerializeField] private float fadeTimeInSeconds=2.0f;
        [BoxGroup("Debug")] [SerializeField] private bool isUiOpen;
        [FoldoutGroup("Show Events")] [SerializeField] private UnityEvent onUiShowEvent;
        [FoldoutGroup("Hide Events")] [SerializeField] private UnityEvent onUiHideEvent;

        public bool IsUiOpen => isUiOpen;

        private static PlayerInput _playerInput;

        private bool _isCursorVisible;
        private CursorLockMode _cursorLockMode;

        private CanvasGroup _uiCanvasGroup;

        private void OnEnable()
        {
            // Subscribe to input changed
            if (!_playerInput)
            {
                _playerInput = FindFirstObjectByType<PlayerInput>();
            }
            _playerInput.controlsChangedEvent.AddListener(ControlSchemeChangedHandler);
        }

        private void OnDisable()
        {
            if (_playerInput)
            {
                _playerInput.controlsChangedEvent.RemoveListener(ControlSchemeChangedHandler);
            }
        }

        public virtual void Awake()
        {
            _uiCanvasGroup = uiCanvas.GetComponent<CanvasGroup>();
        }

        public virtual void Start()
        {
            // Register with controller
            if (UiController.Instance)
            {
                UiController.Instance.RegisterUiWindow(this);
            }

            // Start with UI in specified default state
            SetUiState(defaultUiState, false);
        }

        private void OnDestroy()
        {
            if (UiController.Instance)
            {
                UiController.Instance.UnRegisterUiWindow(this);
            }
        }

        private void ControlSchemeChangedHandler(PlayerInput playerInput)
        {
            Debug.Log($"Control scheme changed to: {playerInput.currentControlScheme}");

            if (isUiOpen)
            {
                // Gamepad
                if (playerInput.currentControlScheme == "Gamepad")
                {
                    Cursor.lockState = CursorLockMode.Locked;
                    Cursor.visible = false;
                }
                else
                {
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                }
            }
        }

        public void ToggleUiState()
        {
            SetUiState(!isUiOpen, true);
        }

        public virtual void ShowUi()
        {
            if (_uiCanvasGroup)
            {
                StartCoroutine(FadeCanvas(true));
                return;
            }
            // Enable the UI panel and set Event content
            SetUiState(true, true);
        }

        public virtual void HideUi()
        {
            if (_uiCanvasGroup)
            {
                StartCoroutine(FadeCanvas(false));
                return;
            }

            // Disable the UI panel
            SetUiState(false, true);
        }

        private IEnumerator FadeCanvas(bool isFadeIn)
        {
            float time = 0;

            float startAlpha = isFadeIn ? 0 : 1;
            float endAlpha = isFadeIn ? 1 : 0;
            _uiCanvasGroup.alpha = startAlpha;
            _uiCanvasGroup.interactable = false;
            // Fade in, so enable
            if (isFadeIn)
            {
                SetUiState(true, true);
            }

            while (time < fadeTimeInSeconds)
            {
                _uiCanvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, time / fadeTimeInSeconds);
                time += Time.unscaledDeltaTime;
                yield return null;
            }

            _uiCanvasGroup.alpha = endAlpha;

            // Fade out, so disable
            if (!isFadeIn)
            {
                SetUiState(false, true);
            }

            _uiCanvasGroup.interactable = true;
        }

        private void SetUiState(bool state, bool triggerEvents)
        {
            uiCanvas.gameObject.SetActive(state);
            isUiOpen = state;

            if (state)
            {
                if (startSelectedGameObject)
                {
                    EventSystem.current.SetSelectedGameObject(startSelectedGameObject);
                }

                // Make cursor visible
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;

                if (triggerEvents)
                {
                    onUiShowEvent.Invoke();
                }
            }
            else
            {
                // Restore cursor state, if all windows closed
                if (!UiController.Instance.IsAnyUiOpen)
                {
                    Cursor.visible = _isCursorVisible;
                    Cursor.lockState = _cursorLockMode;
                }

                if (triggerEvents)
                {
                    onUiHideEvent.Invoke();
                }
            }
        }
    }
}