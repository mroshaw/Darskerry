using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DaftAppleGames.Editor.ProjectSetup
{
    public class BaseProjectEditor : OdinEditorWindow
    {
        [Header("Reporting")]
        [Tooltip("Check this to only report the number of impacted objects, and log details to the console.")]
        public bool reportOnly = false;
        [SerializeField]
        private int updatedCount;

        [Multiline(10)]
        [PropertyOrder(2)]
        [Tooltip("Summary reporting data will be shown here. Refer to the console for more detailed output.")]
        public string outputArea = "";

        /// <summary>
        /// Configure button
        /// </summary>
        [Button("Configure")]
        [Tooltip("Run the editor configuration process.")]
        private void ConfigureClick()
        {
            if(string.IsNullOrEmpty(editorSettingsName))
            {
                Debug.LogError("Please load a config file!");
                return;
            }
            RunEditorConfiguration();
        }

        [Header("Settings")]
        [PropertyOrder(1)]
        public BaseProjectEditorSettings projectEditorSettings;
        [PropertyOrder(1)]
        public string editorSettingsName;

        /// <summary>
        /// Handle the Load Settings button click
        /// </summary>
        [Button("Load Settings")]
        [PropertyOrder(1)]
        public void LoadSettingsClick()
        {
            LoadSettings();
        }

        /// <summary>
        /// Load the default settings. Should be overridden in the parent class.
        /// </summary>
        public virtual void LoadSettings()
        {
            editorSettingsName = projectEditorSettings.settingsName;
        }

        /// <summary>
        /// Runs the configuration. Should be overridden in the parent class.
        /// </summary>
        public virtual void RunEditorConfiguration()
        {
        }
    }
}
