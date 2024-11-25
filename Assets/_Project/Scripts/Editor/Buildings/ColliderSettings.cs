using System;
using DaftAppleGames.Darskerry.Core.Buildings;
using DaftAppleGames.Darskerry.Core.Extensions;
using UnityEngine;

namespace DaftAppleGames.Darskerry.Editor.BuildingTools
{
    [Serializable]
    internal class ColliderSettings
    {
        [SerializeField] internal string[] boxObjectNames;
        [SerializeField] internal string[] capsuleObjectNames;

        internal void ApplyToBuilding(Building building)
        {
            foreach (Transform childTransform in building.GetComponentsInChildren<Transform>())
            {
                string itemName = childTransform.gameObject.name;

                if (boxObjectNames.ItemInString(itemName))
                {
                    Apply(childTransform.gameObject, typeof(BoxCollider));
                }

                if (capsuleObjectNames.ItemInString(itemName))
                {
                    Apply(childTransform.gameObject, typeof(CapsuleCollider));
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
        }

        private void ConfigureBoxCollider(BoxCollider boxCollider)
        {
        }

        private void ConfigureCapsuleCollider(CapsuleCollider capsuleCollider)
        {
        }
    }
}