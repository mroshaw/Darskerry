using DaftAppleGames.Common.Characters;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace DaftAppleGames.Editor.CharacterTools
{
    [CustomEditor(typeof(Creature))]
    public class CreatureEditor : CharacterEditor
    {
        // Public serializable properties
        [BoxGroup("Creature Editor Settings")] public string creatureName;

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