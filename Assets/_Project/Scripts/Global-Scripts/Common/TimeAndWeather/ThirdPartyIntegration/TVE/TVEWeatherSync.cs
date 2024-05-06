#if THE_VEGETATION_ENGINE
using DaftAppleGames.Common.TimeAndWeather.Weather;
using TheVegetationEngine;

namespace DaftAppleGames.Common.TimeAndWeather
{
    /// <summary>
    /// Provides core Weather / Sky / Cloud functionality for use across all providers.
    /// </summary>
    public class TVEWeatherSync : WeatherSyncBase
    {
        /// <summary>
        /// TVE implementation of wind
        /// </summary>
        protected override float Wind
        {
            get => TVEManager.Instance.globalMotion.windPower;
            set => TVEManager.Instance.globalMotion.windPower = value;
        }

        /// <summary>
        /// TVE implementation of wetness
        /// </summary>
        protected override float Wetness
        {
            get => TVEManager.Instance.globalControl.globalWetness;
            set => TVEManager.Instance.globalControl.globalWetness = value;
        }

        /// <summary>
        /// TVE implementation of overlay
        /// </summary>
        protected override float Overlay
        {
            get => TVEManager.Instance.globalControl.globalOverlay;
            set => TVEManager.Instance.globalControl.globalOverlay = value;
        }
    }
}
#endif