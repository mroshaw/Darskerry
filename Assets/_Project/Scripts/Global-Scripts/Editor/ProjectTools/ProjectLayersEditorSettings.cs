using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DaftAppleGames.Editor.ProjectTools
{
    [CreateAssetMenu(fileName = "LayersProjectEditorSettings", menuName = "Daft Apple Games/Project/Project layer settings", order = 1)]
    public class LayersProjectEditorSettings : ProjectBaseEditorSettings
    {
        [BoxGroup("Project Layers Settings")] public List<String> allLayers;
    }
}