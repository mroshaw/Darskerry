using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Audio;

namespace DaftAppleGames.Editor.CharacterTools
{
    [CreateAssetMenu(fileName = "WaterSplashSettings", menuName = "Daft Apple Games/Character/Water splash settings", order = 1)]
    public class WaterSplashSettings : ScriptableObject
    {
        [BoxGroup("Water Splash Settings")] public List<AudioClip> splashClips;
        [BoxGroup("Water Splash Settings")] public GameObject splashFx;
        [BoxGroup("Water Splash Settings")] public AudioMixerGroup mixerGroup;
    }
}