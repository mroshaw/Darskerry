#if HNS
using SickscoreGames.HUDNavigationSystem;
using UnityEngine;

namespace DaftAppleGames.Common.Spawning
{
    public class HudNavSpawnHelper : MonoBehaviour
    {

        private HUDNavigationSystem _hudNavSystem;
        
        /// <summary>
        /// Sets the Player transform on the HUD Nav System
        /// </summary>
        /// <param name="playerGameObject"></param>
        public void SetPlayer(GameObject playerGameObject)
        {
            if (_hudNavSystem == null)
            {
                _hudNavSystem = GetComponentInChildren<HUDNavigationSystem>(true);
            }
            _hudNavSystem.PlayerController = playerGameObject.transform;
        }
    }
}
#endif