using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace DaftAppleGames.Common.UI
{
    /// <summary>
    /// Static class to allow scene name to pass between scenes
    /// </summary>
    public class LoadingData
    {
        public static string SceneNameToLoad;
    }

    public class SceneLoader : MonoBehaviour
    {
        [Header("UI Config")]
        public Slider progressSlider;

        /// <summary>
        /// Run the loading async process
        /// </summary>
        private void Start()
        {
            StartCoroutine(LoadSceneAsync());
        }

        /// <summary>
        /// ASync method to increase image fill while scene is loading
        /// </summary>
        /// <returns></returns>
        IEnumerator LoadSceneAsync()
        {
            if (LoadingData.SceneNameToLoad != "")
            {
                AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(LoadingData.SceneNameToLoad);

                // Stop next scene from loading
                asyncOperation.allowSceneActivation = false;

                while (!asyncOperation.isDone)
                {
                    progressSlider.value = asyncOperation.progress;

                    if (asyncOperation.progress >= 0.9f)
                    {
                        asyncOperation.allowSceneActivation = true;
                    }
                    yield return null;
                }
            }
        }
    }
}