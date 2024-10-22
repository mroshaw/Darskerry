using System;
using System.Runtime.CompilerServices;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
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

        [BoxGroup("Events")] public UnityEvent clickedEvent;

        private Selectable _baseUiObject;
        private Image _backgroundImage;

        public bool IsSelected { get; private set; }

        private void OnEnable()
        {
            _baseUiObject = GetComponent<Selectable>();
            if (!_baseUiObject)
            {
                Debug.LogError($"UiObject: no base Unity component found! {gameObject}");
            }

            if (_baseUiObject is Button button)
            {
                button.onClick.AddListener(UiClickProxy);
            }
        }

        private void OnDestroy()
        {
            if (_baseUiObject is Button button)
            {
                button.onClick.RemoveListener(UiClickProxy);
            }
        }

        public virtual void Awake()
        {
            _backgroundImage = GetComponent<Image>();

        }

        public virtual void Start()
        {
            SetObjectState(false);
        }

        private void UiClickProxy()
        {
            clickedEvent.Invoke();
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

        private void OnDisable()
        {
            if (_baseUiObject is Button button)
            {
                button.onClick.RemoveListener(UiClickProxy);
            }

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

        public void SetBackgroundColour(Color backgroundColor)
        {
            if (_backgroundImage)
            {
                _backgroundImage.color = backgroundColor;
            }
        }

        public void SetBackgroundImage(Sprite imageSprite)
        {
            if (_backgroundImage)
            {
                Debug.Log($"UiObject: Setting background image {imageSprite.name} on {gameObject.name}.");
                _backgroundImage.sprite = imageSprite;
            }
            else
            {
                Debug.Log($"UiObject: No image component found on: {gameObject.name}.");
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