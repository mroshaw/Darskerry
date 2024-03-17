#if GENA_PRO
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;

namespace DaftAppleGames.Editor
{
    [CustomEditor(typeof(GaiaMeshExport))]
    public class GaiaMeshExportEditor : OdinEditor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            GaiaMeshExport meshExport = target as GaiaMeshExport;
            if (GUILayout.Button("Export Mesh"))
            {
                meshExport.ExportMesh();
            }
        }
    }
}
#endif