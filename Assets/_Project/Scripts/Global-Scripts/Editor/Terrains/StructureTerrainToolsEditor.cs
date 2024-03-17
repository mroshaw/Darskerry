#if GENA_PRO
using DaftAppleGames.Common.Terrains;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;

namespace DaftAppleGames.Editor.Terrains
{
    [CustomEditor(typeof(StructureTerrainTools))]
    public class StructureTerrainToolsEditor : OdinEditor
    {
        private StructureTerrainTools _terrainTools;

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            _terrainTools = target as StructureTerrainTools;

            if (GUILayout.Button("Apply All"))
            {
                _terrainTools.ApplyDecorators();
            }
        }
    }
}
#endif