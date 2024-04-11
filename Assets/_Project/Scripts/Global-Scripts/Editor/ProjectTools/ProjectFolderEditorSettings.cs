using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DaftAppleGames.Editor.ProjectTools
{
    [CreateAssetMenu(fileName = "FolderProjectEditorSettings", menuName = "Daft Apple Games/Project/Project folder settings", order = 1)]
    public class FolderProjectEditorSettings : ProjectBaseEditorSettings
    {
        [BoxGroup("Project Folder Settings")] public List<ProjectFolder> allProjectFolders;
    }
}