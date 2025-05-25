using System.Collections.Generic;
using UnityEngine;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using DaftAppleGames.Attributes;
#endif

namespace DaftAppleGames.Darskerry.Core.Audio
{
    [CreateAssetMenu(fileName = "AmbientAudioSO", menuName = "Audio/Ambient Audio SO", order = 1)]
    public class AmbientAudioSO : ScriptableObject
    {
        #region Public/serializable properties

        [SerializeField] private string ambientName;
        [SerializeField] private List<AudioClip> audioClips;

        #endregion

    }
}