using UnityEngine;

namespace DaftAppleGames.Common.GameControllers
{
    /// <summary>
    /// Helper class for the AdditiveSceneLoader singleton
    /// </summary>
    public class SceneLoaderMethods : MonoBehaviour
    {
        /// <summary>
        /// Proxy method to load main game scene
        /// </summary>
        public void LoadGameScene()
        {
            AdditiveSceneLoadManager.Instance.LoadGame();
        }

        /// <summary>
        /// Proxy method to load main menu scene
        /// </summary>
        public void LoadMainMenuScene()
        {
            AdditiveSceneLoadManager.Instance.LoadMainMenu();
        }

        /// <summary>
        /// Proxy method to restart loading the game
        /// </summary>
        public void RestartGame()
        {
            AdditiveSceneLoadManager.Instance.RestartGame();
        }
    }
}
