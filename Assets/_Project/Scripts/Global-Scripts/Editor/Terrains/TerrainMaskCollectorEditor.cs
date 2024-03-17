using DaftAppleGames.Common.Terrains;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;

namespace DaftAppleGames.Editor.Terrains
{
    [CustomEditor(typeof(TerrainMaskCollector))]
    public class TerrainMaskCollectorEditor : OdinEditor
    {
        public TerrainMaskCollector terrainMaskCollector;

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            terrainMaskCollector = target as TerrainMaskCollector;
            if (GUILayout.Button("Refresh Collection"))
            {
                terrainMaskCollector.CollectMasks();
            }
        }
    }
}