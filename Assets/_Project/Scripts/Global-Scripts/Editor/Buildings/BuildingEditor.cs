using DaftAppleGames.Common.Buildings;
#if GENA_PRO
using GeNa.Core;
#endif
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;

namespace DaftAppleGames.Editor.Buildings 
{
    [CustomEditor(typeof(Building))]
    public class BuildingEditor : OdinEditor
    {
        public Building building;
        public DoorEditorSettings doorSettings;
        public BuildingPropertiesEditorSettings buildingSettings;
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            building = target as Building;
            if (GUILayout.Button("Update Colliders"))
            {
                UpdateColliders();
                UpdateEffectRemovalZones();
                UpdateNavMeshAreaVolume();
            }
            
            if (GUILayout.Button("Update Doors"))
            {
                UpdateDoors();
            }
            #if GENA_PRO
            if (GUILayout.Button("Clear Details"))
            {
            
                ApplyTerrain(EffectType.ClearDetails);
            }
            
            if (GUILayout.Button("Clear Trees"))
            {
                ApplyTerrain(EffectType.ClearTrees);
            }
            if (GUILayout.Button("Flatten Terrain"))
            {
                ApplyTerrain(EffectType.Flatten);
            }
  #endif
        }

        /// <summary>
        /// Sets all collider sizes to be the dimension of the house
        /// </summary>
        private void UpdateColliders()
        {
            // BuildingTools.UpdateBuildingColliders(building.gameObject, BuildingPropertiesEditorSettings settings);
        }

        /// <summary>
        /// Update the Enviro Effects Removal Zone
        /// </summary>
        private void UpdateEffectRemovalZones()
        {
            // BuildingTools.UpdateEnviro(building.gameObject, false);
        }

        /// <summary>
        /// Update the NavMeshAreaVolume
        /// </summary>
        private void UpdateNavMeshAreaVolume()
        {
            // BuildingTools.UpdateNavMeshAreaVolume(building.gameObject, false);
        }

        /// <summary>
        /// Apply selected settings to the building doors
        /// </summary>
        private void UpdateDoors()
        {
            // DoorTools.ConfigureAllDoors(building.gameObject, doorSettings, false);
        }

        #if GENA_PRO
        /// <summary>
        /// Apply terrain decorators to this building
        /// </summary>
        private void ApplyTerrain(EffectType effectType)
        {
#if GENA_PRO
            BuildingTools.ApplyTerrain(building.gameObject, effectType, buildingSettings);
#endif
        }
#endif
        }
}
