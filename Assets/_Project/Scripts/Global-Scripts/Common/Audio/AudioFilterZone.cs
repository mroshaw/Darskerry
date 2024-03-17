using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Audio;

namespace DaftAppleGames.Common.Audio
{
    public class AudioFilterZone : MonoBehaviour
    {
        [BoxGroup("Snapshots")]public AudioMixerSnapshot indoorSnapshot;
        [BoxGroup("Snapshots")] public AudioMixerSnapshot outdoorSnapshot;
        [BoxGroup("Settings")] public float transitionTime = 0.1f;
        [BoxGroup("Layers and Tags")] public LayerMask triggerLayerMask;
        [BoxGroup("Layers and Tags")] public string[] triggerTags;

        /// <summary>
        /// Fade in filters when entering zone
        /// </summary>
        /// <param name="other"></param>
        private void OnTriggerEnter(Collider other)
        {
            foreach (string triggerTag in triggerTags)
            {
                if (other.CompareTag(triggerTag))
                {
                    bool inLayer = ((triggerLayerMask & 1 << other.gameObject.layer) == 1 << other.gameObject.layer);
                    if (inLayer)
                    {
                        FadeInFilters();
                    }
                }
            }
        }

        /// <summary>
        /// Fade out filters when leaving zone
        /// </summary>
        /// <param name="other"></param>
        private void OnTriggerExit(Collider other)
        {
            foreach (string triggerTag in triggerTags)
            {
                if (other.CompareTag(triggerTag))
                {
                    bool inLayer = ((triggerLayerMask & 1 << other.gameObject.layer) == 1 << other.gameObject.layer);
                    if (inLayer)
                    {
                        FadeOutFilters();
                    }
                }
            }
        }

        /// <summary>
        /// Fade in the filter effects
        /// </summary>
        [Button("Fade In")]
        public void FadeInFilters()
        {
            indoorSnapshot.TransitionTo(transitionTime);
        }

        /// <summary>
        /// Fade out the filter effects
        /// </summary>
        [Button("Fade Out")]
        public void FadeOutFilters()
        {
            outdoorSnapshot.TransitionTo(transitionTime);
        }
    }
}
