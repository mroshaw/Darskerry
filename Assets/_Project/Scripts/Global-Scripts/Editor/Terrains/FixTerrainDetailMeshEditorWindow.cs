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
    public class FixTerrainDetailMeshEditorWindow : OdinEditorWindow
    {
        [MenuItem("Window/Terrain/Terrain Detail Mesh Fix Editor")]
        public static void ShowWindow()
        {
            GetWindow(typeof(FixTerrainDetailMeshEditorWindow));
        }

        [Button("Fix Selected")]
        private void FixSelectedObject()
        {

            foreach (MeshRenderer l_MeshRenderer in Selection.gameObjects[0].GetComponentsInChildren<MeshRenderer>())
            {
                MeshFilter l_MeshFilter = l_MeshRenderer.GetComponent<MeshFilter>();
                if (l_MeshFilter != null && l_MeshFilter.sharedMesh != null)
                {
                    Mesh l_Mesh = l_MeshFilter.sharedMesh;
                    if (l_Mesh.colors != null)
                    {
                        Debug.Log($"Fixing: {l_Mesh.name}");
                        Color[] l_Colors = l_Mesh.colors;
                        for (int i = 0; i < l_Colors.Length; i = i + 1)
                        {
                            Color l_Color = l_Colors[i];
                            l_Color.a = l_Color.r;
                            l_Color.r = 1.0f;
                            l_Color.g = 1.0f;
                            l_Color.b = 1.0f;
                            l_Colors[i] = l_Color;
                        }
                        l_Mesh.colors = l_Colors;
                    }
                }
            }
        }
    }
}