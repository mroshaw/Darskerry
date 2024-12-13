using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Rendering;

namespace DaftAppleGames.Darskerry.Core.Settings
{
    public abstract class VolumeSettingsProvider : SettingsProvider
    {

        protected VolumeProfile SkyGlobalVolumeProfile;
        protected VolumeProfile LightingVolumeProfile;
        protected VolumeProfile PostProcessVolumeProfile;
        protected Light Sun;
        protected Light Moon;

        protected override void InitSettings()
        {
            SkyGlobalVolumeProfile = VolumeSettingsManager.Instance.SkyGlobalVolumeProfile;
            LightingVolumeProfile = VolumeSettingsManager.Instance.LightingVolumeProfile;
            PostProcessVolumeProfile = VolumeSettingsManager.Instance.PostProcessingVolumeProfile;
            Sun = VolumeSettingsManager.Instance.Sun;
            Moon = VolumeSettingsManager.Instance.Moon;
        }
    }
}