using SickscoreGames.HUDNavigationSystem;
using UnityEngine;
using Sirenix.OdinInspector;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
#endif
namespace DaftAppleGames.Common.Ui
{
    public class NavElementConfiguration : MonoBehaviour
    {
#if UNITY_EDITOR
        [BoxGroup("UI Settings")] public Sprite mainIcon;
        [BoxGroup("UI Settings")] public Sprite upArrowIcon;
        [BoxGroup("UI Settings")] public Sprite downArrowIcon;
        [BoxGroup("UI Settings")] public Sprite pointerArrowIcon;

        private HUDNavigationElement _hudNavElement;

        [Button("Configure")]
        private void Configure()
        {
            _hudNavElement = GetComponent<HUDNavigationElement>();

            if (!mainIcon || !upArrowIcon || !downArrowIcon)
            {
                return;
            }

            ConfigureRadar(_hudNavElement.Prefabs.RadarPrefab);
            ConfigureCompass(_hudNavElement.Prefabs.CompassBarPrefab);
            ConfigureIndicator(_hudNavElement.Prefabs.IndicatorPrefab);
            ConfigureMiniMap(_hudNavElement.Prefabs.MinimapPrefab);
            ConfigureMainMap(_hudNavElement.Prefabs.MainMapPrefab);
            UpdatePrefab();
        }

        /// <summary>
        /// Configure the radar prefab
        /// </summary>
        /// <param name="radarPrefab"></param>
        private void ConfigureRadar(HNSRadarPrefab radarPrefab)
        {
            GameObject radarGameObject = PrefabUtility.GetNearestPrefabInstanceRoot(radarPrefab.gameObject);
            string assetPath = AssetDatabase.GetAssetPath(radarGameObject);
            Debug.Log(assetPath);

            HNSRadarPrefab prefabComponent = radarGameObject.GetComponent<HNSRadarPrefab>();
            prefabComponent.Icon.sprite = mainIcon;
            prefabComponent.ArrowAbove.sprite = upArrowIcon;
            prefabComponent.ArrowBelow.sprite = downArrowIcon;

            PrefabUtility.SaveAsPrefabAsset(radarGameObject, assetPath);
        }

        /// <summary>
        /// Configure the compass prefab
        /// </summary>
        /// <param name="compassPrefab"></param>
        private void ConfigureCompass(HNSCompassBarPrefab compassPrefab)
        {
            GameObject compassGameObject = PrefabUtility.GetNearestPrefabInstanceRoot(compassPrefab.gameObject);
            string assetPath = AssetDatabase.GetAssetPath(compassGameObject);
            Debug.Log(assetPath);

            HNSCompassBarPrefab prefabComponent = compassGameObject.GetComponent<HNSCompassBarPrefab>();
            prefabComponent.Icon.sprite = mainIcon;

            PrefabUtility.SaveAsPrefabAsset(compassGameObject, assetPath);
        }

        /// <summary>
        /// Configure the indicator prefab
        /// </summary>
        /// <param name="indicatorPrefab"></param>
        private void ConfigureIndicator(HNSIndicatorPrefab indicatorPrefab)
        {
            GameObject indicatorGameObject = PrefabUtility.GetNearestPrefabInstanceRoot(indicatorPrefab.gameObject);
            string assetPath = AssetDatabase.GetAssetPath(indicatorGameObject);
            Debug.Log(assetPath);

            HNSIndicatorPrefab prefabComponent = indicatorGameObject.GetComponent<HNSIndicatorPrefab>();
            prefabComponent.OnscreenIcon.sprite = mainIcon;
            prefabComponent.OffscreenIcon.sprite = mainIcon;
            prefabComponent.OffscreenArrowIcon.sprite = pointerArrowIcon;

            PrefabUtility.SaveAsPrefabAsset(indicatorGameObject, assetPath);
        }

        /// <summary>
        /// Configure the MiniMap prefab
        /// </summary>
        private void ConfigureMiniMap(HNSMinimapPrefab minimapPrefab)
        {
            GameObject minimapGameObject = PrefabUtility.GetNearestPrefabInstanceRoot(minimapPrefab.gameObject);
            string assetPath = AssetDatabase.GetAssetPath(minimapGameObject);
            Debug.Log(assetPath);

            HNSMinimapPrefab prefabComponent = minimapGameObject.GetComponent<HNSMinimapPrefab>();
            prefabComponent.Icon.sprite = mainIcon;
            prefabComponent.ArrowAbove.sprite = upArrowIcon;
            prefabComponent.ArrowBelow.sprite = downArrowIcon;

            PrefabUtility.SaveAsPrefabAsset(minimapGameObject, assetPath);
        }

        /// <summary>
        /// Configure the MainMap prefab
        /// </summary>
        private void ConfigureMainMap(HNSMainMapPrefab mainmapPrefab)
        {
            GameObject mainMapGameObject = PrefabUtility.GetNearestPrefabInstanceRoot(mainmapPrefab.gameObject);
            string assetPath = AssetDatabase.GetAssetPath(mainMapGameObject);
            Debug.Log(assetPath);

            HNSMainMapPrefab prefabComponent = mainMapGameObject.GetComponent<HNSMainMapPrefab>();
            prefabComponent.Icon.sprite = mainIcon;
            prefabComponent.ArrowAbove.sprite = upArrowIcon;
            prefabComponent.ArrowBelow.sprite = downArrowIcon;

            PrefabUtility.SaveAsPrefabAsset(mainMapGameObject, assetPath);
        }

        /// <summary>
        /// Mark prefab as dirty to force save
        /// </summary>
        private void UpdatePrefab()
        {
            PrefabStage prefabStage = PrefabStageUtility.GetCurrentPrefabStage();
            if (prefabStage != null)
            {
                EditorSceneManager.MarkSceneDirty(prefabStage.scene);
            }
        }
#endif
    }
}
