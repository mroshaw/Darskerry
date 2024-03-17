/*
using System.Collections.Generic;
using DaftAppleGames.Common.Audio;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;

namespace DaftAppleGames.Editor.Audio
{
    public class UiSoundManagerEditorWindow : OdinEditorWindow
    {
        // Display Editor Window
        [MenuItem("Window/Audio/UI Sound Manager Editor")]
        public static void ShowWindow()
        {
            GetWindow(typeof(UiSoundManagerEditorWindow));
        }

        [BoxGroup("Source Objects", centerLabel: true)]
        [Tooltip("Add prefabs to update")]
        [AssetsOnly]
        public List<GameObject> prefabs;
        
        [BoxGroup("UI Sound Settings", centerLabel: true)]
        [AssetsOnly]
        public UiSoundSettings soundSettings;

        [Button("Apply in Prefabs")]
        private void ApplyAllPrefabsButton()
        {
            foreach (GameObject prefab in prefabs)
            {
                UiSoundManager[] allUiSound = prefab.GetComponentsInChildren<UiSoundManager>(true);
                foreach (UiSoundManager uiSound in allUiSound)
                {
                    uiSound.soundSettings = soundSettings;
                    EditorTools.ForcePrefabSave();
                    Debug.Log($"UiSoundManager: Updated settings on {prefab.name} in {uiSound.gameObject.name}");
                }
            }
        }
    }
}
*/