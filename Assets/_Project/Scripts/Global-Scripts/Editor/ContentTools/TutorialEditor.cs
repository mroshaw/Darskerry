using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;
using DaftAppleGames.Common.Ui;

namespace DaftAppleGames.Editor.ContentTools
{
    [CustomEditor(typeof(Tutorial))]
    public class TutorialEditor : OdinEditor
    {
        public Tutorial tutorial;
        
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            tutorial = target as Tutorial;
            
            // Force show debug button
            if (GUILayout.Button("Show Tutorial (Force)"))
            {
                tutorial.ShowTutorial(true);
            }
            
            // Show debug button
            if (GUILayout.Button("Show Tutorial"))
            {
                tutorial.ShowTutorial(false);
            }
        }
    }
}