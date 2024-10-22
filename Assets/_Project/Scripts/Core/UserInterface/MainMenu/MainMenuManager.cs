using UnityEngine;
using UnityEngine.SceneManagement;

namespace DaftAppleGames.Darskerry.Core.UserInterface.MainMenu
{
    /// <summary>
    /// Implementation of the Pause Game functionality
    /// </summary>
    public class MainMenuManager : MonoBehaviour
    {

        private bool _isCursorVisible;
        private CursorLockMode _cursorLockMode;

        public void StartNewGame()
        {
            SceneManager.LoadScene("GameLoaderScene");
        }
        public void ExitToDesktop()
        {
            Application.Quit();
        }
    }
}