using System;
using DaftAppleGames.Darskerry.Core.Buildings;
using DaftAppleGames.Extensions;
using DaftAppleGames.Editor.Mesh;
using JBooth.MicroVerseCore;
using UnityEngine;
using UnityEngine.Rendering;

namespace DaftAppleGames.Darskerry.Editor.BuildingTools
{
    [Serializable]
    internal class BuildingVolumeSettings
    {
        [SerializeField] internal VolumeSettings apvVolumeSettings;
        [SerializeField] internal VolumeSettings interiorVolumeSettings;
        [SerializeField] internal VolumeProfile interiorProfile;
        [SerializeField] internal VolumeSettings microVerseClearVolumeSettings;
        [SerializeField] internal VolumeSettings microVerseHeightVolumeSettings;


        internal void ApplyToBuilding(Building building)
        {
            ConfigureAdaptiveProbeVolume(building.gameObject);
            ConfigureInteriorVolume(building.gameObject);
            ConfigureMicroVerseClear(building.gameObject);
            ConfigureMicroVerseHeight(building.gameObject);
        }

        private void ConfigureAdaptiveProbeVolume(GameObject buildingGameObject)
        {
            GameObject apvGameObject = apvVolumeSettings.ConfigureGameObject(buildingGameObject);
            ProbeVolume apvProbe = apvGameObject.EnsureComponent<ProbeVolume>();
            apvProbe.size = MeshSettings.GetMeshBoundsSize(buildingGameObject, apvVolumeSettings.meshLayerMask, apvVolumeSettings.meshIgnoreObjects);
            Vector3 meshCenter = MeshSettings.GetMeshBoundsCenter(buildingGameObject, apvVolumeSettings.meshLayerMask, apvVolumeSettings.meshIgnoreObjects);
            apvGameObject.transform.localPosition = buildingGameObject.transform.InverseTransformPoint(meshCenter);
        }

        private void ConfigureInteriorVolume(GameObject buildingGameObject)
        {
            GameObject interiorGameObject = interiorVolumeSettings.ConfigureGameObject(buildingGameObject);
            BoxCollider boxCollider = apvVolumeSettings.ConfigureBoxCollider(buildingGameObject, interiorGameObject);
            Volume interiorVolume = interiorGameObject.EnsureComponent<Volume>();
            interiorVolume.sharedProfile = interiorProfile;
            interiorVolume.isGlobal = false;
        }

        private void ConfigureMicroVerseClear(GameObject buildingGameObject)
        {
            GameObject microVerseClearGameObject = GetMicroVerseGameObject(buildingGameObject, microVerseClearVolumeSettings);

            ClearStamp clearStamp = microVerseClearGameObject.EnsureComponent<ClearStamp>();
            clearStamp.filterSet.falloffFilter.filterType = FalloffFilter.FilterType.Box;

            ConfigureMicroVerseGameObject(buildingGameObject, microVerseClearGameObject, microVerseClearVolumeSettings);
        }

        private void ConfigureMicroVerseHeight(GameObject buildingGameObject)
        {
            GameObject microVerseHeightGameObject = GetMicroVerseGameObject(buildingGameObject, microVerseHeightVolumeSettings);

            HeightStamp heightStamp = microVerseHeightGameObject.EnsureComponent<HeightStamp>();
            heightStamp.falloff.filterType = FalloffFilter.FilterType.Box;
            heightStamp.mode = HeightStamp.CombineMode.Min;
            heightStamp.falloff.falloffRange = new Vector2(0f, 0f);

            ConfigureMicroVerseGameObject(buildingGameObject, microVerseHeightGameObject, microVerseHeightVolumeSettings);
        }

        private GameObject GetMicroVerseGameObject(GameObject buildingGameObject, VolumeSettings volumeSettings)
        {
            GameObject microGameObject = volumeSettings.ConfigureGameObject(buildingGameObject);
            return microGameObject;
        }

        private void ConfigureMicroVerseGameObject(GameObject buildingGameObject, GameObject microVerseGameObject, VolumeSettings volumeSettings)
        {
            Vector3 center =
                MeshSettings.GetMeshBoundsCenter(buildingGameObject, volumeSettings.meshLayerMask, volumeSettings.meshIgnoreObjects);
            Vector3 size = MeshSettings.GetMeshBoundsSize(buildingGameObject, volumeSettings.meshLayerMask, volumeSettings.meshIgnoreObjects);

            Vector3 localPosition = buildingGameObject.transform.InverseTransformPoint(center);
            localPosition.y = 0;
            localPosition += volumeSettings.positionOffset;

            microVerseGameObject.transform.localPosition = localPosition;
            microVerseGameObject.transform.localScale = size + volumeSettings.sizeMargin;
        }
    }
}