#if ENVIRO_3
using UnityEngine;
using Enviro;

namespace DaftAppleGames.Common.Weather
{
    /// <summary>
    /// Implements the snow provider GetRainLevel using Enviro 3
    /// </summary>
    public class EnviroWeatherSnowProvider : BaseWeatherSnowProvider, IWeatherSnowProvider
    {
        /// <summary>
        /// Return the current snow level
        /// </summary>
        /// <returns></returns>
        public override float GetSnowLevel()
        {
            return EnviroManager.instance.Environment.Settings.snow;
        }
    }
}
#endif