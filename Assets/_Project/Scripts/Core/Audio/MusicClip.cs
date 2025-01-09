using System;
using UnityEngine;

namespace DaftAppleGames.Darskerry.Core.Audio
{
    [Serializable]
    public class MusicClip
    {
        [Header("Settings")]
        public string clipName;
        public AudioClip clip;
        public float volume;
        public bool loop;
    }
}