using DaftAppleGames.Common.Characters;
using UnityEditor;
using UnityEngine;

namespace DaftAppleGames.Editor.Characters
{
    [CustomEditor(typeof(Creature))]
    public class CreatureEditor : CharacterEditor
    {
        // Public serializable properties
        [Header("Creature Editor Settings")]
        public string creatureName;

        private Creature _creature;
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            _creature = target as Creature;
            if (GUILayout.Button("Creature Button"))
            {
                
            }
        }
    }
}
