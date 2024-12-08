using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

#if UNITY_EDITOR
#endif
namespace DaftAppleGames.Darskerry.Core.CharController.AiController
{
    public class FovDetectorInherited : ProximityDetector
    {
        #region Class Variables
        [PropertyOrder(1)][BoxGroup("Vision Sensor Configuration")][Tooltip("Objects on these layers will block vision.")][SerializeField] private LayerMask blockedLayerMask;
        [PropertyOrder(1)][BoxGroup("Vision Sensor Configuration")][Tooltip("Vision cone will be projected from the eyes transform.")][SerializeField] private Transform eyesTransform;
        [PropertyOrder(1)][BoxGroup("Vision Sensor Configuration")][Tooltip("Only objects within this distance are added to the target list")][SerializeField] private float visionSensorRange = 5;
        [PropertyOrder(1)][BoxGroup("Vision Sensor Configuration")][Tooltip("The angle 'sweep' of the vision sensor.")][SerializeField] private float visionSensorAngle = 170.0f;
        [PropertyOrder(1)][BoxGroup("Vision Sensor Configuration")][Tooltip("The frequency with which the vision cone is cast. Smaller number for greater accuracy, larger for better performance.")][SerializeField] private int visionCheckFrequencyFrames = 5;
        [PropertyOrder(2)][FoldoutGroup("Events")] public UnityEvent<GameObject> newTargetSeenEvent;
        [PropertyOrder(2)][FoldoutGroup("Events")] public UnityEvent<GameObject> lostSightOfTargetEvent;

        [PropertyOrder(4)][BoxGroup("Debug")][SerializeField] private GameObject[] visibleTargetsDebug;

#if UNITY_EDITOR
        [PropertyOrder(3)][FoldoutGroup("Gizmos")][SerializeField] private bool drawVisionConeGizmo = true;
        [PropertyOrder(3)][FoldoutGroup("Gizmos")][SerializeField] private Color visionConeGizmoColor = Color.red;
        [PropertyOrder(3)][FoldoutGroup("Gizmos")][SerializeField] private int visionConeGizmoResolution = 50;
#endif

        private DetectorTargets _awareTargets;
        private DetectorTargets _visibleTargets;

        private RaycastHit[] _rayHitsBuffer;
        private Collider[] _detectedBuffer;

        #endregion
        #region Startup
        /// <summary>
        /// Configure the component on awake
        /// </summary>   
        protected override void Awake()
        {
            base.Awake();
            _visibleTargets = new DetectorTargets();

            _rayHitsBuffer = new RaycastHit[detectionBufferSize];
            _detectedBuffer = new Collider[detectionBufferSize];
        }
        #endregion
        #region Update Logic
        protected override void Update()
        {

            if (!HasVisibleTargets())
            {
                base.Update();
                return;
            }

            // Only look for targets is we're aware of one
            if (HasProximityTargets() && Time.frameCount % visionCheckFrequencyFrames == 0)
            {
                CheckForVisionObjects();
            }
        }
        #endregion
        #region Class methods

        // Can the detector currently see any targets at all?
        private bool HasVisibleTargets()
        {
            return _visibleTargets.HasTargets();
        }

        // Return the closest target that the detector can currently see, with this tag
        public override GameObject GetClosestTargetWithTag(string targetTag)
        {
            return _visibleTargets.GetClosestTargetWithTag(targetTag);
        }

        private void CheckForVisionObjects()
        {
            int detectedBufferIndex = 0;

            // Loop through the 'aware' game objects, and see if any are within range, within the FOV angle, and not behind any blocking layers
            foreach (KeyValuePair<string, DetectorTarget> currTarget in _awareTargets)
            {
                if (GetDistanceToTarget(currTarget.Value.Target) < visionSensorRange && GetAngleToTarget(currTarget.Value.Target) < visionSensorAngle / 2 && CanSeeTarget(currTarget.Value.Target))
                {
                    _detectedBuffer[detectedBufferIndex] = currTarget.Value.Target.GetComponent<Collider>();
                    detectedBufferIndex++;
                }
            }

            // Clear out the rest of the buffer
            for (int currIndex = detectedBufferIndex; currIndex < detectionBufferSize; currIndex++)
            {
                _detectedBuffer[currIndex] = null;
            }

            RefreshVisionList(_detectedBuffer, detectedBufferIndex);

            if (debugEnabled)
            {
                visibleTargetsDebug = _visibleTargets.GetAllTargetGameObjects();
            }
        }

        private bool CanSeeTarget(GameObject target)
        {
            Vector3 directionToTarget = (target.transform.position + (Vector3.up * 1.5f)) - eyesTransform.position;
            // float maxDistance = directionToTarget.magnitude;
            Ray ray = new Ray(eyesTransform.position, directionToTarget.normalized);

            int objectsDetected = Physics.RaycastNonAlloc(ray, _rayHitsBuffer, visionSensorRange + 0.5f, detectionLayerMask | blockedLayerMask);

#if UNITY_EDITOR
            if (debugEnabled)
            {
                Debug.DrawRay(eyesTransform.position, directionToTarget, Color.red, 0, false);
            }
#endif

            for (int i = 0; i < objectsDetected; i++)
            {
                RaycastHit hit = _rayHitsBuffer[i];

                // Check if the hit object is in blockingLayers; if so, return false
                if (((1 << hit.collider.gameObject.layer) & blockedLayerMask) != 0)
                {
                    return false;
                }

                // Check if the hit object is in targetLayers and matches the target
                if (((1 << hit.collider.gameObject.layer) & detectionLayerMask) != 0 && hit.collider.gameObject == target)
                {
                    return true;
                }
            }

            return false;
        }

        private void NewTargetSighted(GameObject detectedGameObject)
        {
            newTargetSeenEvent.Invoke(detectedGameObject);
        }

        private void LostSightOfTarget(GameObject detectedGameObject)
        {
            lostSightOfTargetEvent.Invoke(detectedGameObject);
        }

        private void RefreshVisionList(Collider[] visionColliders, int numberDetected)
        {
            UpdateTargetDict(visionColliders, numberDetected, ref _visibleTargets, NewTargetSighted, LostSightOfTarget);
        }

        #endregion
        #region Editor methods
#if UNITY_EDITOR
        protected override void OnDrawGizmosSelected()
        {
            base.OnDrawGizmosSelected();
            if (drawVisionConeGizmo)
            {
                // Draw cone for the 'vision' range
                GizmoTools.DrawConeGizmo(eyesTransform, visionSensorAngle, visionSensorRange, visionConeGizmoColor, visionConeGizmoResolution);
            }
        }
#endif
        #endregion
    }
}