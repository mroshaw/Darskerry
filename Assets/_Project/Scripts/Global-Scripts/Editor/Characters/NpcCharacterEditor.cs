using DaftAppleGames.Common.Characters;
using UnityEditor;
using UnityEngine;

namespace DaftAppleGames.Editor.Characters
{
    [CustomEditor(typeof(NpcCharacter))]
    public class NpcCharacterEditor : CharacterEditor
    {
        // Public serializable properties
        [Header("NPC Editor Settings")]
        public string npcName;

        private NpcCharacter _npcCharacter;
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            _npcCharacter = target as NpcCharacter;
            if (GUILayout.Button("NPC Button"))
            {
                
            }
        }
    }
}
