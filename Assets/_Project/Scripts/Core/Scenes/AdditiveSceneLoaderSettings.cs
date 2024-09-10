using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace DaftAppleGames.Darskerry.Core.Scenes
{
    [CreateAssetMenu(fileName = "AdditiveSceneLoaderSettings", menuName = "Additive Scene Loader Settings", order = 1)]
    public class AdditiveSceneLoaderSettings : ScriptableObject
    {
        [BoxGroup("General")] public string scenePath;
        [BoxGroup("Scenes")] [TableList] public List<AdditiveScene> additiveScenes;
    }
}