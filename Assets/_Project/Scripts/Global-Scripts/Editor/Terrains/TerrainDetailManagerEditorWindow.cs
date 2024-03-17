using System.Collections.Generic;
using DaftAppleGames.Editor.Terrains;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;

namespace DaftAppleGames.Editor.AutoEditor
{
    /// <summary>
    /// Editor Window class to allow quick changes to Terrain Details
    /// from Scriptable Object instances
    /// </summary>
    public class TerrainDetailManagerEditorWindow : OdinEditorWindow
    {
        [BoxGroup("Settings")]
        [InlineEditor()]
        public TerrainDetailSettings detailSettings;

        [MenuItem("Window/Terrain/Terrain Detail Editor")]
        public static void ShowWindow()
        {
            GetWindow(typeof(TerrainDetailManagerEditorWindow));
        }

        [Button("Apply to Active Terrain")]
        private void ApplyButton()
        {
            ApplySettingsToTerrain();
        }

        [Button("Refresh From Active Terrain")]
        private void RefreshButton()
        {
            GetSettingsFromTerrain();
        }

        /// <summary>
        /// Recreate settings from active terrain
        /// </summary>
        private void GetSettingsFromTerrain()
        {
            if (detailSettings == null)
            {
                Debug.Log("Please select settings first.");
                return;
            }
            
            DetailPrototype[] allDetailProtoTypes = Terrain.activeTerrain.terrainData.detailPrototypes;
            
            detailSettings.terrainDetailEntries = new List<TerrainDetailSettings.TerrainDetailEntry>();
            foreach (DetailPrototype prototype in allDetailProtoTypes)
            {
                TerrainDetailSettings.TerrainDetailEntry newEntry = new TerrainDetailSettings.TerrainDetailEntry
                    {
                        prototypeGameObject = prototype.prototype,
                        maxHeight = prototype.maxHeight,
                        minHeight = prototype.minHeight,
                        maxWidth = prototype.maxWidth,
                        minWidth = prototype.minWidth
                    };

                detailSettings.terrainDetailEntries.Add(newEntry);
            }
        }
        
        /// <summary>
        /// Apply given settings to active terrain
        /// </summary>
        private void ApplySettingsToTerrain()
        {
            DetailPrototype[] allDetailProtoTypes = Terrain.activeTerrain.terrainData.detailPrototypes;
            int totalPrototypes = allDetailProtoTypes.Length;
            int totalSettings = detailSettings.terrainDetailEntries.Count;

            if (totalPrototypes != totalSettings)
            {
                Debug.Log($"Warning: There are {totalPrototypes} and {totalSettings}. Please review and refresh settings.");
                return;
            }
            
            foreach (DetailPrototype currentPrototype in allDetailProtoTypes)
            {
                TerrainDetailSettings.TerrainDetailEntry detailSetting = FindDetailPrototype(currentPrototype);
                if (detailSetting != null)
                {
                    currentPrototype.minWidth = detailSetting.minWidth;
                    currentPrototype.maxWidth = detailSetting.maxWidth;
                    currentPrototype.minHeight = detailSetting.minHeight;
                    currentPrototype.maxHeight = detailSetting.maxHeight;
                    Debug.Log($"TerrainDetail: Updated {currentPrototype.prototype.name} to W({detailSetting.minWidth}, {detailSetting.maxWidth}) H({detailSetting.minHeight}, {detailSetting.maxHeight})");
                }
                else
                {
                    Debug.Log($"Warning: Couldn't find settings entry for {currentPrototype.prototype.name}");
                }
            }

            Terrain.activeTerrain.terrainData.detailPrototypes = allDetailProtoTypes;
            Terrain.activeTerrain.Flush();
            Debug.Log($"TerrainDetail: Changes flushed to active terrain.");
        }

        /// <summary>
        /// Finds and returns the given prototype in the settings list
        /// </summary>
        /// <param name="prototype"></param>
        /// <returns></returns>
        private TerrainDetailSettings.TerrainDetailEntry FindDetailPrototype(DetailPrototype prototype)
        {
            foreach (TerrainDetailSettings.TerrainDetailEntry entry in detailSettings.terrainDetailEntries)
            {
                if (entry.prototypeGameObject.Equals(prototype.prototype))
                {
                    return entry;
                }
            }
            return null;
        }
    }
}