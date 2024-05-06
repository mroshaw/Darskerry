#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using Sirenix.OdinInspector;
#endif
using DaftAppleGames.Common.TimeAndWeather.Weather;
using UnityEngine;

namespace DaftAppleGames.Common.TimeAndWeather.TimeOfDay
{
    /// <summary>
    /// Scriptable object override for Expanse time of day settings
    /// </summary>
    [CreateAssetMenu(fileName = "TimeOfDayPresetSettings", menuName = "Daft Apple Games/Weather/Expanse Time Of Day preset settings", order = 1)]
    public class ExpanseTimeOfDayPresetSettings : TimeOfDayPresetSettingsBase
    {
        // Public serializable properties
        [BoxGroup("Sky Settings")] public ExpanseWeatherPresetSettings expanseWeatherSettings;
        [BoxGroup("Sky Settings")] public float expanseWeatherTransitionDuration = 5.0f;
    }
}