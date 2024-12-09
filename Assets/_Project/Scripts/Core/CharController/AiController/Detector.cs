using System;
using DaftAppleGames.Darskerry.Core.PropertyAttributes;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace DaftAppleGames.Darskerry.Core.CharController.AiController
{
    internal abstract class Detector : MonoBehaviour
    {
        #region Class Fields

        [BoxGroup("Settings")] [SerializeField] protected float detectorRange;
        [BoxGroup("Settings")] [SerializeField] public float refreshFrequency = 5.0f;

#if UNITY_EDITOR
        [PropertyOrder(3)][FoldoutGroup("Gizmos")][SerializeField] protected bool drawGizmos = true;
        [PropertyOrder(3)][FoldoutGroup("Gizmos")][SerializeField] protected Color gizmoColor1 = Color.yellow;
        [PropertyOrder(3)][FoldoutGroup("Gizmos")][SerializeField] protected Color gizmoColor2 = Color.green;
#endif

        internal LayerMask DetectionLayerMask { get; set; }
        internal string DetectionTag { get; set; }
        internal int DetectionBufferSize { get; set; }

        [PropertyOrder(2)][FoldoutGroup("Events")] public UnityEvent<DetectorTarget> newTargetDetectedEvent;
        [PropertyOrder(2)][FoldoutGroup("Events")] public UnityEvent<DetectorTarget> targetLostEvent;

        #endregion

        #region Abstract Methods

        protected internal abstract void CheckForTargets();
        protected internal abstract DetectorTarget GetClosestTarget();
        #endregion

        #region Class Methods

        protected void SetLayerMask(LayerMask layerMask)
        {
            DetectionLayerMask = layerMask;
        }

        protected void SetTag(string tag)
        {
            DetectionTag = tag;
        }

        protected void NewTargetDetected(DetectorTarget detectedTarget)
        {
            newTargetDetectedEvent.Invoke(detectedTarget);
        }

        protected void TargetLost(DetectorTarget lostTarget)
        {
            targetLostEvent.Invoke(lostTarget);
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

        protected abstract void DrawGizmos();

        protected void OnDrawGizmosSelected()
        {
            if (drawGizmos)
            {
                DrawGizmos();
            }
        }

        #endregion

    }
}