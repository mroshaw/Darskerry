using DaftAppleGames.Common.GameControllers;
using UnityEngine;

namespace DaftAppleGames.Common.MainMenu
{
    public class CharSelectManager : MonoBehaviour
    {
        private SceneLoaderMethods _sceneLoader;
        
        #region UNITY_EVENTS
        /// <summary>
        /// Configure the component on start
        /// </summary>
        private void Start()
        {
            _sceneLoader = GetComponent<SceneLoaderMethods>();
        }
        #endregion

        /// <summary>
        /// Select the Male character
        /// </summary>
        public void SelectMaleCharacter()
        {
            GameController.Instance.SelectedCharacter = CharSelection.Callum;
        }

        /// <summary>
        /// Select the Female character
        /// </summary>
        public void SelectFemaleCharacter()
        {
            GameController.Instance.SelectedCharacter = CharSelection.Emily;
        }
    }
}
