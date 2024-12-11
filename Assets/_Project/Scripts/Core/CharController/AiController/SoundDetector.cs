using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
#if UNITY_EDITOR
#endif
namespace DaftAppleGames.Darskerry.Core.CharController.AiController
{
    internal class SoundDetector : ProximityDetector
    {
        [PropertyOrder(1)] [BoxGroup("Settings")] [SerializeField] private float minSoundVolume = 0.5f;

        [BoxGroup("Debug")] [SerializeReference] DetectorTargets _audibleTargets;
        private Collider[] _detectedBuffer;

        #region Startup
        protected override void Start()
        {
            base.Start();
            _audibleTargets = new DetectorTargets();
            _detectedBuffer = new Collider[DetectionBufferSize];
        }
        #endregion
        protected internal override void CheckForTargets(bool triggerEvents)
        {
            base.CheckForTargets(false);
            if (base.HasTargets())
            {
                CheckForAudibleTargets();
            }
        }

        private void CheckForAudibleTargets()
        {
            int detectedBufferIndex = 0;

            // Loop through the 'proximity' game objects, and see if any are within range, and making a noise over the hearing limit
            foreach (KeyValuePair<string, DetectorTarget> currTarget in DetectedTargets)
            {
                if (currTarget.Value.targetObject.TryGetComponent(out INoiseMaker noiseMaker))
                {
                    if (noiseMaker.GetNoiseLevel() > minSoundVolume)
                    {
                        // Add the target to the detected buffer, if it's not already there.
                        Collider targetCollider = currTarget.Value.targetObject.GetComponent<Collider>();
                        if (!IsColliderInArray(targetCollider, _detectedBuffer))
                        {
                            _detectedBuffer[detectedBufferIndex] = currTarget.Value.targetObject.GetComponent<Collider>();
                            Debug.Log("Added audible target...");
                            detectedBufferIndex++;
                        }
                    }
                }

                // Clear out the rest of the buffer
                for (int currIndex = detectedBufferIndex; currIndex < DetectionBufferSize; currIndex++)
                {
                    _detectedBuffer[currIndex] = null;
                }

                RefreshAudibleList(_detectedBuffer, detectedBufferIndex);
            }
        }

        private void RefreshAudibleList(Collider[] visionColliders, int numberDetected)
        {
            UpdateTargetDict(visionColliders, numberDetected, ref _audibleTargets, true);
        }
    }
}