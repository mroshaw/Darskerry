namespace DaftAppleGames.Common.Weather
{
    /// <summary>
    /// Interface for providing details of snow to the Weather Manager
    /// </summary>
    public interface IWeatherSnowProvider
    {
        /// <summary>
        /// Return the current snow level for the provider
        /// </summary>
        /// <returns></returns>
        public float GetSnowLevel();
    }
}