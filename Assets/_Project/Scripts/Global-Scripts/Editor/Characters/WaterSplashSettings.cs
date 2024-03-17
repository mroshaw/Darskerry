using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace DaftAppleGames.Editor.Characters
{
    [CreateAssetMenu(fileName = "WaterSplashSettings", menuName = "Settings/Character/WaterSplash", order = 1)]
    public class WaterSplashSettings : ScriptableObject
    {
        [Header("Water Splash Settings")]
        public List<AudioClip> splashClips;
        public GameObject splashFx;
        public AudioMixerGroup mixerGroup;
    }
}
