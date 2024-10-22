using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

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
            IsPaused = true;
            Time.timeScale = 0.0f;
            pausedEvent.Invoke();
        }

        public void UnPauseGame()
        {
            IsPaused = false;
            Time.timeScale = 1.0f;
            unPausedEvent.Invoke();
        }

        public void ReturnToMainMenu()
        {
            SceneManager.LoadScene("MainMenuLoaderScene");
        }

        public void ExitToDesktop()
        {
            Application.Quit();
        }
    }
}