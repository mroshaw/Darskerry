#if __MICROSPLAT__
using DaftAppleGames.Common.TimeAndWeather;
using Sirenix.OdinInspector.Editor;
using UnityEditor;

namespace DaftAppleGames.TimeAndWeather.Editor
{
    [CustomEditor(typeof(MicrosplatWeatherSync))]
    public class MicrosplatWeatherSyncEditor : OdinEditor
    {
        public override void OnInspectorGUI()
        {
#if !__MICROSPLAT_SNOW__
            EditorGUILayout.HelpBox("WARNING: MicroSplat Snow module is not installed. Snow sync will have no effect.", MessageType.Warning);
#endif
#if !__MICROSPLAT_STREAMS__
            EditorGUILayout.HelpBox("WARNING: MicroSplat Streams and Water module is not installed. Wetness sync will have no effect.", MessageType.Warning);
#endif
#if !__MICROSPLAT_WINDGLITTER__
            EditorGUILayout.HelpBox("WARNING: MicroSplat Wind and Glitter module is not installed. Wind sync will have no effect.", MessageType.Warning);
#endif

            DrawDefaultInspector();
        }
    }
}
#endif