using DaftAppleGames.Darskerry.TimeAndWeather;
using Sirenix.OdinInspector.Editor;
using UnityEditor;

namespace DaftAppleGames.TimeAndWeather.Editor
{
    /// This class is required in order to force a refresh of the [ShowInInspector] properties that are
    /// shown in the TimeAndWeatherManager inspector. While this isn't great for performance, this is not
    /// included nor executed in a build.
    [CustomEditor(typeof(TimeAndWeatherManager))]
    #if ODIN_INSPECTOR
    public class TimeAndWeatherManagerEditor : OdinEditor
    #else
        public class TimeAndWeatherManagerEditor : UnityEditor.Editor
    #endif
    {
        public override void OnInspectorGUI()
        {
            TimeAndWeatherManager timeAndWeatherManager = target as TimeAndWeatherManager;

            if (timeAndWeatherManager != null && timeAndWeatherManager.timeProvider == null)
            {
                EditorGUILayout.HelpBox(
                    "No time provider selected! Create an instance of a Time Provider in the hierarchy and drag into this component.",
                    MessageType.Error);
            }

            if (timeAndWeatherManager != null && timeAndWeatherManager.weatherProvider == null)
            {
                EditorGUILayout.HelpBox(
                    "No weather provider selected! Create an instance of a Weather Provider in the hierarchy and drag into this component.",
                    MessageType.Error);
            }

            DrawDefaultInspector();
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