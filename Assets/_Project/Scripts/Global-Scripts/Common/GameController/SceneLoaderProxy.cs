using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DaftAppleGames.Common.GameControllers
{
    public class SceneLoaderProxy : MonoBehaviour
    {
        [BoxGroup("Settings")] public string sceneToLoad;

        /// <summary>
        /// Load the scene
        /// </summary>
        private void Start()
        {
            SceneManager.LoadScene(sceneToLoad);
        }
    }
}