using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

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
        [BoxGroup("Debug")] [SerializeField] private bool isUiOpen;
        [FoldoutGroup("Show Events")] [SerializeField] private UnityEvent onUiShowEvent;
        [FoldoutGroup("Hide Events")] [SerializeField] private UnityEvent onUiHideEvent;

        public bool IsUiOpen => isUiOpen;

        public virtual void Awake()
        {
            // Start with UI in specified default state
            SetUiState(defaultUiState, false);
        }

        public virtual void Start()
        {
            // Register with controller
            if (UiController.Instance)
            {
                UiController.Instance.RegisterUiWindow(this);
            }
#if INVECTOR_SHOOTER
            _pausePlayer = GetComponent<PausePlayerHelper>();
#endif
        }

        private void OnDestroy()
        {
            if (UiController.Instance)
            {
                UiController.Instance.UnRegisterUiWindow(this);
            }
        }

        public void ToggleUiState()
        {
            SetUiState(!isUiOpen, true);
        }

        public virtual void ShowUi()
        {
            // Enable the UI panel and set Event content
            SetUiState(true, true);
        }

        public virtual void HideUi()
        {
            // Disable the UI panel
            SetUiState(false, true);
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

                if (triggerEvents)
                {
                    onUiShowEvent.Invoke();
                }
            }
            else
            {
                if (triggerEvents)
                {
                    onUiHideEvent.Invoke();
                }
            }
        }
    }
}