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