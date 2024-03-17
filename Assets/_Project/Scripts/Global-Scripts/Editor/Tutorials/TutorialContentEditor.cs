using DaftAppleGames.Common.Ui;
using Sirenix.OdinInspector.Editor;
using UnityEditor;

namespace DaftAppleGames.Editor.Tutorials
{
    [CustomEditor(typeof(InfoPanelContent))]
    public class TutorialContentEditor : OdinEditor
    {
        public InfoPanelContent InfoPanelContent;
        
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            InfoPanelContent = target as InfoPanelContent;
        }
    }
}
