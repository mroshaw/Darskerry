#if AMPLIFY
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEditor;

using UnityEngine;

namespace DaftAppleGames.Editor.Common.Buildings
{
    public class ImposterEditorWindow : OdinEditorWindow
    {
        [MenuItem("Window/Buildings/Imposter Editor")]
        public static void ShowWindow()
        {
            GetWindow(typeof(ImposterEditorWindow));
        }
    
        // UI layout
        // UI layout
        [BoxGroup("Source Objects", centerLabel: true)]
        [Tooltip("Add prefabs to update")]
        [AssetsOnly]
        [DisableIf("applyToSceneSelection")]
        public List<GameObject> prefabs;
        [BoxGroup("Source Objects", centerLabel: true)]
        [Tooltip("Add scene Game Objects to update")]
        [SceneObjectsOnly]
        [BoxGroup("Source Objects", centerLabel: true)]
        [DisableIf("applyToSceneSelection")]
        public List<GameObject> sceneGameObjects;
        [BoxGroup("Source Objects", centerLabel: true)]
        public bool applyToSceneSelection = false;
        [BoxGroup("Imposter Settings", centerLabel: true)]
        public string imposterAssetPath = "/Assets/_Project/Models/Imposters/";
        [Button("Configure Imposter", ButtonSizes.Large), GUIColor(0, 1, 0)]
        private void ConfigureImposterButton()
        {
            foreach (GameObject gameObject in Selection.gameObjects)
            {
                ImposterTools.ConfigureImposter(gameObject, imposterAssetPath);
            }
        }
        
        [Button("Update LOD Group")]
        private void UpdateLodsButton()
        {
            foreach (GameObject gameObject in Selection.gameObjects)
            {
                ImposterTools.UpdateLodGroup(gameObject);
            }  
        }
        
        [Button("Update Imposter")]
        private void UpdateImposterButton()
        {
            foreach (GameObject gameObject in Selection.gameObjects)
            {
                ImposterTools.UpdateImposter(gameObject, imposterAssetPath);
            }  
        }
    }
}
#endif