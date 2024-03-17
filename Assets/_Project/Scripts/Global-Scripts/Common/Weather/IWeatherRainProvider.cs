namespace DaftAppleGames.Common.Weather
{
    /// <summary>
    /// Interface for providing details of rain to the Weather Manager
    /// </summary>
    public interface IWeatherRainProvider
    {
        /// <summary>
        /// Return the current rain level for the provider
        /// </summary>
        /// <returns></returns>
        public float GetRainLevel();
    }
}