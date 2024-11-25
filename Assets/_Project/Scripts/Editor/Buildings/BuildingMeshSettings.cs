using System;
using DaftAppleGames.Darskerry.Core.Buildings;
using DaftAppleGames.Editor.ObjectTools;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DaftAppleGames.Darskerry.Editor.BuildingTools
{
    [Serializable]
    internal class BuildingMeshSettings
    {
        [BoxGroup("Building")] [SerializeField] internal MeshSettings interiorMeshSettings;
        [BoxGroup("Building")] [SerializeField] internal MeshSettings exteriorMeshSettings;
        [BoxGroup("Building")] [SerializeField] internal MeshSettings interiorPropMeshSettings;
        [BoxGroup("Building")] [SerializeField] internal MeshSettings exteriorPropMeshSettings;

        public void ApplyToBuilding(Building building)
        {
            exteriorMeshSettings.Apply(building.exteriors);
            exteriorPropMeshSettings.Apply(building.exteriorProps);
            interiorMeshSettings.Apply(building.interiors);
            interiorPropMeshSettings.Apply(building.interiorProps);
        }
    }
}