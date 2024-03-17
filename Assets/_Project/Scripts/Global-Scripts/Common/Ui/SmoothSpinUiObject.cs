using UnityEngine;
using Sirenix.OdinInspector;

namespace DaftAppleGames.Common.Ui
{

    /// <summary>
    /// Simple behaviour to spin an object 360 degrees over time
    /// </summary>
    public class SmoothSpinUiObject : MonoBehaviour
    {
        // Public serializable properties
        [BoxGroup("General Settings")]
        public bool spinOnStart;
        [BoxGroup("General Settings")]
        public bool continuous = true;

        [BoxGroup("Spin Settings")]
        public float fullSpinInSeconds;

        // Controls the spinning state
        private bool _isSpinning = false;
        
        // Internal spinning variables
        private float _currentRotation;
        private float _spinTime;
        private float _startTime;

        private float _startRotation = 0.0f;
        private float _endRotation = 360.0f;

        /// <summary>
        /// Start spinning, if set to do so
        /// </summary>
        public void Start()
        {
            Debug.Log($"Start Rotation: {_startRotation.ToString()}, End Rotation: {_endRotation.ToString()}");

            if (spinOnStart)
            {
                StartSpinning();
            }
        }

        /// <summary>
        /// Spin the GameObject, if appropriate
        /// </summary>
        public void Update()
        {
            if (!_isSpinning)
            {
                return;
            }

            Spin();
        }

        /// <summary>
        /// Spin the Game Object
        /// </summary>
        private void Spin()
        {
            float timeDelta = (Time.time - _startTime) / fullSpinInSeconds;
            _currentRotation = Mathf.Lerp(_startRotation, _endRotation, timeDelta);

            transform.eulerAngles = new Vector3(transform.eulerAngles.x, _currentRotation, transform.eulerAngles.z);

            // Debug.Log($"Delta Time: {timeDelta}, LocalRotation: {newRotation.ToString()}");

            // If spin is complete, reset
            if (timeDelta > 1.0f)
            {
                if (continuous)
                {
                    _startTime = Time.time;
                }
                else
                {
                    StopSpinning();
                }
            }
        }

        /// <summary>
        /// Start the GameObject spinning
        /// </summary>
        [Button("Start Spin")]
        public void StartSpinning()
        {
            _startTime = Time.time;
            _isSpinning = true;
        }

        /// <summary>
        /// Stop the GameObject spinning
        /// </summary>
        [Button("Stop Spin")]
        public void StopSpinning()
        {
            _isSpinning = false;
        }

        [Button("Reset")]
        public void Reset()
        {
            _isSpinning = false;
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, _startRotation, transform.eulerAngles.z);
        }
    }
}
