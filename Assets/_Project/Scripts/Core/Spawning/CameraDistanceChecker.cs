using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.Events;

namespace DaftAppleGames.Darskerry.Core.Spawning
{
    public class CameraDistanceChecker : MonoBehaviour
    {
        #region Class Variables

        [BoxGroup("Settings")] [SerializeField] private int checkFrequencyFrames = 10;
        [BoxGroup("Settings")] [SerializeField] private float distanceFromCamera = 10;
        [BoxGroup("Events")] public UnityEvent movedOutsideDistanceEvent;
        [BoxGroup("Events")] public UnityEvent movedInsideDistanceEvent;

        private bool _insideDistance;
        private Camera _camera;

        #endregion

        #region Startup
        /// <summary>
        /// Configure the component on awake
        /// </summary>   
        private void Awake()
        {
            _camera = Camera.main;
        }
    
        /// <summary>
        /// Configure the component on start
        /// </summary>
        private void Start()
        {
            _insideDistance = IsWithinCameraDistance();
        }

        private void Update()
        {
            if (Time.frameCount % checkFrequencyFrames == 0)
            {
                // If already inside but moves out
                if (_insideDistance && !IsWithinCameraDistance())
                {
                    _insideDistance = false;
                    movedOutsideDistanceEvent.Invoke();
                    return;
                }

                // If already outside but moved in
                if (!_insideDistance && IsWithinCameraDistance())
                {
                    _insideDistance = true;
                    movedInsideDistanceEvent.Invoke();
                }
            }
        }

        private bool IsWithinCameraDistance()
        {
            return DistanceFromCamera() <= distanceFromCamera;
        }

        private float DistanceFromCamera()
        {
            return (_camera.transform.position - transform.position).magnitude;
        }
        #endregion
    }
}