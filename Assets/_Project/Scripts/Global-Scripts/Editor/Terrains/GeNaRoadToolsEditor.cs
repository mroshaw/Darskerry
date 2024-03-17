#if GENA_PRO
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;

namespace DaftAppleGames.Editor
{
    [CustomEditor(typeof(GeNaRoadTools))]
    public class GeNaRoadToolsEditor : OdinEditor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            GeNaRoadTools genaTools = target as GeNaRoadTools;
            if (GUILayout.Button("Apply Roads"))
            {
                genaTools.ApplyRoads();
            }
        }
    }
}
#endif