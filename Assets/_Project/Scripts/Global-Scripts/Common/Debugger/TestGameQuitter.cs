using UnityEngine;

namespace DaftAppleGames.Common.Debugger
{
    /// <summary>
    /// Simple debug behaviour to allow us to quit the game when running debug scenes.
    /// </summary>
    public class TestGameQuitter : MonoBehaviour
    {
        [Header("Configuration")]
        public KeyCode quitKey = KeyCode.Escape;
        public KeyCode quitKeyModifier = KeyCode.LeftControl;

        /// <summary>
        /// Check for keypress then quit
        /// </summary>
        void Update()
        {
            if(Input.GetKeyDown(quitKey) && Input.GetKeyDown(quitKeyModifier))
            {
                Application.Quit();
            }
        }
    }
}
