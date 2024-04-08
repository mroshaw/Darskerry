using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DaftAppleGames.Common.UI
{
    /// <summary>
    /// Class to manage Slider UI objects
    /// </summary>
    public class UiSlider : UiObject
    {
        [Header("Slider Settings")]
        public bool showValue = false;
        public TMP_Text valueText;

        private Slider _slider;

        /// <summary>
        /// Get component references
        /// </summary>
        private void Awake()
        {
            _slider = GetComponent<Slider>();
        }

        /// <summary>
        /// Configure the slider
        /// </summary>
        public override void Start()
        {
            base.Start();
            valueText.gameObject.SetActive(showValue);

            if (showValue)
            {
                SetTextValue(_slider.value);
                GetComponent<Slider>().onValueChanged.AddListener(SetTextValue);
            }
        }

        /// <summary>
        /// Updates the slider and text without triggering notifications
        /// </summary>
        /// <param name="value"></param>
        public void SetValueWithoutNotify(float value)
        {
            _slider.SetValueWithoutNotify(value);
            if (showValue)
            {
                SetTextValue(value);
            }

        }

        /// <summary>
        /// Set the value
        /// </summary>
        /// <param name="value"></param>
        public void SetTextValue(float value)
        {
            valueText.text = $"{(int)value}";
        }
    }
}