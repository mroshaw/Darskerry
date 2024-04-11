using DaftAppleGames.Common.Characters.Animals;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace DaftAppleGames.Editor.CharacterTools

{
    [CustomEditor(typeof(Animal))]
    public class AnimalEditor : CharacterEditor
    {
        // Public serializable properties
        [BoxGroup("Animal Editor Settings")]
        public string animalName;

        private Animal _animal;
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            _animal = target as Animal;
        }
    }
}