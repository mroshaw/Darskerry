using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace DaftAppleGames.Editor.Common.Performance
{
    /// <summary>
    /// Scriptable Object to store Editor usable instances of the Player Character Configuration
    /// </summary>
    [CreateAssetMenu(fileName = "LodSettings", menuName = "Settings/Performance/LodTools", order = 1)]
    public class LodEditorSettings : EditorToolSettings
    {
        [BoxGroup("General Settings")]
        public List<LodGroupSetting> lodGroupSettings;
    }

    [Serializable]
    public class LodGroupSetting
    {
        public string[] lodSearchStrings;
        [Range(0, 1.0f)]
        public float lodRelativeHeight;
        [Range(0, 1.0f)]
        public float lodFadeTransitionWidth;
    }
}