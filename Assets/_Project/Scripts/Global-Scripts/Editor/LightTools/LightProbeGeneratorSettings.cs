using UnityEngine;

namespace DaftAppleGames.Editor.Lighting
{
    public enum ProcessingMode { Simple, Advanced }
    public enum LightProbeGenerateMode { Trees, TreesAdvanced, MeshRenderers, Volumes, All }

    [CreateAssetMenu(fileName = "LightProbeEditorSettings", menuName = "Settings/Lighting/LightProbeEditor", order = 1)]
    public class LightProbeGeneratorSettings : ScriptableObject
    {
        public LightProbeGenerateMode m_generateMode = LightProbeGenerateMode.All;
        public ProcessingMode m_processingMode = ProcessingMode.Simple;
        public bool m_addHeightOffset = true;
        public float m_heightOffset = 0.5f;
        public float m_sphereOffset = 2.5f;
        public bool m_spawnProbeUnderDetaial = true;

        public void CopySettings(LightProbeGeneratorSettings settings)
        {
            m_generateMode = settings.m_generateMode;
            m_processingMode = settings.m_processingMode;
            m_addHeightOffset = settings.m_addHeightOffset;
            m_heightOffset = settings.m_heightOffset;
            m_sphereOffset = settings.m_sphereOffset;
        }
    }
}