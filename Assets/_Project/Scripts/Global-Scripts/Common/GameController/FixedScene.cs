using System;
using Sirenix.OdinInspector;

namespace DaftAppleGames.Common.GameControllers
{
    [Serializable]
    public class FixedScene
    {
        [BoxGroup("General")][TableColumnWidth(150, Resizable = true)] public string scenePath;
        [BoxGroup("General")][TableColumnWidth(150, Resizable = true)] public string sceneName;
        [BoxGroup("General")][TableColumnWidth(150, Resizable = true)] public FixedSceneType sceneType;
    }
}
