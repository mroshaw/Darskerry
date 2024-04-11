#if INVECTOR_SHOOTER
using UnityEngine;

namespace DaftAppleGames.Editor.CharacterTools
{
        /// <summary>
    /// Scriptable Object to store Editor usable instances of the Player Character Configuration
    /// </summary>
    [CreateAssetMenu(fileName = "PlayerCharacterSettings", menuName = "Daft Apple Games/Characters/Player character settings", order = 1)]
    public class PlayerCharacterSettings : BaseCharacterSettings
    {
        [Header("Stamina and Health")]
        public float staminaRecovery;
        public float jumpStamina;
        public float rollStamina;
    }
}
#endif