using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

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
