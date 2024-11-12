using DaftAppleGames.Darskerry.Core.PropertyAttributes;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

#if UNITY_EDITOR
#endif
namespace DaftAppleGames.Darskerry.Core.PlayerController.AiBehaviours
{
    public class RangeDetector : MonoBehaviour
    {
        #region Class Variables
        [PropertyOrder(1)][BoxGroup("Sensor Configuration")][Tooltip("Only GameObjects in these layers will be detected.")][SerializeField] protected LayerMask detectionLayerMask;
        [PropertyOrder(1)][BoxGroup("Sensor Configuration")][Tooltip("Only GameObjects with this tag will be detected.")][SerializeField][TagSelector] protected string detectionTag;
        [PropertyOrder(1)][BoxGroup("Aware Sensor Configuration")][Tooltip("An GameObject that is within the OverlapSphere of this radius will be added to the 'awareObjects' list")][SerializeField] private float awareSensorRange = 15;
        [PropertyOrder(1)][BoxGroup("Aware Sensor Configuration")][Tooltip("The frequency with which the 'awareness' OverlapSphere is cast. Smaller number for greater accuracy, larger for better performance.")][SerializeField] private int awareCheckFrequencyFrames = 5;
        [PropertyOrder(1)][BoxGroup("Sensor Configuration")][Tooltip("Maximum number of GameObjects that can be detected by the Overlapsphere. Used to avoid GC.")][SerializeField] private int sphereBufferSize = 20;
        [PropertyOrder(2)][FoldoutGroup("Events")] public UnityEvent<GameObject> AwareDetectedEvent;
        [PropertyOrder(2)][FoldoutGroup("Events")] public UnityEvent<GameObject> AwareLostEvent;

#if UNITY_EDITOR
        [PropertyOrder(3)][FoldoutGroup("Gizmos")][SerializeField] private bool drawAwareSphereGizmo = true;
        [PropertyOrder(3)][FoldoutGroup("Gizmos")][SerializeField] private Color awareSphereGizmoColor = Color.yellow;
#endif
        [PropertyOrder(4)][BoxGroup("Debug")][SerializeField] protected bool debugEnabled = true;
        [PropertyOrder(4)][BoxGroup("Debug")][SerializeField] private List<GameObject> _awareGameObjectsDebug;

        public bool IsAwareOfTargets => _isAwareOfTargets;
        protected Dictionary<string, GameObject> AwareGameObjectsDict => _awareGameObjectsDict;

        protected bool rangeDetectionEnabled = true;

        private Dictionary<string, GameObject> _awareGameObjectsDict;
        private Collider[] _overlapSphereBuffer;
        private bool _isAwareOfTargets = false;
        #endregion

        #region Startup
        /// <summary>
        /// Configure the component on awake
        /// </summary>   
        protected virtual void Awake()
        {
            _overlapSphereBuffer = new Collider[sphereBufferSize];
            _awareGameObjectsDict = new Dictionary<string, GameObject>();
        }

        /// <summary>
        /// Configure the component on start
        /// </summary>
        protected virtual void Start()
        {

        }
        #endregion

        #region Update Logic
        protected virtual void Update()
        {
            if (Time.frameCount % awareCheckFrequencyFrames == 0 && rangeDetectionEnabled)
            {
                CheckForAlertObjects();
                _isAwareOfTargets = _awareGameObjectsDict.Count > 0;
            }
        }
        #endregion

        #region Class methods
        private void CheckForAlertObjects()
        {
            int objectsDetected = Physics.OverlapSphereNonAlloc(transform.position, awareSensorRange, _overlapSphereBuffer, detectionLayerMask);
            RefreshAwareList(_overlapSphereBuffer, objectsDetected);

            if (debugEnabled)
            {
                _awareGameObjectsDebug = _awareGameObjectsDict.Select(kvp => kvp.Value).ToList();
            }
        }

        protected Dictionary<string, GameObject> UpdateTargetDict(Collider[] detectedColliders, int numberDetected, Dictionary<string, GameObject> currentTargetDict, Action<GameObject> targetAddedDelegate, Action<GameObject> targetLostDelegate)
        {
            HashSet<string> existingGuids = new HashSet<string>();

            for (int currCollider = 0; currCollider < numberDetected; currCollider++)
            {
                // Check if this is a new object
                ObjectGuid _guid = detectedColliders[currCollider].GetComponent<ObjectGuid>();

                if (_guid && detectedColliders[currCollider].gameObject.CompareTag(detectionTag))
                {
                    // Add to HashSet for later reference
                    existingGuids.Add(_guid.Guid);

                    if (!currentTargetDict.ContainsKey(_guid.Guid))
                    {
                        currentTargetDict.Add(_guid.Guid, detectedColliders[currCollider].gameObject);
                        targetAddedDelegate.Invoke(detectedColliders[currCollider].gameObject);
                    }
                }
            }
            // Check to see if there are any objects in the 'aware dictionary' that are no longer in the alert list
            List<string> keysToRemove = new List<string>();
            foreach (KeyValuePair<string, GameObject> currDictEntry in currentTargetDict)
            {
                if (!existingGuids.Contains(currDictEntry.Key))
                {
                    targetLostDelegate.Invoke(currDictEntry.Value);
                    keysToRemove.Add(currDictEntry.Key);
                }
            }

            // Remove from the dictionary
            foreach (string key in keysToRemove)
            {
                currentTargetDict.Remove(key);
            }

            return currentTargetDict;
        }

        private void AwareDetected(GameObject detectedGameObject)
        {
            Debug.Log($"RangeDetector: {gameObject.name} detected: {detectedGameObject.name}");
            AwareDetectedEvent.Invoke(detectedGameObject);
        }

        private void AwareLost(GameObject detectedGameObject)
        {
            Debug.Log($"RangeDetector: {gameObject.name} lost: {detectedGameObject.name}");
            AwareLostEvent.Invoke(detectedGameObject);
        }

        private void RefreshAwareList(Collider[] awareColliders, int numberDetected)
        {
            _awareGameObjectsDict = UpdateTargetDict(awareColliders, numberDetected, _awareGameObjectsDict, AwareDetected, AwareLost);
        }
        #endregion

        #region Editor methods
#if UNITY_EDITOR
        protected virtual void OnDrawGizmosSelected()
        {
            if (drawAwareSphereGizmo)
            {
                // Draw a sphere for the 'alert' range
                Gizmos.color = awareSphereGizmoColor;
                Gizmos.DrawSphere(transform.position, awareSensorRange);
            }
        }
#endif
        #endregion
    }
}