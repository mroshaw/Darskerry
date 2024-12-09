using DaftAppleGames.Darskerry.Core.PropertyAttributes;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
#endif
namespace DaftAppleGames.Darskerry.Core.CharController.AiController
{
    internal class ProximityDetector : Detector
    {
        #region Class Variables
        
        [PropertyOrder(4)][BoxGroup("Debug")][SerializeField] protected bool debugEnabled = true;
        [PropertyOrder(4)][BoxGroup("Debug")][SerializeField] private GameObject[] detectedTargetsDebug;

        protected DetectorTargets DetectedTargets;

        protected Collider[] OverlapSphereBuffer;
        private readonly HashSet<string> _existingGuidsBuffer = new();
        #endregion
        #region Startup
        /// <summary>
        /// Configure the component on awake
        /// </summary>   
        protected virtual void Start()
        {
            DetectedTargets = new DetectorTargets();
            OverlapSphereBuffer = new Collider[DetectionBufferSize];
        }
        #endregion
        #region Update Logic
        /* protected virtual void Update()
        {
            if (Time.frameCount % checkFrequency == 0)
            {
                CheckForTargets();
            }
        }
        */

        #endregion
        #region Class methods

        protected internal override DetectorTarget GetClosestTarget()
        {
            return DetectedTargets.GetClosestTarget();
        }

        // Return the closest target that the detector can currently see, with this tag
        public virtual GameObject GetClosestTargetWithTag(string targetTag)
        {
            return DetectedTargets.GetClosestTargetWithTag(targetTag);
        }

        protected internal override void CheckForTargets()
        {
            int objectsDetected = Physics.OverlapSphereNonAlloc(transform.position, detectorRange, OverlapSphereBuffer, DetectionLayerMask);
            RefreshTargetList(OverlapSphereBuffer, objectsDetected);

            if (debugEnabled)
            {
                detectedTargetsDebug = DetectedTargets.GetAllTargetGameObjects();
            }
        }

        protected virtual bool HasTargets()
        {
            return DetectedTargets.HasTargets();
        }

        protected void UpdateTargetDict(Collider[] detectedColliders, int numberDetected, ref DetectorTargets currentTargets)
        {
            _existingGuidsBuffer.Clear();

            for (int currCollider = 0; currCollider < numberDetected; currCollider++)
            {
                // Check if this is a new object
                ObjectGuid guid = detectedColliders[currCollider].GetComponent<ObjectGuid>();
                GameObject colliderGameObject = detectedColliders[currCollider].gameObject;

                if (guid && colliderGameObject.CompareTag(DetectionTag))
                {
                    // Add to HashSet for later reference
                    _existingGuidsBuffer.Add(guid.Guid);

                    if (!currentTargets.HasGuid(guid.Guid))
                    {
                        float distanceToTarget = GetDistanceToTarget(colliderGameObject);
                        NewTargetDetected(currentTargets.AddTarget(guid.Guid, colliderGameObject, distanceToTarget));
                    }
                }
            }
            // Check to see if there are any objects in the 'aware dictionary' that are no longer in the alert list
            List<string> keysToRemove = new List<string>();
            foreach (KeyValuePair<string, DetectorTarget> currTarget in currentTargets)
            {
                if (!_existingGuidsBuffer.Contains(currTarget.Key))
                {
                    keysToRemove.Add(currTarget.Key);
                    TargetLost(currTarget.Value);
                }
            }

            // Remove from the dictionary
            foreach (string key in keysToRemove)
            {
                currentTargets.RemoveTarget(key);
            }
        }

        protected virtual void RefreshTargetList(Collider[] awareColliders, int numberDetected)
        {
            UpdateTargetDict(awareColliders, numberDetected, ref DetectedTargets);
        }

        #endregion
        #region Editor methods
#if UNITY_EDITOR

        protected override void DrawGizmos()
        {
            // Draw a sphere for the 'alert' range
            Gizmos.color = gizmoColor1;
            Gizmos.DrawSphere(transform.position, detectorRange);
        }
#endif
        #endregion
    }
}