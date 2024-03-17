using System;
using DaftAppleGames.Editor.Common;
using DaftAppleGames.Editor.Common.Performance;
using DaftAppleGames.Editor.Common.Terrains;
#if GENA_PRO
using GeNa.Core;
#endif
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;
using TerrainTools = DaftAppleGames.Editor.Common.Terrains.TerrainTools;

namespace DaftAppleGames.Editor.Buildings
{
    public enum ApplyType { Selected, Prefabs, SceneGameObject }
    
    public class BuildingEditorWindow : OdinEditorWindow
    {
        // Display Editor Window
        [MenuItem("Window/Buildings/Building Editor")]
        public static void ShowWindow()
        {
            GetWindow(typeof(BuildingEditorWindow));
        }

        /// <summary>
        /// Refresh the selected list when scene object selection changes
        /// </summary>
        private void OnSelectionChange()
        {
            selectedGameObjects = Selection.gameObjects;
        }

        // UI layout
        // Source Objects
        [BoxGroup("Source Objects", centerLabel: true)]
        [Tooltip("What objects would you like to configure?")]
        public ApplyType applyType;
        [BoxGroup("Source Objects", centerLabel: true)]
        [Tooltip("Add prefabs to update")]
        [AssetsOnly]
        [EnableIf("applyType", ApplyType.Prefabs)]
        public GameObject[] prefabs;
        [BoxGroup("Source Objects", centerLabel: true)]
        [Tooltip("Add scene Game Objects to update")]
        [SceneObjectsOnly]
        [BoxGroup("Source Objects", centerLabel: true)]
        [EnableIf("applyType", ApplyType.SceneGameObject)]
        public GameObject[] sceneGameObjects;
        [BoxGroup("Source Objects", centerLabel: true)]
        [EnableIf("applyType", ApplyType.Selected)]
        public GameObject[] selectedGameObjects;
        
        // Preset and settings
        [BoxGroup("Preset Settings", centerLabel: true)]
        public BuildingEditorWindowSettings presetSettings;
        [BoxGroup("Preset Settings", centerLabel: true)]
        [Button("Load Presets")]
        private void LoadPresetsButton()
        {
            if (presetSettings != null)
            {
                doorSettings = presetSettings.doorEditorSettings;
                colliderSettings = presetSettings.propColliderEditorSettings;
                propertiesSettings = presetSettings.propertiesEditorSettings;
                buildingLodEditorSettings = presetSettings.lodEditorSettings;
                meshSettings = presetSettings.meshEditorSettings;
                lodSettings = presetSettings.lodEditorSettings;
                lightingSettings = presetSettings.lightingEditorSettings;
            }
        }
        [FoldoutGroup("Component Settings")]
        [InlineEditor]
        public DoorEditorSettings doorSettings;
        [FoldoutGroup("Component Settings")] 
        [InlineEditor]
        public PropColliderEditorSettings colliderSettings;
        [FoldoutGroup("Component Settings")]
        [InlineEditor]
        public BuildingPropertiesEditorSettings propertiesSettings;
        [FoldoutGroup("Component Settings")]
        [InlineEditor]
        public LodEditorSettings buildingLodEditorSettings;
        [FoldoutGroup("Component Settings")]
        [InlineEditor]
        public LodEditorSettings lodSettings;
        [FoldoutGroup("Component Settings")]
        [InlineEditor]
        public BuildingMeshEditorSettings meshSettings;
        [FoldoutGroup("Component Settings")]
        [InlineEditor]
        public LightingEditorSettings lightingSettings;
        
        // Buttons
        [Button("DO EVERYTHING", ButtonSizes.Large), GUIColor(0, 1, 0)]
        private void DoEveryThingButton()
        {
            ApplyAllComponents();
        }
        [Button("Layers and Flags")]
        private void LayersAndFlagsButton()
        {
            ApplyChanges(propertiesSettings, UpdateLayersAndFlags);
        }
        [Button("Building Components")]
        private void BuildingComponentsButton()
        {
            ApplyChanges(propertiesSettings, UpdateBuildingComponents);
        }
        [Button("Building Colliders")]
        private void BuildingCollidersButton()
        {
            ApplyChanges(propertiesSettings, UpdateBuildingColliders);
        }
        [Button("Doors")]
        private void DoorsButton()
        {
            ApplyChanges(doorSettings, UpdateDoors);
        }

        [Button("Align Exterior Props")]
        private void AlignPropsButton()
        {
            ApplyChanges(propertiesSettings, AlignExteriorProps);
        }

        [Button("Clear Details")]
        private void ClearDetailsButton()
        {
            #if GENA_PRO
            ApplyChanges(propertiesSettings, UpdateTerrain);
            #endif
        }
        [Button("Clear Trees")]
        private void ClearTreesButton()
        {
            #if GENA_PRO
            ApplyChanges(propertiesSettings, UpdateTerrain);
            #endif
        }
        [Button("Flatten Terrain")]
        private void FlattenTerrainButton()
        {
            #if GENA_PRO
            ApplyChanges(propertiesSettings, UpdateTerrain);
            #endif
        }

        [Button("Reposition")]
        private void RepositionButton()
        {
            ApplyChanges(propertiesSettings, UpdatePositionOnTerrain);
        }
        [Button("Prop Colliders")]
        private void PropColliderButtons()
        {
            ApplyChanges(colliderSettings, UpdatePropColliders);
        }
        [Button("Building LODs")]
        private void BuildingLodButton()
        {
            ApplyChanges(lodSettings, UpdateLodGroup);
        }
        
        [Button("Refresh LOD One")]
        private void LodOneButton()
        {
            LodTools.RefreshLodOne(Selection.gameObjects[0]);
        }
        
        [Button("Building Meshes")]
        private void BuildingMeshesButton()
        {
            ApplyChanges(meshSettings, UpdateMeshes);
        }
        
        [Button("Lighting")]
        private void LightingButton()
        {
            ApplyChanges(lightingSettings, UpdateLighting);
        }
        
         
        /// <summary>
        /// Execute everything in the correct order
        /// </summary>
        private void ApplyAllComponents()
        {

        }

        [Button("Find Meshes Light Layer")]
        private void FindMeshesWithLightLayer()
        {
            BuildingTools.FindMeshLightLayers(Selection.activeGameObject);
        }

        /// <summary>
        /// Apply changes, given settings and a delegate action method
        /// </summary>
        /// <param name="settings"></param>
        /// <param name="updateMethod"></param>
        private void ApplyChanges(EditorToolSettings settings, Action<GameObject[], EditorToolSettings> updateMethod)
        {
            switch (applyType)
            {
                case ApplyType.Selected:
                    if (Selection.gameObjects.Length > 0)
                    {
                        updateMethod.Invoke(Selection.gameObjects, settings);
                    }
                    break;
                case ApplyType.Prefabs:
                    if (prefabs.Length > 0)
                    {
                        updateMethod.Invoke(prefabs, settings);
                    }
                    break;
                case ApplyType.SceneGameObject:
                    if(sceneGameObjects.Length > 0)
                    {
                        updateMethod.Invoke(sceneGameObjects, settings);
                    }
                    break;
            }
            EditorTools.ForcePrefabSave();
        }
        
        /// <summary>
        /// Move building to selected height above terrain
        /// </summary>
        public void UpdatePositionOnTerrain(GameObject[] gameObjects, EditorToolSettings toolSettings)
        {
            BuildingPropertiesEditorSettings settings = toolSettings as BuildingPropertiesEditorSettings;
            // Scene game objects
            foreach (GameObject currentGameObject in gameObjects)
            {
                Debug.Log($"Reposition - Processing Scene GameObject: {currentGameObject.name}");
                BuildingTools.RepositionBuilding(currentGameObject, settings);
            }
        }

        /// <summary>
        /// Creates or updates the building LOD group
        /// </summary>
        public void UpdateLodGroup(GameObject[] gameObjects, EditorToolSettings toolSettings)
        {
            // Cast component specific settings
            LodEditorSettings settings = toolSettings as LodEditorSettings;
            
            // Process objects
            foreach (GameObject currentGameObject in gameObjects)
            {
                Debug.Log($"LodGroup - Processing Scene GameObject: {currentGameObject.name}");
                LodTools.UpdateLodGroup(currentGameObject, settings);
            }
        }
        
        /// <summary>
        /// Update all Layer and Static properties
        /// </summary>
        private void UpdateLayersAndFlags(GameObject[] gameObjects, EditorToolSettings editorSettings)
        {
            // Cast component specific settings
            BuildingPropertiesEditorSettings settings = editorSettings as BuildingPropertiesEditorSettings;
            
            // Process objects
            foreach (GameObject currentGameObject in gameObjects)
            {
                    Debug.Log($"Layers and Static - Processing Scene GameObject: {currentGameObject.name}");
                    BuildingTools.ConfigureLayersAndStaticFlags(currentGameObject, settings);
            }
        }

        /// <summary>
        /// Aligns Exterior Props to the terrain
        /// </summary>
        /// <param name="gameObjects"></param>
        /// <param name="editorSettings"></param>
        private void AlignExteriorProps(GameObject[] gameObjects, EditorToolSettings editorSettings)
        {
            // Cast component specific settings
            BuildingPropertiesEditorSettings settings = editorSettings as BuildingPropertiesEditorSettings;
            foreach (GameObject currentGameObject in gameObjects)
            {
                GameObject[] exteriorPropParent =
                    BuildingTools.GetParentBuildingLayerGameObjects(currentGameObject, BuildingLayer.ExteriorProps,
                        settings);
                foreach (GameObject childGameObject in exteriorPropParent)
                {
                    TerrainTools.AlignObjectToTerrain(childGameObject, false, false, false, false);
                }
            }
        }
        
        /// <summary>
        /// Configure all doors in all buildings
        /// </summary>
        private void UpdateDoors(GameObject[] gameObjects, EditorToolSettings editorSettings)
        {
            // Cast component specific settings
            DoorEditorSettings settings = editorSettings as DoorEditorSettings;
            
            foreach (GameObject currentGameObject in gameObjects)
            {
                Debug.Log($"Doors - Processing Scene GameObject: {currentGameObject.name}");
                DoorTools.ConfigureAllDoors(currentGameObject, settings, false);
            }
        }

        /// <summary>
        /// Configure all prop colliders
        /// </summary>
        private void UpdatePropColliders(GameObject[] gameObjects, EditorToolSettings editorSettings)
        {
            // Cast component specific settings
            PropColliderEditorSettings settings = editorSettings as PropColliderEditorSettings;;
            
            foreach (GameObject currentGameObject in gameObjects)
            {
                Debug.Log($"Colliders - Processing Scene GameObject: {currentGameObject.name}");
                ColliderTools.AddColliders(currentGameObject, settings, true);
            }
        }

        private void UpdateBuildingComponents(GameObject[] gameObjects, EditorToolSettings editorSettings)
        {
            // Cast component specific settings
            BuildingPropertiesEditorSettings settings = editorSettings as BuildingPropertiesEditorSettings;
            
            // Process objects
            foreach (GameObject currentGameObject in gameObjects)
            {
                Debug.Log($"BuildingComponents - Processing Scene GameObject: {currentGameObject.name}");
                BuildingTools.AddBuildingComponents(currentGameObject, settings, false);
            }
        }

        /// <summary>
        /// Update building colliders
        /// </summary>
        private void UpdateBuildingColliders(GameObject[] gameObjects, EditorToolSettings editorSettings)
        {
            // Cast component specific settings
            BuildingPropertiesEditorSettings settings = editorSettings as BuildingPropertiesEditorSettings;
            
            // Process objects
            foreach (GameObject currentGameObject in gameObjects)
            {
                Debug.Log($"BuildingColliders - Processing Scene GameObject: {currentGameObject.name}");
                BuildingTools.UpdateBuildingColliders(currentGameObject, settings);
            }
        }

        /// <summary>
        /// Update Enviro Effect Exclusion Zone
        /// </summary>
        private void UpdateEnviro(GameObject[] gameObjects, EditorToolSettings editorSettings)
        {
            // Cast component specific settings
            BuildingPropertiesEditorSettings settings = editorSettings as BuildingPropertiesEditorSettings;
            
            foreach (GameObject currentGameObject in gameObjects)
            {
                Debug.Log($"Enviro - Processing Scene GameObject: {currentGameObject.name}");
                BuildingTools.UpdateEnviro(currentGameObject, settings);
            }
        }
        
        #if GENA_PRO
        /// <summary>
        /// Apply flattening and terrain clearance - scene objects only
        /// </summary>
        private void UpdateTerrain(GameObject[] gameObjects, EditorToolSettings editorSettings)
        {
            /*
            // Cast component specific settings
            TerrainPropertiesEditorSettings settings = editorSettings as BuildingPropertiesEditorSettings;

            foreach (GameObject currentGameObject in gameObjects)
            {
                Debug.Log($"ApplyTerrain - Processing Scene GameObject: {currentGameObject.name} with {effectType}");
                BuildingTools.ApplyTerrain(currentGameObject, effectType, settings);
            }
            */
        }
        #endif

        /// <summary>
        /// Applies Mesh changes across buildings
        /// </summary>
        private void UpdateMeshes(GameObject[] gameObjects, EditorToolSettings settings)
        {
            Debug.Log($"Update Meshes - Processing...");
            BuildingMeshEditorSettings meshSettings = settings as BuildingMeshEditorSettings;
            // Update Interior Meshes
            // MeshRenderer[] interiorMeshes = MeshTools.FindMeshRenderersInGameObjects(gameObjects, true,
            //    meshSettings.interior.searchCriteria);

            MeshRenderer[] interiorMeshes =
                MeshTools.FindMeshRenderersInBuildingGameObjects(gameObjects, BuildingLayer.Interior);
            MeshTools.UpdateMeshProperties(interiorMeshes, meshSettings.interior);

            // Update Exterior meshes
            // MeshRenderer[] exteriorMeshes = MeshTools.FindMeshRenderersInGameObjects(gameObjects, true,
            //     meshSettings.exterior.searchCriteria);
            MeshRenderer[] exteriorMeshes =
                MeshTools.FindMeshRenderersInBuildingGameObjects(gameObjects, BuildingLayer.Exterior);
            MeshTools.UpdateMeshProperties(exteriorMeshes, meshSettings.exterior);

            // Update interior Prop meshes
            // MeshRenderer[] interiorPropMeshes = MeshTools.FindMeshRenderersInGameObjects(gameObjects, true,
            //     meshSettings.interiorProps.searchCriteria);
            MeshRenderer[] interiorPropMeshes =
                MeshTools.FindMeshRenderersInBuildingGameObjects(gameObjects, BuildingLayer.InteriorProps);
            MeshTools.UpdateMeshProperties(interiorPropMeshes, meshSettings.interiorProps);

            // Update exterior Prop meshes
            // MeshRenderer[] exteriorPropMeshes = MeshTools.FindMeshRenderersInGameObjects(gameObjects, true,
            //     meshSettings.exteriorProps.searchCriteria);
            MeshRenderer[] exteriorPropMeshes =
                MeshTools.FindMeshRenderersInBuildingGameObjects(gameObjects, BuildingLayer.ExteriorProps);
            MeshTools.UpdateMeshProperties(exteriorPropMeshes, meshSettings.exteriorProps);

            Debug.Log($"Update Meshes - Done Processing.");
        }

        /// <summary>
        /// Update building lighting
        /// </summary>
        /// <param name="gameObjects"></param>
        /// <param name="settings"></param>
        private void UpdateLighting(GameObject[] gameObjects, EditorToolSettings settings)
        {
            Debug.Log($"Update Lights - Processing...");
            Light[] allLights = LightTools.FindLightsInGameObjects(gameObjects);
            LightTools.ConfigureLightsOfType(allLights, LightType.Point, lightingSettings);
            Debug.Log($"Update Lights - Done Processing.");
        }
    }
}
