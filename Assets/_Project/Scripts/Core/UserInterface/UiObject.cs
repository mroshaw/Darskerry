using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace DaftAppleGames.Darskerry.Core.UserInterface
{
    /// <summary>
    /// Class to underpin all UI components
    /// </summary>
    [ExecuteInEditMode]
    public abstract class UiObject : MonoBehaviour, ISelectHandler, IDeselectHandler, ICancelHandler
    {
        [BoxGroup("UI Settings")] [SerializeField] private GameObject selectFrame;
        [BoxGroup("UI Settings")] public string labelText;

        private Selectable _baseUiObject;

        public bool IsSelected { get; private set; }

        public virtual void Awake()
        {
            _baseUiObject = GetComponent<Selectable>();
            if (!_baseUiObject)
            {
                Debug.LogError($"UiObject: no base Unity component found! {gameObject}");
            }

        }

        public virtual void Start()
        {
            SetObjectState(false);
        }

        public virtual void OnSelect(BaseEventData eventData)
        {
            SetObjectState(true);
        }

        public virtual void OnDeselect(BaseEventData eventData)
        {
            SetObjectState(false);
        }

        public void OnCancel(BaseEventData eventData)
        {
            SetObjectState(false);
        }

        public void OnDisable()
        {
            SetObjectState(false);
        }

        private void SetObjectState(bool state)
        {
            IsSelected = state;
            if (selectFrame != null)
            {
                selectFrame.SetActive(state);
            }
        }

        [Button("Update label")]
        private void SetUnityObjectLabel()
        {

            TMP_Text labelTextControl = GetComponentInChildren<TMP_Text>();

            if (labelTextControl && !string.IsNullOrEmpty(labelText))
            {
                labelTextControl.text = labelText;
                gameObject.name = $"{labelText} {GetUiObjectType()}";
            }
        }

        public abstract string GetUiObjectType();
    }
}