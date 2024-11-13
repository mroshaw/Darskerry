using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

#if UNITY_EDITOR
#endif
namespace DaftAppleGames.Darskerry.Core.CharController.AiController
{
    public class FovDetector : RangeDetector
    {
        #region Class Variables
        [PropertyOrder(1)][BoxGroup("Vision Sensor Configuration")][Tooltip("Objects on these layers will block vision.")][SerializeField] private LayerMask blockedLayerMask;
        [PropertyOrder(1)][BoxGroup("Vision Sensor Configuration")][Tooltip("Vision cone will be projected from the eyes transform.")][SerializeField] private Transform eyesTransform;
        [PropertyOrder(1)][BoxGroup("Vision Sensor Configuration")][Tooltip("When a GameObject is in the 'awareObjects' list, a vision cone is cast at this range. If hit, the GameObject is added to the 'seeObjects' list")][SerializeField] private float visionSensorRange = 5;
        [PropertyOrder(1)][BoxGroup("Vision Sensor Configuration")][Tooltip("The angle 'sweep' of the vision sensor.")][SerializeField] private float visionSensorAngle = 170.0f;
        [PropertyOrder(1)][BoxGroup("Vision Sensor Configuration")][Tooltip("The number of rays cast across the full angle. Larger number for greater accuracy, smaller for better performance.")][SerializeField] private int sweepSteps = 10;
        [PropertyOrder(1)][BoxGroup("Vision Sensor Configuration")][Tooltip("The frequency with which the vision cone is cast. Smaller number for greater accuracy, larger for better performance.")][SerializeField] private int visionCheckFrequencyFrames = 5;
        [PropertyOrder(1)][BoxGroup("Sensor Configuration")][Tooltip("Maximum number of GameObjects that can be detected by the Ray. Used to avoid GC.")][SerializeField] private int rayBufferSize = 20;
        [PropertyOrder(2)][FoldoutGroup("Events")] public UnityEvent<GameObject> VisionDetectedEvent;
        [PropertyOrder(2)][FoldoutGroup("Events")] public UnityEvent<GameObject> VisionLostEvent;

#if UNITY_EDITOR
        [PropertyOrder(3)][FoldoutGroup("Gizmos")][SerializeField] private bool drawVisionSphereGizmo = true;
        [PropertyOrder(3)][FoldoutGroup("Gizmos")][SerializeField] private Color visionConeGizmoColor = Color.red;
        [PropertyOrder(3)][FoldoutGroup("Gizmos")][SerializeField] private int visionConeGizmoResolution = 50;
#endif
        [PropertyOrder(4)][BoxGroup("Debug")][SerializeField] private List<GameObject> _visionGameObjectsDebug;
        [PropertyOrder(4)][BoxGroup("Debug")][SerializeField] private float distanceToTargetDebug;
        [PropertyOrder(4)][BoxGroup("Debug")][SerializeField] private float angleToTargetDebug;

        private Dictionary<string, GameObject> _visionGameObjectsDict;
        private RaycastHit[] _rayHitsBuffer;
        #endregion

        #region Startup
        /// <summary>
        /// Configure the component on awake
        /// </summary>   
        protected override void Awake()
        {
            base.Awake();
            _rayHitsBuffer = new RaycastHit[rayBufferSize];
            _visionGameObjectsDict = new Dictionary<string, GameObject>();
        }
        #endregion

        #region Update Logic
        protected override void Update()
        {
            base.Update();

            // Only look for targets is we're aware of one
            if (IsAwareOfTargets && Time.frameCount % visionCheckFrequencyFrames == 0)
            {
                rangeDetectionEnabled = false;
                CheckForVisionObjects();

                // Toggle range detection off if we're tracking a vision target, otherwise let it run
                rangeDetectionEnabled = (_visionGameObjectsDict.Count == 0);
            }
        }
        #endregion

        #region Class methods
        private void CheckForVisionObjects()
        {
            // Loop through the 'aware' game objects, and see if any are within range, within the FOV angle, and not behind any blocking layers
            foreach (KeyValuePair<string, GameObject> target in AwareGameObjectsDict)
            {
                if (GetDistanceToTarget(target.Value) < visionSensorRange && GetAngleToTarget(target.Value) < visionSensorAngle / 2 && CanSeeTarget(target.Value))
                {
                    // Target spotted
                    Debug.Log("FovDetector: Target Spotted!");
                }
            }

            // Refresh
            // RefreshVisionList(_rayHitsBuffer.Select(hit => hit.collider).ToArray(), objectsDetected);

            if (debugEnabled)
            {
                _visionGameObjectsDebug = _visionGameObjectsDict.Select(kvp => kvp.Value).ToList();
            }
        }

        private float GetDistanceToTarget(GameObject target)
        {
            distanceToTargetDebug = (target.transform.position - transform.position).magnitude;
            return (target.transform.position - transform.position).magnitude;
        }

        private float GetAngleToTarget(GameObject target)
        {
            // Calculate the direction from GameObject1 to GameObject2
            Vector3 directionToTarget = target.transform.position - transform.position;

            // Calculate the angle between forward vector and the direction to the target
            float angle = Vector3.Angle(transform.forward, directionToTarget);

            // Determine if the target is to the left or right of GameObject1
            // float sign = Mathf.Sign(Vector3.Dot(transform.up, Vector3.Cross(transform.forward, directionToTarget)));

            // Apply the sign to get the signed angle
            angleToTargetDebug = angle;
            return angle;
        }

        private bool CanSeeTarget(GameObject target)
        {
            Vector3 directionToTarget = target.transform.position - eyesTransform.position;
            float maxDistance = directionToTarget.magnitude;
            Ray ray = new Ray(eyesTransform.position, directionToTarget.normalized);

            int objectsDetected = Physics.RaycastNonAlloc(ray, _rayHitsBuffer, visionSensorRange, detectionLayerMask | blockedLayerMask);

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

        private void VisionDetected(GameObject detectedGameObject)
        {
            Debug.Log($"FovDetector: {gameObject.name} detected: {detectedGameObject.name}");
            AwareDetectedEvent.Invoke(detectedGameObject);
        }

        private void VisionLost(GameObject detectedGameObject)
        {
            Debug.Log($"FovDetector: {gameObject.name} lost: {detectedGameObject.name}");
            AwareLostEvent.Invoke(detectedGameObject);
        }

        private void RefreshVisionList(Collider[] visionColliders, int numberDetected)
        {
            _visionGameObjectsDict = UpdateTargetDict(visionColliders, numberDetected, _visionGameObjectsDict, VisionDetected, VisionLost);
        }

        #endregion

        #region Editor methods
#if UNITY_EDITOR
        protected override void OnDrawGizmosSelected()
        {
            base.OnDrawGizmosSelected();
            if (drawVisionSphereGizmo)
            {
                // Draw cone for the 'vision' range
                GizmoTools.DrawConeGizmo(eyesTransform, visionSensorAngle, visionSensorRange, visionConeGizmoColor, visionConeGizmoResolution);
            }
        }
#endif
        #endregion
    }
}