using System;
using System.Collections.Generic;
using System.Linq;
using DaftAppleGames.Common.Buildings;
#if ENVIRO_3
using Enviro;
#endif
#if GENA_PRO
using System;
using GeNa.Core;
#endif

using Unity.AI.Navigation;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

namespace DaftAppleGames.Editor.Buildings
{
    public enum BuildingLayer { Interior, Exterior, InteriorProps, ExteriorProps }
    
    public class BuildingTools : MonoBehaviour
    {
        #region PUBLIC METHODS

        /// <summary>
        /// Raises the building above the terrain
        /// </summary>
        /// <param name="buildingGameObject"></param>
        /// <param name="settings"></param>
        public static void RepositionBuilding(GameObject buildingGameObject, BuildingPropertiesEditorSettings settings)
        {
            // Get the position on terrain
            float terrainHeight = Terrain.activeTerrain.SampleHeight(buildingGameObject.transform.position);
            Vector3 newPosition = buildingGameObject.transform.position;
            newPosition.y = terrainHeight + settings.buildingHeightOffset;
            buildingGameObject.transform.position = newPosition;
        }
        
        /// <summary>
        /// Add the Building Components prefab to the building
        /// </summary>
        /// <param name="buildingGameObject"></param>
        /// <param name="settings"></param>
        /// <param name="isPrefab"></param>
        public static void AddBuildingComponents(GameObject buildingGameObject, BuildingPropertiesEditorSettings settings, bool isPrefab)
        {
            string objectName = settings.buildingComponentPrefab.name;
            Transform[] allTransforms = buildingGameObject.GetComponentsInChildren<Transform>(true);
            GameObject buildingComponents = null;
            
            bool found = false;
            
            foreach (Transform child in allTransforms)
            {
                if (child.name == objectName)
                {
                    Debug.Log($"BuildingTools: BuildingComponents already found on {buildingGameObject.name}");
                    found = true;
                    buildingComponents = child.gameObject;
                    buildingComponents.SetActive(true);
                    break;
                }
            }

            if (!found)
            {
                buildingComponents = PrefabUtility.InstantiatePrefab(settings.buildingComponentPrefab) as GameObject;
            }
            
            // Not found, so we should add a new one
            // GameObject newBuildingComponents = Instantiate(settings.buildingComponentPrefab, buildingGameObject.transform);
            buildingComponents.name = objectName;
            buildingComponents.transform.SetParent(buildingGameObject.transform);
            buildingComponents.transform.localPosition = new Vector3(0, 0, 0);
            buildingComponents.transform.localRotation = new Quaternion(0, 0, 0, 0);
            SetCustomComponents(buildingGameObject, buildingComponents, settings);
            Debug.Log($"BuildingTools: Added BuildingComponents to {buildingGameObject.name}");
            
            if(isPrefab)
            {
                EditorTools.ForcePrefabSave();
            }
        }
        
        /// <summary>
        /// Swap the interior of the building between the two provided sets of materials
        /// </summary>
        /// <param name="buildingGameObject"></param>
        /// <param name="settings"></param>
        public static void SwapInteriors(GameObject buildingGameObject, InteriorSwapperEditorSettings settings)
        {
            /*
            // Instantiate a new target part
            GameObject newGameObject = Instantiate(targetPartsList[index]);

            // Position the target part over the source
            newGameObject.transform.position = renderer.gameObject.transform.position;
            newGameObject.transform.rotation = renderer.gameObject.transform.rotation;
            newGameObject.transform.SetParent(renderer.gameObject.transform.parent);

            // Rename, to remove the "clone" part and allow reversion
            newGameObject.name = targetPartsList[index].name;

            // Delete or disable the source
            if (destroySourceGameObjects)
            {
                DestroyImmediate(renderer.gameObject, true);
            }
            else
            {
                renderer.gameObject.SetActive(false);
            }
            */
        }
        
        /// <summary>
        /// Add or update the colliders for interior lighting, culling etc
        /// </summary>
        /// <param name="buildingGameObject"></param>
        /// <param name="isPrefab"></param>
        public static void UpdateBuildingColliders(GameObject buildingGameObject, BuildingPropertiesEditorSettings settings)
        {
            Building building = buildingGameObject.GetComponent<Building>();
            BoxCollider[] allColliders = building.customComponents.GetComponentsInChildren<BoxCollider>(true);
            foreach (BoxCollider collider in allColliders)
            {
                Vector3 newScale;
                if (collider.GetComponent<InteriorCullingController>())
                {
                    // Box Collider size for "Interior Culling Controller".
                    newScale = new Vector3(building.length + settings.interiorCullingMargin, building.height, building.width + settings.interiorCullingMargin);
                }
                else if (collider.GetComponent<InteriorTriggerController>())
                {
                    // Box Collider for "Interior Trigger Controller"
                    newScale = new Vector3(building.length + settings.interiorTriggerMargin, building.height, building.width + settings.interiorTriggerMargin);
                }
                else
                {
                    newScale = new Vector3(building.length, building.height, building.width);
                }

                collider.gameObject.layer = LayerMask.NameToLayer("Triggers");
                collider.transform.localScale = new Vector3(1, 1, 1);
                collider.size = newScale;
                
                // Adjust for height and center offset
                Vector3 newPosition = new Vector3(building.lengthCenterOffset, building.height / 2.0f,0);
                collider.transform.localPosition = newPosition;

                EditorTools.ForcePrefabSave();
                
                UpdateEnviro(buildingGameObject, settings);
                
                Debug.Log($"BuildingTools: Collider {collider.gameObject.name} updated");
            }
        }
        
        /// <summary>
        /// Update the Enviro Effects Removal Zone
        /// </summary>
        public static void UpdateEnviro(GameObject buildingGameObject, BuildingPropertiesEditorSettings settings )
        {
            #if ENVIRO_3
            Building building = buildingGameObject.GetComponent<Building>();
            EnviroEffectRemovalZone zone = building.customComponents.GetComponentInChildren<EnviroEffectRemovalZone>(true);

            if (zone)
            {
                Vector3 newPosition = zone.gameObject.transform.localPosition;
                newPosition.y = building.height / 2;
                zone.gameObject.transform.localPosition = newPosition;

                float density = -2.0f;
                float radius = new [] { building.width, building.height, building.length }.Max() / 1.2f;
                float stretch = 1.0f;

                zone.density = density;
                zone.radius = radius;
                zone.stretch = stretch;
                Debug.Log("BuildingTools: Enviro updated");
            }
            #endif
        }
        
        /// <summary>
        /// Update the NavMeshAreaVolume
        /// </summary>
        public static void UpdateNavMeshAreaVolume(GameObject buildingGameObject, BuildingPropertiesEditorSettings settings)
        {
            Building building = buildingGameObject.GetComponent<Building>();
            NavMeshModifierVolume navMeshModifier = building.customComponents.GetComponentInChildren<NavMeshModifierVolume>(true);

            float buffer = settings.navMeshMargin;
            
            Vector3 newSize = new Vector3(building.length + buffer, building.height + buffer, building.width + buffer);
            
            if (navMeshModifier)
            {
                navMeshModifier.size = newSize;
                navMeshModifier.area = NavMesh.GetAreaFromName(settings.navMeshArea);
            }

            EditorTools.ForcePrefabSave();
            Debug.Log("BuildingTools: NavMeshAreaVolume updated");
        }
        
        /// <summary>
        /// Configure building properties
        /// </summary>
        /// <param name="buildingGameObject"></param>
        /// <param name="settings"></param>
        /// <param name="isPrefab"></param>
        public static void ConfigureLayersAndStaticFlags(GameObject buildingGameObject,
            BuildingPropertiesEditorSettings settings)
        {

            if (settings.propsSearchStrings.Length == 0 || settings.exteriorSearchStrings.Length == 0 ||
                settings.interiorSearchStrings.Length == 0)
            {
                Debug.Log("BuildingTools: ConfigureLayersAndStaticFlags - No search strings set for Interior, Exterior or Props. Aborting.");
                return;
            }
                
            ConfigureLayers(buildingGameObject, settings);
            ConfigureStaticFlags(buildingGameObject, settings);
        }

        /// <summary>
        /// Returns list of GameObjects representing parent(s) of building layer type
        /// </summary>
        /// <param name="buildingGameObject"></param>
        /// <param name="buildingLayer"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static GameObject[] GetParentBuildingLayerGameObjects(GameObject buildingGameObject, BuildingLayer buildingLayer,
            BuildingPropertiesEditorSettings settings)
        {
            string[] parentSearchStrings = Array.Empty<string>();
            string[] childSearchStrings = Array.Empty<string>();;

            // We have different search criteria depending on what we're looking for
            switch (buildingLayer)
            {
                case BuildingLayer.ExteriorProps:
                    parentSearchStrings = settings.exteriorSearchStrings;
                    childSearchStrings = settings.propsSearchStrings;
                    break;
                case BuildingLayer.InteriorProps:
                    parentSearchStrings = settings.interiorSearchStrings;
                    childSearchStrings = settings.propsSearchStrings;
                    break;
                case BuildingLayer.Exterior:
                    parentSearchStrings = settings.exteriorSearchStrings;
                    childSearchStrings = null;
                    break;
                case BuildingLayer.Interior:
                    parentSearchStrings = settings.interiorSearchStrings;
                    childSearchStrings = null;
                    break;
            }

            // Find the first set of parent GameObjects
            GameObject[] parentGameObjects = FindGameObjectsByName(buildingGameObject, parentSearchStrings);
            if (childSearchStrings == null)
            {
                // No children to find, so return the parent array
                return parentGameObjects;
            }

            // May be multile children, so we'll maintain a list
            List<GameObject> childGameObjectList = new List<GameObject>();
            
            foreach (GameObject parentGameObject in parentGameObjects)
            {
                GameObject[] childGameObjects = FindGameObjectsByName(parentGameObject, childSearchStrings);
                childGameObjectList.AddRange(childGameObjects.ToList());
            }

            // Return our final list as an array
            return childGameObjectList.ToArray();
        }
        
        #if GENA_PRO
        /// <summary>
        /// Apply terrain decorators
        /// </summary>
        /// <param name="buildingGameObject"></param>
        public static void ApplyTerrain(GameObject buildingGameObject, EffectType effectType, BuildingPropertiesEditorSettings settings)
        {
            Building building = buildingGameObject.GetComponent<Building>();
            if (!building)
            {
                Debug.LogError("BuildingTools: No Building component found!");
                return;
            }

            if (building.customComponents == null)
            {
                Debug.LogError("BuildingTools: BuildingComponents not set on building!");
                return;
            }

            GeNaTerrainDecorator[] allDecorators = building.customComponents.GetComponentsInChildren<GeNaTerrainDecorator>();

            foreach (GeNaTerrainDecorator terrainDecorator in allDecorators)
            {
                if (terrainDecorator.TerrainModifier.EffectType == effectType)
                {
                    // Apply height offset
                    if (effectType == EffectType.Flatten)
                    {
                        Vector3 decoratorPosition = buildingGameObject.transform.position;
                        decoratorPosition.y += settings.flattenHeightModifier - settings.buildingHeightOffset;
                        terrainDecorator.gameObject.transform.position = decoratorPosition;
                    }
                
                    // Configure decorators
                    TerrainModifier modifier = terrainDecorator.TerrainModifier;
                    modifier.NoiseEnabled = true;
                    modifier.ClearBrushTextures();
                    modifier.AddBrushTexture(settings.terrainBrush);
                    modifier.BrushIndex = 0;
                    modifier.GenerateTerrainEntity();
                    modifier.UpdateTerrain = true;
                    terrainDecorator.IsSelected = true;
                    TerrainEntity terrainEntity = modifier.GenerateTerrainEntity();
                    if (terrainEntity != null)
                    {
                        GeNaManager.GetInstance().TerrainTools.Visualize(terrainEntity);
                        terrainEntity.Dispose();
                    }
                
                    // Set decorator bounds
                    Vector3 buildingSize = CalculateLocalBounds(buildingGameObject, settings);
                    switch (effectType)
                    {
                        case EffectType.Flatten:
                            modifier.AreaOfEffect = ((int)Math.Max(buildingSize.x, buildingSize.y) * 2) + (int)settings.flattenMargin;
                            break;
                        case EffectType.ClearDetails:
                        case EffectType.ClearTrees:
                            modifier.AreaOfEffect = ((int)Math.Max(buildingSize.x, buildingSize.y) * 2) + (int)settings.clearDetailMargin;
                            break;
                    }
                
                    // Apply decorators
                    modifier.ApplyToTerrain();
                }
            }
        }
        #endif
        /// <summary>
        /// Aligns all props associate with a building
        /// </summary>
        /// <param name="buildingGameObject"></param>
        /// <param name="settings"></param>
        public static void AlignAllProps(GameObject buildingGameObject, BuildingPropertiesEditorSettings settings)
        {
            GameObject[] props = FindGameObjectsByName(buildingGameObject, settings.propsSearchStrings);
            if (props.Length == 0)
            {
                Debug.Log("BuildingTools: Can't find Props gameobject for AlignAllProps");
                return;
            }

            if (settings.propsSearchStrings.Length == 0)
            {
                Debug.Log("BuildingTools: No search strings set for PropNames. Aborting.");
                return;
            }

            foreach (GameObject parentGameObject in props)
            {
                foreach (Transform transform in parentGameObject.transform)
                {
                    if (settings.propsSearchStrings.Any(transform.gameObject.name.Contains))
                    {
                        SetObjectHeight(transform.gameObject, Terrain.activeTerrain.SampleHeight(transform.position) + settings.propAlignOffset);
                    }
                }
                
            }
            
        }

        public static void FindMeshLightLayers(GameObject buildingGameObject)
        {
            // Process all components
            MeshRenderer[] allMeshRenderers = buildingGameObject.GetComponentsInChildren<MeshRenderer>(true);
            foreach (MeshRenderer meshRenderer in allMeshRenderers)
            {
                if (meshRenderer.renderingLayerMask == 1)
                {
                    Debug.Log($"Parent: {meshRenderer.gameObject.transform.parent.gameObject.name}, Mesh: {meshRenderer.name}, Light Layers: {meshRenderer.renderingLayerMask}");
                }
            }
        }

        #endregion

        #region INTERNAL PRIVATE METHODS

        /// <summary>
        /// Adds the Building component and sets the customer component
        /// </summary>
        /// <param name="buildingGameObject"></param>
        /// <param name="customComponentObject"></param>
        private static void SetCustomComponents(GameObject buildingGameObject, GameObject customComponentObject, BuildingPropertiesEditorSettings settings)
        {
            Building building = buildingGameObject.GetComponent<Building>();
            if (!building)
            {
                building = buildingGameObject.AddComponent<Building>();
            }

            building.customComponents = customComponentObject;
            
            Vector3 buildingSize = CalculateLocalBounds(buildingGameObject, settings);
            // building.width = Math.Min(buildingSize.x, buildingSize.z);
            building.width = buildingSize.z;
            building.height = buildingSize.y;
            // building.length = Math.Max(buildingSize.x, buildingSize.z);
            building.length = buildingSize.x;

        }
        
        /// <summary>
        /// Sets the height of the given GameObject
        /// </summary>
        /// <param name="gameObject"></param>
        /// <param name="height"></param>
        private static void SetObjectHeight(GameObject gameObject, float height)
        {
            Vector3 newPosition = gameObject.transform.position;
            newPosition.y = height;
            gameObject.transform.position = newPosition;
        }
        
        /// <summary>
        /// Configure Building Layers
        /// </summary>
        /// <param name="building"></param>
        private static void ConfigureLayers(GameObject building, BuildingPropertiesEditorSettings settings)
        {
            // Find and set Exterior layer objects
            Debug.Log($"BuildingTools: Setting Exterior layer on {building.gameObject.name}");
            FindAndSetLayer(building, settings.interiorSearchStrings, settings.interiorLayer, settings);
            Debug.Log($"BuildingTools: Setting Interior layer on {building.gameObject.name}");
            FindAndSetLayer(building, settings.exteriorSearchStrings, settings.exteriorLayer, settings);
            Debug.Log($"BuildingTools: Setting Props layer on {building.gameObject.name}");
            FindAndSetLayer(building, settings.propsSearchStrings, settings.propsLayer, settings);
            Debug.Log($"BuildingTools: Setting Trigger Layers on {building.gameObject.name}");
            FindAndSetTriggerLayer(building);
        }

        /// <summary>
        /// Returns the GameObject by searching for name across all input strings
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="searchStrings"></param>
        /// <returns></returns>
        private static GameObject[] FindGameObjectsByName(GameObject parent, string[] searchStrings)
        {
            Transform[] allChildren = parent.GetComponentsInChildren<Transform>(true);
            List<GameObject> foundTransforms = new();
            foreach (Transform child in allChildren)
            {
                if(searchStrings.Any(child.gameObject.name.Contains))
                {
                    foundTransforms.Add(child.gameObject);
                }
            }
            return foundTransforms.ToArray();
        }
        
        /// <summary>
        /// Finds and sets the layer on game objects by name
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="searchStrings"></param>
        /// <param name="layer"></param>
        private static void FindAndSetLayer(GameObject parent, string[] searchStrings, string layer, BuildingPropertiesEditorSettings settings)
        {
            GameObject[] foundGameObjects = FindGameObjectsByName(parent, searchStrings);
            if (foundGameObjects.Length == 0)
            {
                Debug.Log($"Couldn't find any game objects in {parent.name} called {searchStrings} for layer {layer}");
            }
            else
            {
                foreach (GameObject parentGameObject in foundGameObjects)
                {
                    parentGameObject.layer = LayerMask.NameToLayer(layer);
                
                    Transform[] allExtTransforms = parentGameObject.GetComponentsInChildren<Transform>(true);
                    foreach (Transform childTransform in allExtTransforms)
                    {
                        if (!settings.layerExcludeSearchStrings.Any(childTransform.gameObject.name.Contains))
                        {
                            childTransform.gameObject.layer = LayerMask.NameToLayer(layer);
                        }
                    }
                    
                }
            }
        }
        
        /// <summary>
        /// Sets all Trigger colliders to the Triggers layer
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="searchStrings"></param>
        /// <param name="layer"></param>
        /// <param name="settings"></param>
        private static void FindAndSetTriggerLayer(GameObject parent)
        {

        }
        
        /// <summary>
        /// Configure Static Flags
        /// </summary>
        /// <param name="buildingGameObject"></param>
        private static void ConfigureStaticFlags(GameObject buildingGameObject, BuildingPropertiesEditorSettings settings)
        {
            // Set Static flags
            Debug.Log("Setting Static flags...");
            GameObjectUtility.SetStaticEditorFlags(buildingGameObject, settings.staticFlags);
            Transform[] allTransforms = buildingGameObject.GetComponentsInChildren<Transform>(true);
            foreach (Transform currTransform in allTransforms)
            {
                // Check if we need to ignore the game object
                if (!settings.staticIgnoreSearchStrings.Any(currTransform.gameObject.name.Contains) || settings.staticIgnoreSearchStrings.Length == 0)
                {
                    GameObjectUtility.SetStaticEditorFlags(currTransform.gameObject, settings.staticFlags);
                }
            }

            // Remove static flag on doors
            Door[] doors = buildingGameObject.GetComponentsInChildren<Door>(true);
        
            foreach (Door door in doors)
            {
                GameObjectUtility.SetStaticEditorFlags(door.gameObject, 0);
            }
            Debug.Log($"BuildingTools: Set static flags on {buildingGameObject.name}");
        }
        
        /// <summary>
        /// Calculates the size of the building exterior, excluding props
        /// </summary>
        /// <param name="buildingGameObject"></param>
        private static Vector3 CalculateLocalBounds(GameObject buildingGameObject, BuildingPropertiesEditorSettings settings)
        {
            Quaternion currentRotation = buildingGameObject.transform.rotation;
            buildingGameObject.transform.rotation = Quaternion.Euler(0f,0f,0f);
            Bounds bounds = new Bounds(buildingGameObject.transform.position, Vector3.zero);
            foreach(Renderer renderer in buildingGameObject.GetComponentsInChildren<Renderer>(true))
            {
                if ((settings.boundsCheckIncludeLayer == "" || renderer.gameObject.layer == LayerMask.NameToLayer(settings.boundsCheckIncludeLayer)) && settings.boundsCheckIncludeNames.Any(renderer.gameObject.name.Contains))
                {
                    bounds.Encapsulate(renderer.bounds);
                }
            }
            Vector3 localCenter = bounds.center - buildingGameObject.transform.position;
            bounds.center = localCenter;
            buildingGameObject.transform.rotation = currentRotation;
            return bounds.size;
        }
        #endregion
    }
}
