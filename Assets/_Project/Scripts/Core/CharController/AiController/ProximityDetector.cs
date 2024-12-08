using DaftAppleGames.Darskerry.Core.PropertyAttributes;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

#if UNITY_EDITOR
#endif
namespace DaftAppleGames.Darskerry.Core.CharController.AiController
{
    public class ProximityDetector : MonoBehaviour
    {
        #region Class Variables
        [PropertyOrder(1)][BoxGroup("Sensor Configuration")][Tooltip("Only GameObjects in these layers will be detected.")][SerializeField] protected LayerMask detectionLayerMask;
        [PropertyOrder(1)][BoxGroup("Sensor Configuration")][Tooltip("Only GameObjects with this tag will be detected.")][SerializeField][TagSelector] protected string detectionTag;
        [PropertyOrder(1)][BoxGroup("Sensor Configuration")][Tooltip("Maximum number of GameObjects that can be detected by the OverlapSphere. Used to avoid GC.")][SerializeField] protected int detectionBufferSize = 20;
        [PropertyOrder(1)][BoxGroup("Proximity Sensor Configuration")][Tooltip("An GameObject that is within the OverlapSphere of this radius will be added to the 'awareObjects' list")][SerializeField] protected float proximityRange = 15;
        [PropertyOrder(1)][BoxGroup("Proximity Sensor Configuration")][Tooltip("The frequency with which the 'awareness' OverlapSphere is cast. Smaller number for greater accuracy, larger for better performance.")][SerializeField] private int checkFrequency = 5;
        [PropertyOrder(2)][FoldoutGroup("Events")] public UnityEvent<GameObject> newTargetDetectedEvent;
        [PropertyOrder(2)][FoldoutGroup("Events")] public UnityEvent<GameObject> targetLostEvent;

        [PropertyOrder(4)][BoxGroup("Debug")][SerializeField] protected bool debugEnabled = true;
        [PropertyOrder(4)][BoxGroup("Debug")][SerializeField] private GameObject[] detectedTargetsDebug;

#if UNITY_EDITOR
        [PropertyOrder(3)][FoldoutGroup("Gizmos")][SerializeField] private bool drawProximityGizmo = true;
        [PropertyOrder(3)][FoldoutGroup("Gizmos")][SerializeField] private Color proximityGizmoColor = Color.yellow;
#endif

        private DetectorTargets _detectedTargets;

        protected Collider[] OverlapSphereBuffer;
        private readonly HashSet<string> _existingGuidsBuffer = new();
        #endregion
        #region Startup
        /// <summary>
        /// Configure the component on awake
        /// </summary>   
        protected virtual void Awake()
        {
            _detectedTargets = new DetectorTargets();
            OverlapSphereBuffer = new Collider[detectionBufferSize];
        }
        #endregion
        #region Update Logic
        protected virtual void Update()
        {
            if (Time.frameCount % checkFrequency == 0)
            {
                CheckForTargets();
            }
        }
        #endregion
        #region Class methods


        // Return the closest target that the detector can currently see, with this tag
        public virtual GameObject GetClosestTargetWithTag(string targetTag)
        {
            return _detectedTargets.GetClosestTargetWithTag(targetTag);
        }

        protected virtual void CheckForTargets()
        {
            int objectsDetected = Physics.OverlapSphereNonAlloc(transform.position, proximityRange, OverlapSphereBuffer, detectionLayerMask);
            RefreshTargetList(OverlapSphereBuffer, objectsDetected);

            if (debugEnabled)
            {
                detectedTargetsDebug = _detectedTargets.GetAllTargetGameObjects();
            }
        }

        protected bool HasProximityTargets()
        {
            return _detectedTargets.HasTargets();
        }

        protected void UpdateTargetDict(Collider[] detectedColliders, int numberDetected, ref DetectorTargets currentTargets, Action<GameObject> targetAddedDelegate, Action<GameObject> targetLostDelegate)
        {
            _existingGuidsBuffer.Clear();

            for (int currCollider = 0; currCollider < numberDetected; currCollider++)
            {
                // Check if this is a new object
                ObjectGuid guid = detectedColliders[currCollider].GetComponent<ObjectGuid>();
                GameObject colliderGameObject = detectedColliders[currCollider].gameObject;

                if (guid && colliderGameObject.CompareTag(detectionTag))
                {
                    // Add to HashSet for later reference
                    _existingGuidsBuffer.Add(guid.Guid);

                    if (!currentTargets.HasGuid(guid.Guid))
                    {
                        float distanceToTarget = GetDistanceToTarget(colliderGameObject);
                        currentTargets.AddTarget(guid.Guid, colliderGameObject, distanceToTarget);
                        targetAddedDelegate.Invoke(colliderGameObject);
                    }
                }
            }
            // Check to see if there are any objects in the 'aware dictionary' that are no longer in the alert list
            List<string> keysToRemove = new List<string>();
            foreach (KeyValuePair<string, DetectorTarget> currTarget in currentTargets)
            {
                if (!_existingGuidsBuffer.Contains(currTarget.Key))
                {
                    targetLostDelegate.Invoke(currTarget.Value.Target);
                    keysToRemove.Add(currTarget.Key);
                }
            }

            // Remove from the dictionary
            foreach (string key in keysToRemove)
            {
                currentTargets.RemoveTarget(key);
            }
        }

        protected float GetDistanceToTarget(GameObject target)
        {
            return (target.transform.position - transform.position).magnitude;
        }

        protected float GetAngleToTarget(GameObject target)
        {
            // Calculate the direction from GameObject1 to GameObject2
            Vector3 directionToTarget = target.transform.position - transform.position;

            // Calculate the angle between forward vector and the direction to the target
            float angle = Vector3.Angle(transform.forward, directionToTarget);

            // Determine if the target is to the left or right of GameObject1
            // float sign = Mathf.Sign(Vector3.Dot(transform.up, Vector3.Cross(transform.forward, directionToTarget)));
            return angle;
        }

        protected virtual void TargetDetected(GameObject detectedGameObject)
        {
            // Debug.Log($"RangeDetector: {gameObject.name} detected: {detectedGameObject.name}");
            newTargetDetectedEvent.Invoke(detectedGameObject);
        }

        protected virtual void TargetLost(GameObject detectedGameObject)
        {
            // Debug.Log($"RangeDetector: {gameObject.name} lost: {detectedGameObject.name}");
            targetLostEvent.Invoke(detectedGameObject);
        }

        protected virtual void RefreshTargetList(Collider[] awareColliders, int numberDetected)
        {
            UpdateTargetDict(awareColliders, numberDetected, ref _detectedTargets, TargetDetected, TargetLost);
        }

        #endregion
        #region Editor methods
#if UNITY_EDITOR
        protected virtual void OnDrawGizmosSelected()
        {
            if (drawProximityGizmo)
            {
                // Draw a sphere for the 'alert' range
                Gizmos.color = proximityGizmoColor;
                Gizmos.DrawSphere(transform.position, proximityRange);
            }
        }
#endif
        #endregion
    }
}