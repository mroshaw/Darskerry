using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.SceneManagement;

namespace DaftAppleGames.Common.GameControllers
{
    /// <summary>
    /// Scriptable Object: TODO Purpose and Summary
    /// </summary>
    [CreateAssetMenu(fileName = "AdditiveSceneLoaderSettings", menuName = "GameController/Additive Scene Loader Settings", order = 1)]
    public class AdditiveSceneLoaderSettings : ScriptableObject
    {
        [BoxGroup("General")] public string scenePath;
        [BoxGroup("Scenes")] [TableList] public List<AdditiveScene> additiveScenes;
        [BoxGroup("Scenes")] [TableList] public List<FixedScene> fixedScenes;
#if UNITY_EDITOR
        /// <summary>
        /// Get a list of Unity scenes from the Additive scenes
        /// </summary>
        /// <returns></returns>
        public List<Scene> GetScenes()
        {
            List<Scene> listOfScenes = new List<Scene>();
            foreach (AdditiveScene additiveScene in additiveScenes)
            {
                // listOfScenes.Add(additiveScene.sceneAsset);
            }

            return listOfScenes;
        }
#endif
        /// <summary>
        /// Returns the Game scene name
        /// </summary>
        /// <returns></returns>
        public string GetGameSceneName()
        {
            foreach (FixedScene scene in fixedScenes)
            {
                if (scene.sceneType == FixedSceneType.GameLoader)
                {
                    return scene.sceneName;
                }
            }

            return string.Empty;
        }
    }
}