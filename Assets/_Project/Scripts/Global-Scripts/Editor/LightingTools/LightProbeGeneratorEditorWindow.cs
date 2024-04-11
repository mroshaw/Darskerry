#if UNITY_2021_2_OR_NEWER
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;

namespace DaftAppleGames.Editor.LightingTools
{
    public class LightProbeGeneratorEditorWindow : OdinEditorWindow
    {
        [BoxGroup("Settings")]
        [InlineEditor()]
        public LightProbeGeneratorSettings lightProbeGeneratorSettings;

        [MenuItem("Daft Apple Games/Tools/Lighting/Light probe generator")]
        private static void OpenWindow()
        {
            GetWindow(typeof(LightProbeGeneratorEditorWindow));
        }

        [Button("Generate")]
        [Tooltip("Generate lighting probes.")]
        private void GenerateClick()
        {
            LightProbeGenerator.GenerateLightProbes(lightProbeGeneratorSettings);
        }
    }
}
#endif