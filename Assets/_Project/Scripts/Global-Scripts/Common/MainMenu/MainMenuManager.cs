using DaftAppleGames.Common.GameControllers;
using UnityEngine;

namespace DaftAppleGames.Common.MainMenu
{
    public class MainMenuManager : MonoBehaviour
    {
        /// <summary>
        /// Initialise setting controllers
        /// </summary>
        private void Start()
        {
            Time.timeScale = 1.0f;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        
        /// <summary>
        /// Start a new game
        /// </summary>
        public void StartNewGame()
        {
            // sceneLoadManager.LoadScene(GameScenes.MasterGame);
            AdditiveSceneLoadManager.Instance.LoadGame();
        }

        /// <summary>
        /// Exit to Desktop
        /// </summary>
        public void ExitToDesktop()
        {
            Application.Quit();
        }

        /// <summary>
        /// Set the Selected Character as Emily
        /// </summary>
        public void SetSelectedCharEmily()
        {
            GameController.Instance.SelectedCharacter = CharSelection.Emily;
        }

        /// <summary>
        /// Set the Selected Character as Callum
        /// </summary>
        public void SetSelectedCharCallum()
        {
            GameController.Instance.SelectedCharacter = CharSelection.Callum;
        }
    }
}
