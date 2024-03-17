using System;
using System.Collections.Generic;
using UnityEngine;

namespace DaftAppleGames.Editor.ProjectSetup
{
    [CreateAssetMenu(fileName = "TagsProjectEditorSettings", menuName = "Settings/Project/TagsProjectEditor", order = 1)]
    public class TagsProjectEditorSettings : BaseProjectEditorSettings
    {
        [Header("Project Tags Settings")]
        public List<String> allTags;
    }
}