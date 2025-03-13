using System;
using DaftAppleGames.Darskerry.Core.Buildings;
using DaftAppleGames.Extensions;
using UnityEditor;
using UnityEngine;

namespace DaftAppleGames.Darskerry.Editor.BuildingTools
{
    [Serializable]
    internal class ColliderSettings
    {
        [SerializeField] internal string[] boxObjectNames;
        [SerializeField] internal string[] capsuleObjectNames;
        [SerializeField] internal string[] meshObjectNames;

        internal void ApplyToBuilding(Building building)
        {
            foreach (MeshRenderer renderer in building.GetComponentsInChildren<MeshRenderer>(true))
            {
                string itemName = renderer.gameObject.name;

                if (boxObjectNames.ItemInString(itemName))
                {
                    Apply(renderer.gameObject, typeof(BoxCollider));
                }

                if (capsuleObjectNames.ItemInString(itemName))
                {
                    Apply(renderer.gameObject, typeof(CapsuleCollider));
                }

                if (meshObjectNames.ItemInString(itemName))
                {
                    Apply(renderer.gameObject, typeof(MeshCollider));
                }
            }
        }

        private void Apply(GameObject itemGameObject, Type colliderType)
        {
            // Box Colliders
            if (colliderType == typeof(BoxCollider))
            {
                BoxCollider boxCollider = itemGameObject.EnsureComponent<BoxCollider>();
                ConfigureBoxCollider(boxCollider);
            }

            // Capsule Colliders
            if (colliderType == typeof(CapsuleCollider))
            {
                CapsuleCollider capsuleCollider = itemGameObject.EnsureComponent<CapsuleCollider>();
                ConfigureCapsuleCollider(capsuleCollider);
            }

            if (colliderType == typeof(MeshCollider))
            {
                MeshCollider meshCollider = itemGameObject.EnsureComponent<MeshCollider>();
                ConfigureMeshCollider(meshCollider);
            }

            EditorUtility.SetDirty(itemGameObject);
            PrefabUtility.RecordPrefabInstancePropertyModifications(itemGameObject);
        }

        private void ConfigureBoxCollider(BoxCollider boxCollider)
        {
        }

        private void ConfigureCapsuleCollider(CapsuleCollider capsuleCollider)
        {
        }

        private void ConfigureMeshCollider(MeshCollider capsuleCollider)
        {
        }
    }
}