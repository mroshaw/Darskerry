using System;
using UnityEngine;
using UnityEngine.UI;

namespace DaftAppleGames.Darskerry.Core
{
    public class ProgressSlider : MonoBehaviour
    {
        private Slider _progressSlider;

        private void Awake()
        {
            _progressSlider = GetComponent<Slider>();
        }

        public void SetProgress(float progressValue)
        {
            _progressSlider.value = progressValue;
        }
    }
}