using Sirenix.OdinInspector;
using UnityEngine;

namespace DaftAppleGames.Darskerry.Editor.BuildingTools
{
    [CreateAssetMenu(fileName = "BuildingEditorSetting", menuName = "Daft Apple Games/Building Editor Settings", order = 1)]
    public class BuildingEditorSettings : ScriptableObject
    {
        #region Properties

        [BoxGroup("Settings")] [SerializeField] internal LayoutSettings layoutSettings;
        [BoxGroup("Settings")] [SerializeField] internal ColliderSettings colliderSettings;
        [BoxGroup("Settings")] [SerializeField] internal LightSettings[] lightingSettings;
        [BoxGroup("Settings")] [SerializeField] internal BuildingMeshSettings meshSettings;
        [BoxGroup("Settings")] [SerializeField] internal BuildingVolumeSettings volumeSettings;
        [BoxGroup("Settings")] [SerializeField] internal DoorSettings doorSettings;

        #endregion
    }
}