using Sirenix.OdinInspector;
using UnityEngine;

namespace DaftAppleGames.Editor.ProjectTools
{
    public class ProjectBaseEditorSettings : ScriptableObject
    {
        [BoxGroup("Basic Settings")]
        public string settingsName;
    }
}