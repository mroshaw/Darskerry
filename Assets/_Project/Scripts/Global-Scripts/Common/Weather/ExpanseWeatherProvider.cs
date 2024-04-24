using System.Collections;
using Expanse;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DaftAppleGames.Common.Weather
{
    public enum FogSetting { VisibiltyDistance, Radius, Thickness }
    public class ExpanseWeatherProvider : WeatherProviderBase
    {
        [BoxGroup("Settings")] public CloudLayerInterpolator expanseCloudLayerInterpolator;
        [BoxGroup("Settings")] public CreativeFog expanseCreativeFog;
        [BoxGroup("Settings")] public AtmosphereLayer[] expanseAtmosphereLayers;
        [BoxGroup("Settings")] public ExpanseWeatherPresetSettings defaultPreset;

        /// <summary>
        /// Expanse implementation of Start Weather
        /// </summary>
        /// <param name="weatherPreset"></param>
        /// <param name="transitionDuration"></param>
        public override void ApplyWeatherPresetProvider(WeatherPresetSettingsBase weatherPreset, float transitionDuration)
        {
            if (!(weatherPreset is ExpanseWeatherPresetSettings expanseWeatherPreset))
            {
                return;
            }

            expanseCloudLayerInterpolator.m_transitionTime = transitionDuration;

            // Check if this is first weather, if so load default to give something to extrapolate from
            if (expanseCloudLayerInterpolator.m_currentPreset == null)
            {
                expanseCloudLayerInterpolator.LoadPreset(defaultPreset.expanseCloudLayer);
            }

            // Interpolate to the target settings
            StartCoroutine(ApplyWeatherPresetAsync(expanseWeatherPreset));

            // Apply fog settings
            ApplyFogSetting(FogSetting.VisibiltyDistance, expanseWeatherPreset);
            ApplyFogSetting(FogSetting.Radius, expanseWeatherPreset);
            ApplyFogSetting(FogSetting.Thickness, expanseWeatherPreset);
        }

        /// <summary>
        /// Waits until any current interpolation is complete, then triggers a transition to the new preset
        /// </summary>
        /// <param name="expansePreset"></param>
        /// <returns></returns>
        private IEnumerator ApplyWeatherPresetAsync(ExpanseWeatherPresetSettings expansePreset)
        {
            yield return new WaitForSeconds(0.5f);
            while (expanseCloudLayerInterpolator.IsInterpolating())
            {
                yield return null;
            }
            expanseCloudLayerInterpolator.LoadPreset(expansePreset.expanseCloudLayer);
        }

        public void ApplyFogSetting(FogSetting fogSetting, ExpanseWeatherPresetSettings expansePreset)
        {
            StartCoroutine(ApplyFogSettingAsync(fogSetting, expansePreset));
        }

        private IEnumerator ApplyFogSettingAsync(FogSetting fogSetting, ExpanseWeatherPresetSettings expansePreset)
        {
            float time = 0;

            float startValue = 0;
            float endValue = 0;
            switch (fogSetting)
            {
                case FogSetting.Radius:
                    startValue = expanseCreativeFog.m_radius;
                    endValue = expansePreset.fogRadius;
                    break;
                case FogSetting.Thickness:
                    startValue = expanseCreativeFog.m_thickness;
                    endValue = expansePreset.fogThickness;
                    break;
                case FogSetting.VisibiltyDistance:
                    startValue = expanseCreativeFog.m_visibilityDistance;
                    endValue = expansePreset.fogVisibilityDistance;
                    break;
            }

            while (time < expansePreset.fogInterpolateDuration)
            {
                switch (fogSetting)
                {
                    case FogSetting.Radius:
                        expanseCreativeFog.m_radius = Mathf.Lerp(startValue, endValue, time / expansePreset.fogInterpolateDuration);
                        break;
                    case FogSetting.Thickness:
                        expanseCreativeFog.m_thickness = Mathf.Lerp(startValue, endValue, time / expansePreset.fogInterpolateDuration);
                        break;
                    case FogSetting.VisibiltyDistance:
                        expanseCreativeFog.m_visibilityDistance = Mathf.Lerp(startValue, endValue, time / expansePreset.fogInterpolateDuration);
                        break;
                }

                time += Time.deltaTime;
                yield return null;
            }
        }
    }
}