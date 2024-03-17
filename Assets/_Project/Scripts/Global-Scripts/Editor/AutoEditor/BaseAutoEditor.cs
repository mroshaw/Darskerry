using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using System.Linq;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace DaftAppleGames.Editor.AutoEditor
{
    public class BaseAutoEditor : OdinEditorWindow
    {
        [Header("Object Identifier")]
        [Tooltip("Add strings to uniquely identify Game Objects to effect")]
        public string[] objectNameStrings;

        [Header("Configuration")]
        [Tooltip("Choose a root GameObject from the hierarchy. If left blank, the whole scene will be used.")]
        public List<GameObject> rootGameObjects;
       
        [Header("Reporting")]
        [Tooltip("Check this to only report the number of impacted objects, and log details to the console.")]
        public bool reportOnly = false;
        public int progressPercentage = 0;
        [SerializeField]
        private int updatedCount;

        [Multiline(10)]
        [PropertyOrder(2)]
        [Tooltip("Summary reporting data will be shown here. Refer to the console for more detailed output.")]
        public string outputArea = "";

        private int _numStrings;
        
        /// <summary>
        /// Configure button
        /// </summary>
        [Button("Configure")]
        [Tooltip("Run the editor configuration process.")]
        private void ConfigureClick()
        {
            if (string.IsNullOrEmpty(editorSettingsName))
            {
                Debug.LogError("Please load a config file!");
                return;
            }
            RunEditorConfiguration();
        }

        [Header("Settings")]
        [PropertyOrder(1)]
        public BaseAutoEditorSettings autoEditorSettings;
        [PropertyOrder(1)]
        public string editorSettingsName;

        /// <summary>
        /// Load Config button
        /// </summary>
        [Button("Load Settings")]
        [PropertyOrder(1)]
        private void LoadSettingsClick()
        {
            LoadSettings();
        }

        /// <summary>
        /// Load the default settings. Should be overridden in the parent class.
        /// </summary>
        public virtual void LoadSettings()
        {
            // Update config settings
            editorSettingsName = autoEditorSettings.settingsName;

            // Update object identifiers
            objectNameStrings = autoEditorSettings.objectNameStrings;

            _numStrings = objectNameStrings.Length;
        }

        /// <summary>
        /// Runs the configuration. Should be overridden in the parent class.
        /// </summary>
        public virtual void RunEditorConfiguration()
        {
            // Query all MeshFilter objects
            outputArea += $"Applying configuration {editorSettingsName}..\n";
            
            // Reset counters
            updatedCount = 0;
            
            if (rootGameObjects.Count == 0)
            {
                Debug.Log("Processing whole scene!!!");
                return;
                /*
                objects = FindObjectsOfType<MeshFilter>();
                ProcessMeshObjects(objects);
                return;
                */
            }

            foreach(GameObject rootGameObject in rootGameObjects)
            {
                Debug.Log($"Processing root object: {rootGameObject.name}...");
                ProcessRootGameObject(rootGameObject);
                Debug.Log($"Done processing root object: {rootGameObject.name}.");
            }

            outputArea += $"Done processing objects.\nObjects updated: {updatedCount}\n";
        }

        /// <summary>
        /// Process Root Game Objects
        /// </summary>
        /// <param name="rootGameObject"></param>
        public virtual void ProcessRootGameObject(GameObject rootGameObject)
        {
            Debug.Log("Processing Meshes...");
            ProcessMeshes(rootGameObject);
            UnityEditor.EditorUtility.SetDirty(rootGameObject);
            UpdatePrefab();
        }

        private void ProcessMeshes(GameObject rootGameObject)
        {
            MeshFilter[] objects = rootGameObject.GetComponentsInChildren<MeshFilter>(true);
            ProcessMeshObjects(objects);
        }
        
        /// <summary>
        /// Find lal Meshes in the GameObject and parse against the string list.
        /// </summary>
        /// <param name="objectList"></param>
        private void ProcessMeshObjects(MeshFilter[] objectList)
        {
            foreach (MeshFilter currentMesh in objectList)
            {
                var currentGameObject = currentMesh.gameObject;

                if (objectNameStrings.Any(currentMesh.name.Contains) || _numStrings == 0)
                {
                    Debug.Log($"Found matching object: {currentMesh.name}...\n");

                    // Don't process any further if "report only"
                    if (reportOnly)
                    {
                        updatedCount++;
                        continue;
                    }

                    // Configure the main component
                    Debug.Log($"Configuring: {currentMesh.name}...\n");
                    ConfigureGameObject(currentGameObject);
                    Debug.Log($"Configured successfully: {currentMesh.name}.\n");
                    updatedCount++;
                }
            }
        }
        
        /// <summary>
        /// Configure the main component
        /// </summary>
        /// <param name="gameObject"></param>
        public virtual void ConfigureGameObject(GameObject gameObject)
        {

        }
        
                
        /// <summary>
        /// Marks the prefab as dirty, so it can be saved.
        /// </summary>
        public static void UpdatePrefab()
        {
            PrefabStage prefabStage = PrefabStageUtility.GetCurrentPrefabStage();
            if (prefabStage != null)
            {
                Debug.Log("Marking Prefab Dirty...");
                EditorSceneManager.MarkSceneDirty(prefabStage.scene);
            }
        }
    }
}
