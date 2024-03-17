using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DaftAppleGames.Editor.ProjectSetup
{
    public class FoldersProjectEditor : BaseProjectEditor, IProjectEditor
    {
        [Header("Project Folder Settings")]
        public List<ProjectFolder> folders = new List<ProjectFolder>();

        [MenuItem("Window/Project Setup/Project Folders")]
        public static void ShowWindow()
        {
            GetWindow(typeof(FoldersProjectEditor));
        }

        /// <summary>
        /// Override base class to load specific Editor settings
        /// </summary>        
        public override void LoadSettings()
        {
            base.LoadSettings();
            FolderProjectEditorSettings folderProjectEditorSettings = projectEditorSettings as FolderProjectEditorSettings;
            folders = new List<ProjectFolder>(folderProjectEditorSettings.allProjectFolders);
        }

        /// <summary>
        /// Override base class to apply Editor specific Configuration
        /// </summary>
        public override void RunEditorConfiguration()
        {
            CreateFolders();
        }


        /// <summary>
        /// Creates Asset Folders, based on loaded configuration
        /// </summary>
        private void CreateFolders()
        {
            //create all the folders required in a project
            //primary and sub folders
            foreach (ProjectFolder folder in folders)
            {
                Debug.Log($"Creating {folder.FolderPath}\\{folder.FolderName}...\n");
                if(AssetDatabase.IsValidFolder($"Assets/{folder.FolderPath}/{folder.FolderName}"))
                {
                    Debug.Log($"Already exists. Skipping folder {folder.FolderPath}\\{folder.FolderName}.\n");
                }
                else
                {
                    string baseFolder = $"Assets";

                    if (!string.IsNullOrEmpty(folder.FolderPath))
                    {
                        baseFolder = $"{baseFolder}/{folder.FolderPath}";
                    }
                    string guid = AssetDatabase.CreateFolder(baseFolder, folder.FolderName);
                    if(string.IsNullOrEmpty(guid))
                    {
                        Debug.LogError($"Failed to create {folder.FolderPath}\\{folder.FolderName}!!!\n");
                    }
                    Debug.Log($"Done creating {folder.FolderPath}/{folder.FolderName}.\n");

                }
            }
            AssetDatabase.Refresh();
        }
    }
}
