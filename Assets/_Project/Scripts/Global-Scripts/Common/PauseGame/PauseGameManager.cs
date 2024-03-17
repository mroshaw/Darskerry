using DaftAppleGames.Common.GameControllers;
using UnityEngine;

namespace DaftAppleGames.Common.PauseGame
{
    /// <summary>
    /// Implementation of the Pause Game functionality
    /// </summary>
    public class PauseGameManager : MonoBehaviour
    {
        [Header("Debug")]
        public bool isPaused = false;

        private bool _isCursorVisible;
        private CursorLockMode _cursorLockMode;

        /// <summary>
        /// Toggle the current pause state
        /// </summary>
        /// <returns></returns>
        public bool TogglePauseGame()
        {
            if(isPaused)
            {
                UnPauseGame();
            }
            else
            {
                PauseGame();
            }
            return isPaused;
        }
        
        /// <summary>
        /// Pause the game
        /// </summary>
        public void PauseGame()
        {
            _isCursorVisible = Cursor.visible;
            _cursorLockMode = Cursor.lockState;
            
            isPaused = true;
            Time.timeScale = 0.0f;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        /// <summary>
        /// Unpause the game
        /// </summary>
        public void UnPauseGame()
        {
            isPaused = false;
            Time.timeScale = 1.0f;
            Cursor.visible = _isCursorVisible;
            Cursor.lockState = _cursorLockMode;
        }

        /// <summary>
        /// Return to the Main Menu scene
        /// </summary>
        public void ReturnToMainMenu()
        {
            AdditiveSceneLoadManager.Instance.LoadMainMenu();
        }
        
        /// <summary>
        /// Exit To Desktop
        /// </summary>
        public void ExitToDesktop()
        {
            Application.Quit();
        }
    }
}