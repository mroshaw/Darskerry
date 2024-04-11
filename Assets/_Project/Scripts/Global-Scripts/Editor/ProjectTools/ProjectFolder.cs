using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DaftAppleGames.Editor.ProjectTools
{
    [Serializable]
    public class ProjectFolder
    {
        public string FolderPath;
        public string FolderName;

        public ProjectFolder(string folderPath, string folderName)
        {
            FolderPath = folderPath;
            FolderName = folderName;
        }
    }

}