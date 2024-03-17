using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;

namespace DaftAppleGames.Editor.Terrains
{
    [CreateAssetMenu(fileName = "TerrainDetailSettings", menuName = "Settings/Terrains/TerrainDetail", order = 1)]
    public class TerrainDetailSettings : ScriptableObject
    {
        [BoxGroup("Terrain Detail Settings")]
        [HideIf("terrainDetailEntries")]
        [Button("Create")]
        private void RefreshButton()
        {
            DetailPrototype[] allDetailProtoTypes = Terrain.activeTerrain.terrainData.detailPrototypes;
            int totalPrototypes = allDetailProtoTypes.Length;

            if (totalPrototypes != terrainDetailEntries.Count)
            {
                terrainDetailEntries = new List<TerrainDetailEntry>();
                foreach (DetailPrototype prototype in allDetailProtoTypes)
                {
                    TerrainDetailEntry newEntry = new TerrainDetailEntry();
                    newEntry.prototypeGameObject = prototype.prototype;
                    newEntry.maxHeight = 1.0f;
                    newEntry.minHeight = 0.9f;
                    newEntry.maxWidth = 1.0f;
                    newEntry.minWidth = 0.9f;

                    terrainDetailEntries.Add(newEntry);
                }
            }
        }
        [TableList(ShowIndexLabels = true)]
        public List<TerrainDetailEntry> terrainDetailEntries;
        
        [Serializable]
        public class TerrainDetailEntry
        {
            public GameObject prototypeGameObject;
            [TableColumnWidth(70, Resizable = false)]
            public float minHeight;
            [TableColumnWidth(70, Resizable = false)]
            public float maxHeight;
            [TableColumnWidth(70, Resizable = false)]
            public float minWidth;
            [TableColumnWidth(70, Resizable = false)]
            public float maxWidth;
        }
    }
}
