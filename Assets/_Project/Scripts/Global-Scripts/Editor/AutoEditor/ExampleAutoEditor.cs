using UnityEditor;
using UnityEngine;

namespace DaftAppleGames.Editor.AutoEditor
{
    public class ExampleAutoEditor : BaseAutoEditor
    {
        [Header("Example Editor Configuration")]
        public string sampleString;
        public int sampleInt;
        public float sampleFloat;
        public bool sampleBool;


        [MenuItem("Window/Example Area/Example Name")]
        public static void ShowWindow()
        {
            GetWindow(typeof(ExampleAutoEditor));
        }

        /// <summary>
        /// Override base class to load specific Editor settings
        /// </summary>      
        public override void LoadSettings()
        {
            base.LoadSettings();
            ExampleAutoEditorSettings exampleEditorSettings = autoEditorSettings as ExampleAutoEditorSettings;

            // Update editor specific config settings
            sampleString = exampleEditorSettings.sampleString;
            sampleInt = exampleEditorSettings.sampleInt;
            sampleFloat = exampleEditorSettings.sampleFloat;
            sampleBool = exampleEditorSettings.sampleBool;
        }

        /// <summary>
        /// Override base class to apply Editor specific Configuration
        /// </summary>
        /// <param name="gameObject"></param>
        public override void ConfigureGameObject(GameObject gameObject)
        {
            outputArea += $"Example processing: {gameObject.name}";
        }
    }
}
