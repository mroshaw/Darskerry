using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace DaftAppleGames.Darskerry.Core.UserInterface.PauseGame
{
    /// <summary>
    /// Implementation of the Pause Game functionality
    /// </summary>
    public class PauseGameManager : MonoBehaviour
    {
        [BoxGroup("Events")] public UnityEvent pausedEvent;
        [BoxGroup("Events")] public UnityEvent unPausedEvent;

        public bool IsPaused { get; private set; }

        private bool _isCursorVisible;
        private CursorLockMode _cursorLockMode;

        private void Start()
        {
            UnPauseGame();
        }

        public void TogglePauseGame()
        {
            if(IsPaused)
            {
                UnPauseGame();
            }
            else
            {
                PauseGame();
            }
        }

        public void PauseGame()
        {
            _isCursorVisible = Cursor.visible;
            _cursorLockMode = Cursor.lockState;
            
            IsPaused = true;
            Time.timeScale = 0.0f;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            pausedEvent.Invoke();
        }

        public void UnPauseGame()
        {
            IsPaused = false;
            Time.timeScale = 1.0f;
            Cursor.visible = _isCursorVisible;
            Cursor.lockState = _cursorLockMode;

            unPausedEvent.Invoke();
        }

        public void ReturnToMainMenu()
        {

        }

        public void ExitToDesktop()
        {
            Application.Quit();
        }
    }
}