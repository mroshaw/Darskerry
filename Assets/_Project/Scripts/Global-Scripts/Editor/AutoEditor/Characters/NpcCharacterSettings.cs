#if INVECTOR_SHOOTER
using System.Collections.Generic;
using UnityEngine;

namespace DaftAppleGames.Editor.AutoEditor.Characters
{
        /// <summary>
    /// Scriptable Object to store Editor usable instances of the Player Character Configuration
    /// </summary>
    [CreateAssetMenu(fileName = "NpcCharacterSettings", menuName = "Settings/Characters/NPC Settings", order = 1)]
    public class NpcCharacterSettings : BaseCharacterSettings
    {
        [Header("Detection")]
        public LayerMask detectLayer;
        public List<string> detectTags;
        public LayerMask obstaclesLayer;
    }
}
#endif