using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DaftAppleGames.Darskerry.Core.CharController.AiController
{
    public class Noise : MonoBehaviour, INoiseMaker
    {
        #region Class Variables

        [BoxGroup("Settings")] [SerializeField] private float fadeDuration = 2.0f;
        [BoxGroup("Debug")] [SerializeField] private float noiseLevel;
        #endregion

        #region Update methods

        private void Update()
        {
            if (noiseLevel < 0.01f)
            {
                noiseLevel = 0.0f;
                return;
            }

            noiseLevel -= Time.deltaTime / fadeDuration;
        }
        #endregion

        #region Class methods
        public void MakeNoise(float noiseHeard)
        {
            this.noiseLevel = Math.Max(this.noiseLevel, noiseHeard);
        }

        public float GetNoiseLevel()
        {
            return noiseLevel;
        }
        #endregion
    }
}