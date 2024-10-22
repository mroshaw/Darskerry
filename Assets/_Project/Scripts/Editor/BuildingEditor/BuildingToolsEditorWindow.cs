using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace DaftAppleGames.Darskerry.Editor.BuildingTools
{
    /// <summary>
    /// Editor window class for the main Building Tools Editor functions
    /// </summary>
    public class BuildingToolsEditorWindow : OdinEditorWindow
    {
        public enum ConfigurationFunction {Init, Layers, PropColliders, Lighting, Doors, InteriorVolumes} 
        #region CSS_STYLES
        // CSS styles
        private const string HeadingLabelStyleClass = "heading-label";
        private const string InstructionLabelStyleClass = "instruction-label";
        private const string PropertyLabelStyleClass = "property-label";
        private const string FunctionButtonStyleClass = "function-button";
        private const string UtilityButtonStyleClass = "utility-button";
        private const string ObjectScrollViewStyleClass = "object-scrollview";
        private const string ObjectScrollViewItemStyleClass = "object-scrollview-item";
        private const string ObjectScrollViewItemInvalidStyleClass = "object-scrollview-item-invalid";
        private const string LogScrollViewStyleClass = "log-scrollview";
        private const string LogTextStyleClass = "log-text";
        #endregion
        // Public fields for user selected configuration settings
        private BuildingAssetSettings _configSettings;
        private CustomBuildingSettings _customConfigSettings;
        
        // Private fields for selecting hierarchy game objects to apply changes to
        private GameObject[] _selectedGameObjects;
        private List<GameObject> _invalidSelectedGameObjects;
        private ListView _selectedListView;
        private ScrollView _selectedScrollView;
        private bool _overrideChecks = false;
        
        // Manage control status to validate user input
        private List<Button> _buttonList = new();
        
        // Logging
        private TextField _logTextField;
        
        /// <summary>
        /// Show the Editor window
        /// </summary>
        [MenuItem("Daft Apple Games/Buildings/Building Editor")]
        public static void ShowWindow()
        {
            EditorWindow editorWindow = GetWindow(typeof(BuildingToolsEditorWindow));
            editorWindow.titleContent = new GUIContent("Building Editor");
            editorWindow.Show();
        }

        /// <summary>
        /// Create the User Interface
        /// </summary>
        public void CreateGUI()
        {
            // Each editor window contains a root VisualElement object
            VisualElement root = rootVisualElement;

            // Set up the style sheet
            StyleSheet styleSheet = (StyleSheet)AssetDatabase.LoadAssetAtPath(
                "Assets/_Project/Scripts/Editor/BuildingEditor/BuildingToolsEditorWindow.uss",
                typeof(StyleSheet));
            root.styleSheets.Add(styleSheet);
            _selectedGameObjects = Selection.gameObjects;

            // Header
            Label headingLabel = new Label("Building Tools");
            headingLabel.AddToClassList(HeadingLabelStyleClass);
            root.Add(headingLabel);
            
            // Instructions 1 - select config
            Label selectConfigInstructionLabel =
                new Label("1. Select a settings file for your chosen asset:");
            selectConfigInstructionLabel.AddToClassList(InstructionLabelStyleClass);
            root.Add(selectConfigInstructionLabel);
 
            // Selected config
            ObjectField configSettingsObject = new ObjectField();
            configSettingsObject.objectType = typeof(BuildingAssetSettings);
            configSettingsObject.AddToClassList(PropertyLabelStyleClass);
            configSettingsObject.RegisterCallback<ChangeEvent<UnityEngine.Object>>((evt) =>
            {
                _configSettings = (BuildingAssetSettings)evt.newValue;
                SetControlState();
            });
            root.Add(configSettingsObject);
            
            // Instructions 2 - select custom config
            Label selectCustomConfigInstructionLabel =
                new Label("2. Select a custom settings file with the configuration to apply:");
            selectCustomConfigInstructionLabel.AddToClassList(InstructionLabelStyleClass);
            root.Add(selectCustomConfigInstructionLabel);
            
            ObjectField customConfigSettingsObject = new ObjectField();
            customConfigSettingsObject.objectType = typeof(CustomBuildingSettings);
            customConfigSettingsObject.AddToClassList(PropertyLabelStyleClass);
            customConfigSettingsObject.RegisterCallback<ChangeEvent<UnityEngine.Object>>((evt) =>
            {
                _customConfigSettings = (CustomBuildingSettings)evt.newValue;
                SetControlState();
            });
            root.Add(customConfigSettingsObject);
            
            
            // Instructions 3 - selecting objects
            Label selectObjectsInstructionLabel =
                new Label("3. Select one or more buildings in the hierarchy:");
            selectObjectsInstructionLabel.AddToClassList(InstructionLabelStyleClass);
            root.Add(selectObjectsInstructionLabel);

            // Selected objects
            Label selectedObjectsLabel = new Label("Selected Objects:");
            selectedObjectsLabel.AddToClassList(PropertyLabelStyleClass);
            root.Add(selectedObjectsLabel);

            _selectedScrollView = new ScrollView(ScrollViewMode.Vertical);
            _selectedScrollView.AddToClassList(ObjectScrollViewStyleClass);
            root.Add(_selectedScrollView);
            
            // Add name check override
            Toggle overrideCheckToggle = new Toggle()
            {
                label = "Override validation checks",
                tooltip =
                    "Check this to override the validation checks and apply changes to any Game Object. Use with caution!"
            };
            overrideCheckToggle.RegisterCallback<ChangeEvent<bool>>((evt) =>
            {
                _overrideChecks = evt.newValue;
                RefreshSelectedScrollView();
                SetControlState();
            });
            root.Add(overrideCheckToggle);
            
            // Instructions 4 - Buttons
            Label configButtonsInstructionlabel =
                new Label("4. Use the buttons below to configure the selected GameObjects:");
            configButtonsInstructionlabel.AddToClassList(InstructionLabelStyleClass);
            root.Add(configButtonsInstructionlabel);
            
            // Add the various function buttons
            AddFunctionButtons(root);
            
            // Log label
            Label logLabel = new Label("Log:");
            logLabel.AddToClassList(PropertyLabelStyleClass);
            root.Add(logLabel);
            
            // Add scrollview
            ScrollView logScrollView = new ScrollView(ScrollViewMode.Vertical);
            logScrollView.AddToClassList(LogScrollViewStyleClass);
            
            // Add log text control
            _logTextField = new TextField()
            {
                multiline = true,
            };
            _logTextField.AddToClassList(LogTextStyleClass);
            
            logScrollView.Add(_logTextField);
            root.Add(logScrollView);
            
            // Add log clear button
            AddButton(root, "Clear Log", "Clear the log text.", UtilityButtonStyleClass, ClearLog, false);
            
            // Set the initial control state to disabled
            RefreshSelectedScrollView();
            SetControlState();
            WriteToLog("Welcome to Building Tools by Daft Apple Games!");
        }

        #region UI_CREATION_METHODS
        /// <summary>
        /// Adds and configures individual function buttons
        /// </summary>
        /// <param name="root"></param>
        private void AddFunctionButtons(VisualElement root)
        {
            // Init building button
            AddButton(root, "Initialise Buildings", "Perform some validation, then adds the core components.", FunctionButtonStyleClass, InitBuildingHandler, true);

            // Add configure layers button
            AddButton(root, "Configure Layers", "Sets the GameObject layers for interiors, exteriors and props.", FunctionButtonStyleClass, ConfigureLayersHandler, true);

            // Add prop colliders button
            AddButton(root, "Configure Prop Colliders", "Add sphere, box and capsule colliders to interior and exterior props.", FunctionButtonStyleClass, AddCollidersHandler, true);
            
            // Add configure lights button
            AddButton(root, "Configure Lighting", "Adds lighting components and configured lighting with selected buildings.", FunctionButtonStyleClass, ConfigureLightsHandler, true);
            
            // Add doors button
            AddButton(root, "Configure Doors", "Adds door and door trigger components automatically on all doors within the selected buildings.", FunctionButtonStyleClass, ConfigureDoorsHandler, true);
            
            // Add configure interior volumes button
            AddButton(root, "Configure Interior Volumes", "Adds trigger colliders and components that add functionality when entering and exiting buildings.", FunctionButtonStyleClass, ConfigureInteriorVolumesHandler, true);
        }
        
        /// <summary>
        /// Adds a button to the root VisualElement
        /// </summary>
        /// <param name="root"></param>
        /// <param name="buttonText"></param>
        /// <param name="buttonToolTip"></param>
        /// <param name="cssClass"></param>
        /// <param name="methodToInvoke"></param>
        private void AddButton(VisualElement root, string buttonText, string buttonToolTip, string cssClass, Action methodToInvoke, bool addToToggleList)
        {
            // Add log clear button
            Button newButton = new Button(methodToInvoke)
            {
                text = buttonText,
                tooltip = buttonToolTip
            };
            newButton.AddToClassList(cssClass);
            if (addToToggleList)
            {
                _buttonList.Add(newButton);
            }
            root.Add(newButton);

        }
        #endregion
        #region UI_CONTROL_METHODS
        /// <summary>
        /// Refreshes the Selected Scroll View with selected GameObject names
        /// </summary>
        private void RefreshSelectedScrollView()
        {
            _invalidSelectedGameObjects = new List<GameObject>();
            foreach (VisualElement scrollItem in _selectedScrollView.Children().ToArray())
            {
                _selectedScrollView.Remove(scrollItem);
            }

            foreach (GameObject selectedGameObject in _selectedGameObjects)
            {
                Label itemLabel = new Label(selectedGameObject.name);

                // Check to see if the selected objects are valid, if the override hasn't been checked
                string itemStyleClass;
                if (_overrideChecks)
                {
                    itemStyleClass = ObjectScrollViewItemStyleClass;
                }
                else
                {
                    if (BuildingEditorTools.IsValidAssetBuilding(selectedGameObject, _configSettings))
                    {
                        itemStyleClass = ObjectScrollViewItemStyleClass;
                    }
                    else
                    {
                        itemStyleClass = ObjectScrollViewItemInvalidStyleClass;
                        _invalidSelectedGameObjects.Add(selectedGameObject);
                    }
                }
                
                itemLabel.AddToClassList(itemStyleClass);
                _selectedScrollView.Add(itemLabel);
            }
        }
        
        /// <summary>
        /// Refresh the ListView when user selects objects in the hierarchy
        /// </summary>
        private void OnSelectionChange()
        {
            _selectedGameObjects = Selection.gameObjects;
            RefreshSelectedScrollView();
            SetControlState();
        }

        /// <summary>
        /// Sets the control states depending on what user has currently selected
        /// </summary>
        private void SetControlState()
        {
            ToggleControls(_selectedGameObjects.Length != 0 && _configSettings != null
                                                            && _customConfigSettings != null && _invalidSelectedGameObjects.Count == 0);
        }
        
        /// <summary>
        /// Toggles control states to prevent users accessing functions before
        /// appropriate pre-requisites are met
        /// </summary>
        /// <param name="controlState"></param>
        private void ToggleControls(bool controlState)
        {
            foreach (Button currButton in _buttonList)
            {
                currButton.SetEnabled(controlState);
            }
        }
        #endregion
        #region LOGGING_METHODS
        /// <summary>
        /// Clears the log TextField control content
        /// </summary>
        private void ClearLog()
        {
            _logTextField.value = "";
        }
        
        /// <summary>
        /// Appends entries to the log TextView control
        /// </summary>
        /// <param name="logText"></param>
        private void WriteToLog(string logText)
        {
            string currLogText = _logTextField.value;
            currLogText += logText + "\n";
            _logTextField.value = currLogText;
            _logTextField.cursorIndex = currLogText.Length;
        }

        /// <summary>
        /// Appends a list of log entries
        /// </summary>
        /// <param name="logTextArray"></param>
        private void WriteToLog(List<string> logTextArray)
        {
            foreach (string logText in logTextArray)
            {
                WriteToLog(logText);
            }
        }
        
        #endregion
        #region CORE_FUNCTION_METHODS

        /// <summary>
        /// Checks that buildings are initialised, so that we can avoid trying to configure these.
        /// </summary>
        /// <returns></returns>
        private bool CheckBuildingsInitialised()
        {
            bool allValid = true;
            foreach (GameObject buildingGameObject in _selectedGameObjects)
            {
                if (!BuildingEditorTools.IsValidBuilding(buildingGameObject))
                {
                    allValid = false;
                    WriteToLog($"One or more of the selected GameObjects has not been initialised {buildingGameObject.name}. Please correct this and try again.");
                    break;
                }
            }
            return allValid;
        }

        /// <summary>
        /// Checks that the configuration is valid for the chosen function.
        /// </summary>
        /// <param name="function"></param>
        /// <returns></returns>
        private bool CheckConfigValid(ConfigurationFunction function)
        {
            // Check settings are valid for the function
            if (!BuildingEditorTools.IsValidFunctionSettings(function, _configSettings, _customConfigSettings,
                    out List<string> validationActivityLog))
            {
                WriteToLog(validationActivityLog);
                return false;
            }

            return true;
        }
        
        /// <summary>
        /// Handler for "Initialise Building" function button
        /// </summary>
        private void InitBuildingHandler()
        {
            WriteToLog($"Config File: {_configSettings.name}");
            WriteToLog("Initialising buildings: ");
            List<string> activityLog = new List<string>();
            foreach (GameObject buildingGameObject in _selectedGameObjects)
            {
                BuildingEditorTools.InitialiseBuilding(buildingGameObject, _configSettings, out activityLog);
                WriteToLog(activityLog);
            }
        }

        /// <summary>
        /// Handler for "Add Prop Colliders" function button
        /// </summary>
        private void AddCollidersHandler()
        {
            ConfigureBuildings(ConfigurationFunction.PropColliders);
        }

        /// <summary>
        /// Handler for "Configure Lights" function button
        /// </summary>
        private void ConfigureLightsHandler()
        {
            ConfigureBuildings(ConfigurationFunction.Lighting);
        }

        /// <summary>
        /// Handler for "Configure Layers" function button
        /// </summary>
        private void ConfigureLayersHandler()
        {
            ConfigureBuildings(ConfigurationFunction.Layers);
        }

        /// <summary>
        /// Handler for "Configure Doors" function button
        /// </summary>
        private void ConfigureDoorsHandler()
        {
            ConfigureBuildings(ConfigurationFunction.Doors);
        }

        /// <summary>
        /// Handler for "Clean Up Colliders" function button
        /// </summary>
        private void CleanupCollidersHandler()
        {
            ConfigureBuildings(ConfigurationFunction.PropColliders);
        }

        /// <summary>
        /// Handler for "Interior Volumes" function button
        /// </summary>
        private void ConfigureInteriorVolumesHandler()
        {
            ConfigureBuildings(ConfigurationFunction.InteriorVolumes);
        }

        /// <summary>
        /// Main function to call appropriate methods in BuildingEditorTools static class
        /// </summary>
        /// <param name="function"></param>
        private void ConfigureBuildings(ConfigurationFunction function)
        {
            if (!CheckBuildingsInitialised())
            {
                return;
            }

            // Check settings are valid for the function
            if (!CheckConfigValid(function))
            {
                return;
            }
            
            foreach (GameObject buildingGameObject in _selectedGameObjects)
            {
                WriteToLog($"Executing function '{function}' on '{buildingGameObject.name}'");
                
                // Execute the corresponding function for the selected GameObjects
                List<string> activityLog = new List<string>();
                switch (function)
                {
                    case ConfigurationFunction.Layers:
                        BuildingEditorTools.ConfigureBuildingLayers(buildingGameObject, _configSettings,
                            _customConfigSettings, out activityLog);
                        break;
                    case ConfigurationFunction.PropColliders:
                        break;
                }
                WriteToLog(activityLog);
                WriteToLog($"Executing function '{function}' on '{buildingGameObject.name}'. Done.");
            }
        }
        #endregion
    }
}