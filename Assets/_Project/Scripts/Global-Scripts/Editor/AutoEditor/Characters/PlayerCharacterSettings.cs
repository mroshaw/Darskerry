#if INVECTOR_SHOOTER
using UnityEngine;

namespace DaftAppleGames.Editor.AutoEditor.Characters
{
        /// <summary>
    /// Scriptable Object to store Editor usable instances of the Player Character Configuration
    /// </summary>
    [CreateAssetMenu(fileName = "PlayerCharacterSettings", menuName = "Settings/Characters/Player Settings", order = 1)]
    public class PlayerCharacterSettings : BaseCharacterSettings
    {
        [Header("Stamina and Health")]
        public float staminaRecovery;
        public float jumpStamina;
        public float rollStamina;
    }
}
#endif
