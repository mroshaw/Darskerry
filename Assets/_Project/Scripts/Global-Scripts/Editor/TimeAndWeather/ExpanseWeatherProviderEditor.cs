#if ODIN_INSPECTOR
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using DaftAppleGames.Common.TimeAndWeather;
using DaftAppleGames.Common.TimeAndWeather.Weather;

namespace DaftAppleGames.TimeAndWeather.Editor
{
    /// This class is required in order to force a refresh of the [ShowInInspector] properties that are
    /// shown in the TimeAndWeatherManager inspector. While this isn't great for performance, this is not
    /// included nor executed in a build.
    [CustomEditor(typeof(ExpanseWeatherProvider))]
    #if ODIN_INSPECTOR
    public class ExpanseWeatherProviderEditor : OdinEditor
    #else
        public class TimeAndWeatherManagerEditor : UnityEditor.Editor
    #endif
    {
        public override void OnInspectorGUI()
        {
            ExpanseWeatherProvider timeAndWeatherManager = target as ExpanseWeatherProvider;

            if (timeAndWeatherManager && !timeAndWeatherManager.expanseCloudLayerInterpolator)
            {
                EditorGUILayout.HelpBox(
                    "No Cloud Layer Interpolator selected! Drag one in from the Expanse game object in your hierarchy.",
                    MessageType.Error);
            }

            if (timeAndWeatherManager && !timeAndWeatherManager.expanseCreativeFog)
            {
                EditorGUILayout.HelpBox(
                    "No Creative Fog component selected! Drag one in from the Expanse game object in your hierarchy.",
                    MessageType.Error);
            }

            if (timeAndWeatherManager && !timeAndWeatherManager.interpolatorPrimerPreset)
            {
                EditorGUILayout.HelpBox(
                    "No Primer Preset selected! Pick a weather preset that will act as the 'primer' for the the initial cloud interpolation.",
                    MessageType.Error);
            }

#if ODIN_INSPECTOR
            DrawDefaultInspector();
#else
            base.OnInspectorGUI();
#endif
        }

        /// <summary>
        /// This forces the OnInspectorGUI to run each frame, refreshing the Inspector
        /// </summary>
        /// <returns></returns>
        public override bool RequiresConstantRepaint()
        {
            return true;
        }
    }
}
#endif