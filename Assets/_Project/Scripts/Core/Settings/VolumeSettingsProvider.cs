using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Rendering;

namespace DaftAppleGames.Darskerry.Core.Settings
{
    public abstract class VolumeSettingsProvider : SettingsProvider
    {

        protected VolumeProfile SkyGlobalVolumeProfile;
        protected VolumeProfile LightingVolumeProfile;
        protected VolumeProfile CloudsVolumeProfile;
        protected VolumeProfile FogVolumeProfile;
        protected VolumeProfile PostProcessVolumeProfile;
        protected Light Sun;
        protected Light Moon;

        protected override void InitSettings()
        {
            SkyGlobalVolumeProfile = VolumeSettingsManager.Instance.SkyGlobalVolumeProfile;
            CloudsVolumeProfile = VolumeSettingsManager.Instance.CloudsVolumeProfile;
            FogVolumeProfile = VolumeSettingsManager.Instance.FogVolumeProfile;
            LightingVolumeProfile = VolumeSettingsManager.Instance.LightingVolumeProfile;
            PostProcessVolumeProfile = VolumeSettingsManager.Instance.PostProcessingVolumeProfile;
            Sun = VolumeSettingsManager.Instance.Sun;
            Moon = VolumeSettingsManager.Instance.Moon;
        }
    }
}