using DaftAppleGames.Common.Characters.Animals;
using UnityEditor;
using UnityEngine;

namespace DaftAppleGames.Editor.Characters.Animals

{
    [CustomEditor(typeof(Animal))]
    public class AnimalEditor : CharacterEditor
    {
        // Public serializable properties
        [Header("Animal Editor Settings")]
        public string animalName;

        private Animal _animal;
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            _animal = target as Animal;
        }
    }
}
