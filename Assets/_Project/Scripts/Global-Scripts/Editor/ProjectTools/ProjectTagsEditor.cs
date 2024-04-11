using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace DaftAppleGames.Editor.ProjectTools
{
    public class ProjectTagsEditor : ProjectBaseEditor, IProjectEditor
    {
        [BoxGroup("Project Tags Settings")] public List<string> tags = new List<string>();
        [BoxGroup("Project Tags Settings")] public bool replaceExisting = true;
        private SerializedObject _tagManager;
        private SerializedProperty _tagProp;
        private int _totalTags;

        [MenuItem("Daft Apple Games/Tools/Project/Project tags setup")]
        public static void ShowWindow()
        {
            GetWindow(typeof(ProjectTagsEditor));
        }

        /// <summary>
        /// Override base class to load specific Editor settings
        /// </summary>     
        public override void LoadSettings()
        {
            base.LoadSettings();
            ProjectTagsEditorSettings tagsProjectEditorSettings = projectEditorSettings as ProjectTagsEditorSettings;
            tags = new List<string>(tagsProjectEditorSettings.allTags);
        }

        /// <summary>
        /// Override base class to apply Editor specific Configuration
        /// </summary>
        public override void RunEditorConfiguration()
        {
            UpdateTags();
        }

        /// <summary>
        /// Update Tags given loaded configuration
        /// </summary>
        private void UpdateTags()
        {
            int count = 0;
            // Get current tag list
            _tagManager = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);
            _tagProp = _tagManager.FindProperty("tags");
            _totalTags = _tagProp.arraySize;

            outputArea = "Updating project tags...\n\n";

            // Go through each tag
            foreach (string currentTag in tags)
            {
                outputArea += "Processing " + currentTag + "\n";

                // No existing tag at that index
                if (count >= _totalTags)
                {
                    outputArea += $"New tag required for : {currentTag}\n";
                    AddNewTag(count, currentTag);
                    outputArea += $"Tag {currentTag} configured at position {count}.\n";
                }
                else
                {
                    // Check for existing
                    SerializedProperty existingTag = _tagProp.GetArrayElementAtIndex(count);
                    string existingTagValue = existingTag.stringValue;

                    // Check for existing but empty
                    if (existingTagValue.Equals(string.Empty))
                    {
                        outputArea += $"Empty Tag at position {count}. Setting to new Tag: {currentTag}\n";
                        AddNewTag(count, currentTag);
                        outputArea += $"Layer {currentTag} configured at position {count}.\n";
                    }
                    else
                    {
                        // Check for existing, populated
                        if (existingTagValue.Equals(currentTag))
                        {
                            outputArea += $"Tag at position {count} already set to {currentTag}. Skipping.\n";
                        }
                        else
                        {
                            // Tag exists and is different
                            outputArea += $"Tag at position {count} already set to {existingTagValue}.\n";
                            if (replaceExisting)
                            {
                                outputArea += $"Updating existing Tag...\n";
                                AddNewTag(count, currentTag);
                                outputArea += $"Tag {currentTag} configured at position {count}.\n";
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
            outputArea += "\n" + count + " tags processed.";
        }

        /// <summary>
        /// Adds a new layer into the tag manager
        /// </summary>
        /// <param name="tagPosition"></param>
        /// <param name="tagName"></param>
        private void AddNewTag(int tagPosition, string tagName)
        {
            if (reportOnly)
            {
                return;
            }
            if (tagPosition >= _totalTags)
            {
                _tagProp.InsertArrayElementAtIndex(tagPosition);
            }

            SerializedProperty tagToUpdate = _tagProp.GetArrayElementAtIndex(tagPosition);
            tagToUpdate.stringValue = tagName;
            _tagManager.ApplyModifiedPropertiesWithoutUndo();
        }
    }

}