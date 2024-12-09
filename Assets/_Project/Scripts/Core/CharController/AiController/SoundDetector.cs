using Sirenix.OdinInspector;
using UnityEngine;
#if UNITY_EDITOR
#endif
namespace DaftAppleGames.Darskerry.Core.CharController.AiController
{
    internal class SoundDetector : ProximityDetector
    {
        [PropertyOrder(1)] [BoxGroup("Settings")] [SerializeField] private float minSoundVolume = 0.5f;

        private Collider[] _soundTargetsBuffer;

        #region Startup
        protected override void Start()
        {
            base.Start();
            _soundTargetsBuffer = new Collider[DetectionBufferSize];
        }
        #endregion
        protected internal override void CheckForTargets()
        {
            int objectsDetected = Physics.OverlapSphereNonAlloc(transform.position, detectorRange, OverlapSphereBuffer, DetectionLayerMask);
            KeepAudibleTargets();
            RefreshTargetList(_soundTargetsBuffer, objectsDetected);
        }

        private void KeepAudibleTargets()
        {
            int currTargetIndex = 0;
            foreach (Collider targetCollider in OverlapSphereBuffer)
            {
                if (targetCollider.gameObject.TryGetComponent(out INoiseMaker noiseMaker))
                {
                    if (noiseMaker.GetNoiseLevel() < minSoundVolume)
                    {
                        Debug.Log("Sound detected!");
                        _soundTargetsBuffer[currTargetIndex] = targetCollider;
                        currTargetIndex++;
                    }
                }
            }

            for (int currIndex = currTargetIndex; currIndex < _soundTargetsBuffer.Length; currIndex++)
            {
                _soundTargetsBuffer[currIndex] = null;
            }
        }
    }
}