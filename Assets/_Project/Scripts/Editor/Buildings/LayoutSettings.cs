using System;
using DaftAppleGames.Darskerry.Core.Buildings;
using UnityEngine;

namespace DaftAppleGames.Darskerry.Editor.BuildingTools
{
    [Serializable]
    internal class LayoutSettings
    {
        [SerializeField] internal string interiorLayer = "BuildingInterior";
        [SerializeField] internal string exteriorLayer = "BuildingExterior";
        [SerializeField] internal string interiorPropsLayer = "PropsInterior";
        [SerializeField] internal string exteriorPropsLayer = "PropsExterior";

        internal void ApplyToBuilding(Building building)
        {
            Apply(building.interiors, interiorLayer);
            Apply(building.exteriors, exteriorLayer);
            Apply(building.interiorProps, interiorPropsLayer);
            Apply(building.exteriorProps, exteriorPropsLayer);
        }

        private void Apply(GameObject[] itemGameObjects, string layer)
        {
            foreach (GameObject itemGameObject in itemGameObjects)
            {
                itemGameObject.layer = LayerMask.NameToLayer(layer);
                foreach (Transform childTransform in itemGameObject.GetComponentsInChildren<Transform>(true))
                {
                    childTransform.gameObject.layer = LayerMask.NameToLayer(layer);
                }
            }
        }
    }
}