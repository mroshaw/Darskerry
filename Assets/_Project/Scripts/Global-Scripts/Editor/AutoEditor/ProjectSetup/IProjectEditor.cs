using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DaftAppleGames.Editor.ProjectSetup
{
    public interface IProjectEditor
    {
        public abstract void LoadSettings();
        public abstract void RunEditorConfiguration();
    }
}
