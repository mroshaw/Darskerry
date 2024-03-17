#if UNITY_2021_2_OR_NEWER
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;

namespace DaftAppleGames.Editor.Lighting
{
    public class LightProbeGeneratorEditor : OdinEditorWindow
    {
        [BoxGroup("Settings")]
        [InlineEditor()]
        public LightProbeGeneratorSettings lightProbeGeneratorSettings;

        [MenuItem("Window/Lighting/Lighting Probe Editor")]
        private static void OpenWindow()
        {
            GetWindow(typeof(LightProbeGeneratorEditor));
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