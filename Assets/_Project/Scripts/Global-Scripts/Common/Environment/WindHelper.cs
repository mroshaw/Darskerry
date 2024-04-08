#if THE_VEGETATION_ENGINE
using UnityEngine;

namespace DaftAppleGames.Common.Environment
{
    /// <summary>
    /// Helper component to manage TVE wind
    /// </summary>
    public class WindHelper : MonoBehaviour
    {
        private float _windSpeed;
        /// <summary>
        /// Stops TVE wind from blowing
        /// </summary>
        public void StopWind()
        {
            if (TheVegetationEngine.TVEManager.Instance == null)
            {
                return;
            }
            _windSpeed = TheVegetationEngine.TVEManager.Instance.globalMotion.windPower; 
            TheVegetationEngine.TVEManager.Instance.globalMotion.windPower = 0.0f;
        }

        /// <summary>
        /// Restarts TVE wind blowing
        /// </summary>
        public void RestartWind()
        {
            if (TheVegetationEngine.TVEManager.Instance == null)
            {
                return;
            }
            TheVegetationEngine.TVEManager.Instance.globalMotion.windPower = _windSpeed;
        }

        /// <summary>
        /// Sets the TVE wind speed
        /// </summary>
        /// <param name="speed"></param>
        public void SetWindSpeed(float speed)
        {
            if (TheVegetationEngine.TVEManager.Instance == null)
            {
                return;
            }
            TheVegetationEngine.TVEManager.Instance.globalMotion.windPower = speed;
        }
    }
}
#endif