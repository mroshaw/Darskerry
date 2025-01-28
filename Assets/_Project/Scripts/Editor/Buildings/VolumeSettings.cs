using System;
using DaftAppleGames.Extensions;
using DaftAppleGames.Editor.Mesh;
using UnityEngine;

namespace DaftAppleGames.Darskerry.Editor.BuildingTools
{
    [Serializable]
    internal class VolumeSettings
    {
        [SerializeField] internal LayerMask meshLayerMask;
        [SerializeField] internal string[] meshIgnoreObjects;
        [SerializeField] internal string gameObjectName;
        [SerializeField] internal bool centerPositionY = true;
        [SerializeField] internal Vector3 sizeMargin;
        [SerializeField] internal Vector3 positionOffset;

        internal GameObject ConfigureGameObject(GameObject parentGameObject)
        {
            GameObject volumeGameObject = parentGameObject.FindChildGameObject(gameObjectName);
            if (!volumeGameObject)
            {
                volumeGameObject = new GameObject(gameObjectName);
            }

            volumeGameObject.transform.SetParent(parentGameObject.transform);
            volumeGameObject.transform.localPosition = Vector3.zero;
            volumeGameObject.transform.localRotation = Quaternion.identity;
            volumeGameObject.transform.localScale = Vector3.one;

            return volumeGameObject;
        }

        internal BoxCollider ConfigureBoxCollider(GameObject meshGameObject, GameObject volumeGameObject)
        {
            Vector3 meshSize = MeshSettings.GetMeshBoundsSize(meshGameObject, meshLayerMask, meshIgnoreObjects);
            Vector3 meshCenter = MeshSettings.GetMeshBoundsCenter(meshGameObject, meshLayerMask, meshIgnoreObjects);
            BoxCollider boxCollider = volumeGameObject.EnsureComponent<BoxCollider>();
            boxCollider.isTrigger = true;
            boxCollider.size = meshSize + sizeMargin;

            if (centerPositionY)
            {
                boxCollider.center = meshGameObject.transform.InverseTransformPoint(meshCenter);
            }

            return boxCollider;
        }
    }
}