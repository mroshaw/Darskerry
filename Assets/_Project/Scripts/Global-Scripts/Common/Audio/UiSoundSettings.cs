using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Audio;

namespace DaftAppleGames.Common.Audio
{
    [CreateAssetMenu(fileName = "UiSoundSettings", menuName = "Settings/Custom Audio/UiSoundSettings")]
    public class UiSoundSettings : ScriptableObject
    {
        [BoxGroup("UI Sound Clips")]
        public AudioClip clickClip;
        [BoxGroup("UI Sound Clips")]
        public AudioClip bigClickClip;
        [BoxGroup("UI Sound Clips")]
        public AudioClip cancelClip;
        [BoxGroup("UI Sound Clips")]
        public AudioClip backClip;
        [BoxGroup("UI Sound Clips")]
        public AudioClip positiveClip;
        
        [BoxGroup("Mixer Settings")]
        public AudioMixerGroup mixerGroup;
    }
}
