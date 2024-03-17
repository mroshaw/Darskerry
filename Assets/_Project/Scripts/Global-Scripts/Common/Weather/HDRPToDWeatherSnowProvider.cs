#if HDRPTIMEOFDAY

namespace DaftAppleGames.Common.Weather
{
    /// <summary>
    /// Implements the snow provider GetRainLevel using PCWs HDRP Time of Time
    /// </summary>
    public class HDRTPToDWeatherSnowProvider : BaseWeatherSnowProvider, IWeatherSnowProvider
    {
        /// <summary>
        /// Return the current snow level
        /// </summary>
        /// <returns></returns>
        public override float GetSnowLevel()
        {
            return 0.0f;
        }
    }
}
#endif