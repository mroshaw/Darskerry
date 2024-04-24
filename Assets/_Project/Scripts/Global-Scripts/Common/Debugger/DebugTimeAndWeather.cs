using System.Collections.Generic;
using DaftAppleGames.Common.Weather;

namespace DaftAppleGames.Common.Debugger
{
    public class DebugTimeAndWeather : DebugBase
    {
        /// <summary>
        /// Get the current time of day preset names
        /// </summary>
        /// <returns></returns>
        public List<string> GetTimePresetNames()
        {
            return TimeAndWeatherManager.Instance.GetTimePresetNames();
        }

        /// <summary>
        /// Get the current time of day preset names
        /// </summary>
        /// <returns></returns>
        public List<string> GetWeatherPresetNames()
        {
            return TimeAndWeatherManager.Instance.GetWeatherPresetNames();
        }

        /// <summary>
        /// Set the time of day
        /// </summary>
        /// <param name="presetName"></param>
        /// <param name="transitionDuration"></param>
        public void SetTimeOfDay(string presetName, float transitionDuration)
        {
            TimeAndWeatherManager.Instance.ApplyTimePreset(presetName, transitionDuration);
        }

        /// <summary>
        /// Set the weather preset
        /// </summary>
        /// <param name="presetName"></param>
        /// <param name="transitionDuration"></param>
        public void SetWeather(string presetName, float transitionDuration)
        {
            TimeAndWeatherManager.Instance.ApplyWeatherPreset(presetName, transitionDuration);
        }
    }
}