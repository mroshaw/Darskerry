using UnityEngine;
using UnityEditor;

namespace Assets._Project.Scripts.Editor.EditorTools
{
    /// <summary>
    /// Scriptable Object: TODO Purpose and Summary
    /// </summary>
    [CreateAssetMenu(fileName = "SceneLoaderSettings", menuName = "Daft Apple Games/Editor/Scene Loader Settings", order = 1)]
    public class SceneLoaderSettings : ScriptableObject
    {
        #region Properties
        [SerializeField] public SceneAsset mainMenuSceneAsset;
        [SerializeField] public SceneAsset gameSceneAsset;
        [SerializeField] public SceneAsset gameWorldSceneAsset;
        [SerializeField] public SceneAsset emptySceneAsset;
        #endregion
    }
}