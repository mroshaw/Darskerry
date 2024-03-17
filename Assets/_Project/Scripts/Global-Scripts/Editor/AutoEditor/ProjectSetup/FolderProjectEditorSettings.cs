using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DaftAppleGames.Editor.ProjectSetup
{
    [CreateAssetMenu(fileName = "FolderProjectEditorSettings", menuName = "Settings/Project/FolderProjectEditor", order = 1)]
    public class FolderProjectEditorSettings : BaseProjectEditorSettings
    {
        [Header("Project Foldert Settings")]
        public List<ProjectFolder> allProjectFolders;
    }
}