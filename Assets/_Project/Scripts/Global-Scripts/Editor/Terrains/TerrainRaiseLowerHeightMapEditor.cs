//--------------------------------//
//  TerrainRaiseLowerHeightmap.cs //
//  Written by Jay Kay            //
//  2016/4/8                      //
//--------------------------------//
using DaftAppleGames.Common.Terrains;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;

namespace DaftAppleGames.Editor
{
    [CustomEditor(typeof(TerrainRaiseLowerHeightMap))]
    public class TerrainRaiseLowerHeightmapEditor : OdinEditor
    {
        private GameObject obj;
        private TerrainRaiseLowerHeightMap objScript;

        protected override void OnEnable()
        {
            obj = Selection.activeGameObject;
            objScript = obj.GetComponent<TerrainRaiseLowerHeightMap>();
        }

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            // spacing between buttons
            EditorGUILayout.Space();

            // check if there is a terrain
            if (objScript.terrain == null)
            {
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("Assign a terrain to modify", GUILayout.MinWidth(80), GUILayout.MaxWidth(350));
                EditorGUILayout.EndHorizontal();

                return;
            }

            // raise/lower button
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Raise/Lower Terrain", GUILayout.MinWidth(80), GUILayout.MaxWidth(350)))
            {
                objScript.ModifyTerrainHeightmap();
            }
            EditorGUILayout.EndHorizontal();

            // check if there is an undo array
            if (objScript.undoHeights.Count > 0)
            {
                // spacing between buttons
                EditorGUILayout.Space();

                EditorGUILayout.BeginHorizontal();
                if (GUILayout.Button("UNDO", GUILayout.MinWidth(80), GUILayout.MaxWidth(350)))
                {
                    objScript.UndoModifyTerrainHeightmap();
                }
                EditorGUILayout.EndHorizontal();
            }
        }
    }
}