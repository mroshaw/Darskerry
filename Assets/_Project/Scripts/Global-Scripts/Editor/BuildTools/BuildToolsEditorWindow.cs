using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEditor;

namespace DaftAppleGames.Editor.Buildings
{
    public class BuildToolsEditorWindow : OdinEditorWindow
    {
        // Display Editor Window
        [MenuItem("Window/Build/Build Tools")]
        public static void ShowWindow()
        {
            GetWindow(typeof(BuildToolsEditorWindow));
        }

        [Button("DO EVERYTHING", ButtonSizes.Large), GUIColor(0, 1, 0)]
        private void DoEveryThingButton()
        {
            BakeLighting();
            BakeNavMesh();
            BakeOcclusion();
        }
        [Button("Bake Lighting")]
        private void BakeLightsButton()
        {
            BakeLighting();
        }
        [Button("Bake NavMesh")]
        private void BakeNavmeshButton()
        {
            BakeNavMesh();
        }

        [Button("Bake Occlusion")]
        private void BakeOcclusionButton()
        {
            BakeOcclusion();
        }

        private void BakeLighting()
        {
            BuildTools.BakeLighting();
        }

        private void BakeNavMesh()
        {
            BuildTools.BakeNavMesh();
        }

        private void BakeOcclusion()
        {
            BuildTools.BakeOcclusion();
        }
    }
}
