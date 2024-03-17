using System;
using System.Collections.Generic;
using UnityEngine;

namespace DaftAppleGames.Editor.ProjectSetup
{
    [CreateAssetMenu(fileName = "LayersProjectEditorSettings", menuName = "Settings/Project/LayersProjectEditor", order = 1)]
    public class LayersProjectEditorSettings : BaseProjectEditorSettings
    {
        [Header("Project Layers Settings")]
        public List<String> allLayers;
    }
}