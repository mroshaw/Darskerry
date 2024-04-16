using System;
using Sirenix.OdinInspector;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace DaftAppleGames.Common.GameControllers
{
    [Serializable]
    public class AdditiveScene
    {
        [TableColumnWidth(120, Resizable = true)] public string sceneName;
#if UNITY_EDITOR
        [TableColumnWidth(120, Resizable = false)] public SceneAsset sceneAsset;
#endif
        [Tooltip("Is this is the main scene in the list?")][TableColumnWidth(90, Resizable = false)] public bool isMainScene;
        [Tooltip("Load when button is pressed in editor mode")][TableColumnWidth(70, Resizable = false)] public bool edit;
        [Tooltip("Load when running game from build")][TableColumnWidth(70, Resizable = false)] public bool inGame;
        [Tooltip("Load when running game in editor")][TableColumnWidth(70, Resizable = false)] public bool inEditor;
        
        [Button("Load")]
        private void Load()
        {
            Debug.Log($"Loading: {sceneName}");
        }

        private AsyncOperation _sceneOp;
        private SceneLoadStatus _loadStatus = SceneLoadStatus.None;

        public AsyncOperation SceneOp
        {
            get => _sceneOp;
            set => _sceneOp = value;
        }

        public SceneLoadStatus LoadStatus
        {
            get => _loadStatus;
            set => _loadStatus = value;
        }
    }
}