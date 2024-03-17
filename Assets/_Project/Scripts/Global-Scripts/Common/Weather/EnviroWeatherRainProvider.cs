#if ENVIRO_3
using UnityEngine;
using Enviro;

namespace DaftAppleGames.Common.Weather
{
    /// <summary>
    /// Implements the rain provider GetRainLevel using Enviro 3
    /// </summary>
    public class EnviroWeatherRainProvider : BaseWeatherRainProvider, IWeatherRainProvider
    {
        /// <summary>
        /// Return the current rain level
        /// </summary>
        /// <returns></returns>
        public override float GetRainLevel()
        {
            return EnviroManager.instance.Environment.Settings.wetness;
        }
    }
}
#endif