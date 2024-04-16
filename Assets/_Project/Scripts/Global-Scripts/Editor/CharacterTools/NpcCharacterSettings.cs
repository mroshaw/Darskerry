#if INVECTOR_SHOOTER
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DaftAppleGames.Editor.CharacterTools
{
        /// <summary>
    /// Scriptable Object to store Editor usable instances of the Player Character Configuration
    /// </summary>
    [CreateAssetMenu(fileName = "NpcCharacterSettings", menuName = "Daft Apple Games/Characters/NPC settings", order = 1)]
    public class NpcCharacterSettings : BaseCharacterSettings
    {
        [BoxGroup("Detection")] public LayerMask detectLayer;
        [BoxGroup("Detection")] public List<string> detectTags;
        [BoxGroup("Detection")] public LayerMask obstaclesLayer;
    }
}
#endif