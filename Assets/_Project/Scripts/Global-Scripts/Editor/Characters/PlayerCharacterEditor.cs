using DaftAppleGames.Common.Characters;
using UnityEditor;
using UnityEngine;

namespace DaftAppleGames.Editor.Characters
{
    [CustomEditor(typeof(PlayerCharacter))]
    public class PlayerCharacterEditor : CharacterEditor
    {
        // Public serializable properties
        [Header("Player Editor Settings")]
        public string playerName;

        private PlayerCharacter _playerCharacter;
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            _playerCharacter = target as PlayerCharacter;
            if (GUILayout.Button("Player Button"))
            {
                
            }
        }
    }
}