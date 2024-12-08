using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DaftAppleGames.Darskerry.Core.CharController.AiController
{
    public class NoiseEmitter : MonoBehaviour
    {
        #region Class Variables

        [BoxGroup("Settings")] [SerializeField] private float noiseLevel;
        [BoxGroup("Settings")] [SerializeField] private INoiseMaker _noiseMaker;

        #endregion

        #region Startup

        private void Awake()
        {
            _noiseMaker = gameObject.transform.root.GetComponent<INoiseMaker>();
        }
        #endregion

        #region Class Methods

        public void MakeNoise()
        {
            if (_noiseMaker == null)
            {
                return;
            }

            _noiseMaker.MakeNoise(noiseLevel);
        }
        #endregion

        #region Editor Methods
        #if UNITY_EDITOR
        [Button("Set Noise Maker")]
        private void SetNoiseMaker()
        {
            _noiseMaker = gameObject.transform.root.GetComponent<INoiseMaker>();
        }
        #endif
        #endregion
    }
}