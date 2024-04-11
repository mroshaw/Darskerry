using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DaftAppleGames.Editor.ProjectTools
{
    [CreateAssetMenu(fileName = "TagsProjectEditorSettings", menuName = "Daft Apple Games/Project/Project tags setup", order = 1)]
    public class ProjectTagsEditorSettings : ProjectBaseEditorSettings
    {
        [BoxGroup("Project Tags Settings")] public List<String> allTags;
    }
}