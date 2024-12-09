using DaftAppleGames.Darskerry.Core.PropertyAttributes;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace DaftAppleGames.Darskerry.Core.CharController.AiController
{
    internal class DetectorManager : MonoBehaviour
    {
        #region Class Variables

        [BoxGroup("Settings")] [SerializeField] private LayerMask detectionLayerMask;
        [BoxGroup("Settings")] [SerializeField] [TagSelector] string detectionTag;
        [BoxGroup("Settings")][Tooltip("Maximum number of GameObjects that can be detected by the OverlapSphere. Used to avoid GC.")][SerializeField] protected int detectionBufferSize = 20;
        [BoxGroup("Settings")] [SerializeField] private Detector[] detectors;
        [BoxGroup("Events")] public UnityEvent newClosestTargetDetectedEvent;
        [BoxGroup("Events")] public UnityEvent closestTargetLostEvent;

        [BoxGroup("Debug")] [SerializeField] public DetectorTarget closestTarget;

        #endregion

        #region Startup

        private void OnEnable()
        {
            foreach (Detector detector in detectors)
            {
                detector.newTargetDetectedEvent.AddListener(NewTargetDetected);
                detector.targetLostEvent.AddListener(TargetLost);
            }
        }

        private void Awake()
        {
            foreach (Detector detector in detectors)
            {
                detector.DetectionTag = detectionTag;
                detector.DetectionBufferSize = detectionBufferSize;
                detector.DetectionLayerMask = detectionLayerMask;
            }
        }

        private void OnDisable()
        {
            foreach (Detector detector in detectors)
            {
                detector.newTargetDetectedEvent.RemoveListener(NewTargetDetected);
                detector.targetLostEvent.RemoveListener(TargetLost);
            }
        }
        #endregion

        #region Update

        private void Update()
        {
            foreach (Detector detector in detectors)
            {
                if (Time.frameCount % detector.refreshFrequency == 0)
                {
                    detector.CheckForTargets();
                }
            }
        }
        #region Class Methods

        public bool HasTarget()
        {
            return closestTarget.Target != null;
        }

        public GameObject GetClosestTarget()
        {
            return closestTarget.Target;
        }
        
        private void NewTargetDetected(DetectorTarget detectorTarget )
        {
            if (detectorTarget.Target && (detectorTarget.Distance < closestTarget.Distance || closestTarget.Target == null))
            {
                closestTarget = detectorTarget;
                newClosestTargetDetectedEvent.Invoke();
            }
        }

        private void TargetLost(DetectorTarget detectorTarget)
        {
            if (detectorTarget == closestTarget)
            {
                closestTarget = null;
                closestTargetLostEvent.Invoke();
            }
        }

        #endregion


        #endregion

        #region Editor Methods

        [Button("Refresh Detectors")]
        private void RefreshDetectors()
        {
            detectors = GetComponentsInChildren<Detector>(true);
        }
        #endregion
    }
}