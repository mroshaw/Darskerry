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

        /// <summary>
        /// Configure the slider
        /// </summary>
        public override void Start()
        {
            base.Start();
            valueText.gameObject.SetActive(showValue);

            if (showValue)
            {
                SetValue(GetComponent<Slider>().value);
                GetComponent<Slider>().onValueChanged.AddListener(SetValue);
            }
        }

        /// <summary>
        /// Set the value
        /// </summary>
        /// <param name="value"></param>
        public void SetValue(float value)
        {
            valueText.text = $"{(int)value}";
        }
    }
}