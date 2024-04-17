using UnityEngine;
using Sirenix.OdinInspector;

namespace DaftAppleGames.Common.Characters
{
    /// <summary>
    /// Scriptable Object: TODO Purpose and Summary
    /// </summary>
    [CreateAssetMenu(fileName = "CharacterSettings", menuName = "Daft Apple Games/Game/Character settings", order = 1)]
    public class CharacterSettings : ScriptableObject
    {
        [BoxGroup("Character Details")] public string CharacterName;
        [BoxGroup("Character Details")] public string InformalSalutation;
        [BoxGroup("Character Details")] public string FormalSalutation;
    }
}