#if HDRPTIMEOFDAY

namespace DaftAppleGames.Common.Weather
{
    /// <summary>
    /// Implements the rain provider GetRainLevel using PCWs HDRP Time of Time
    /// </summary>
    public class HDRTPToDWeatherRainProvider : BaseWeatherRainProvider, IWeatherRainProvider
    {
        /// <summary>
        /// Return the current rain level
        /// </summary>
        /// <returns></returns>
        public override float GetRainLevel()
        {
            return 0.0f;
        }
    }
}
#endif