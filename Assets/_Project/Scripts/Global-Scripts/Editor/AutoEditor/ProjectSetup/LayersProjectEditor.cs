using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DaftAppleGames.Editor.ProjectSetup
{
    public class LayersProjectEditor : BaseProjectEditor, IProjectEditor
    {
        [Header("Project Layers Settings")]
        public List<string> layers = new List<string>();
        public bool replaceExisting = true;
        private SerializedObject _tagManager;
        private SerializedProperty _layersProp;
        private int _totalLayers;

        [MenuItem("Window/Project Setup/Project Layers")]
        public static void ShowWindow()
        {
            GetWindow(typeof(LayersProjectEditor));
        }

        /// <summary>
        /// Override base class to load specific Editor settings
        /// </summary>        
        public override void LoadSettings()
        {
            base.LoadSettings();
            LayersProjectEditorSettings layersProjectEditorSettings = projectEditorSettings as LayersProjectEditorSettings;
            layers  = new List<string>(layersProjectEditorSettings.allLayers);
        }

        /// <summary>
        /// Override base class to apply Editor specific Configuration
        /// </summary>
        public override void RunEditorConfiguration()
        {
            UpdateLayers();
        }

        /// <summary>
        /// Updates Layers given loaded configuration
        /// </summary>
        private void UpdateLayers()
        {
            int count = 0;
            // Get current tag list
            _tagManager = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);
            _layersProp = _tagManager.FindProperty("layers");
            _totalLayers = _layersProp.arraySize;

            outputArea = "Updating project layers...\n\n";

            // Go through each tag
            foreach (string curentLayer in layers)
            {
                outputArea += "Processing" + curentLayer + "\n";

                // No existing tag at that index
                if (count >= _totalLayers)
                {
                    outputArea += $"New Layer required for : {curentLayer}\n";
                    AddNewLayer(count, curentLayer);
                    outputArea += $"Layer {curentLayer} configured at position {count}.\n";
                }
                else
                {
                    // Check for existing
                    SerializedProperty existingTag = _layersProp.GetArrayElementAtIndex(count);
                    string existingTagValue = existingTag.stringValue;

                    // Check for existing but empty
                    if (existingTagValue.Equals(string.Empty))
                    {
                        outputArea += $"Empty Layer at position {count}. Setting to new Layer: {curentLayer}\n";
                        AddNewLayer(count, curentLayer);
                        outputArea += $"Layer {curentLayer} configured at position {count}.\n";
                    }
                    else
                    {
                        // Check for existing, populated
                        if (existingTagValue.Equals(curentLayer))
                        {
                            outputArea += $"Layer at position {count} already set to {curentLayer}. Skipping.\n";
                        }
                        else
                        {
                            // Tag exists and is different
                            outputArea += $"Layer at position {count} already set to {existingTagValue}.\n";
                            if (replaceExisting)
                            {
                                outputArea += $"Updating existing layer...\n";
                                AddNewLayer(count, curentLayer);
                                outputArea += $"Tag {curentLayer} configured at position {count}.\n";
                            }
                            else
                            {
                                outputArea += $"Replace Existing is false. Skipping.\n";
                            }
                        }
                    }
                }
                count++;
            }
            outputArea += "\n" + count + " layers processed.";
        }

        /// <summary>
        /// Adds a new layer into the tag manager
        /// </summary>
        /// <param name="layerPosition"></param>
        /// <param name="tagName"></param>
        private void AddNewLayer(int layerPosition, string layerName)
        {
            if (reportOnly)
            {
                return;
            }
            if (layerPosition >= _totalLayers)
            {
                _layersProp.InsertArrayElementAtIndex(layerPosition);
            }

            SerializedProperty tagToUpdate = _layersProp.GetArrayElementAtIndex(layerPosition);
            tagToUpdate.stringValue = layerName;
            _tagManager.ApplyModifiedPropertiesWithoutUndo();
        }
    }

}
