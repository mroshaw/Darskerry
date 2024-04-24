using System.Collections;
using Expanse;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DaftAppleGames.Common.Weather
{
    public class ExpanseWeatherProvider : WeatherProviderBase
    {
        [BoxGroup("Settings")] public CloudLayerInterpolator expanseCloudLayerInterpolator;
        [BoxGroup("Settings")] public AtmosphereLayer[] expanseFogLayers;
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
    }
}