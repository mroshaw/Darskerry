using System.Linq;
using DaftAppleGames.Darskerry.Core.Extensions;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AI;

namespace DaftAppleGames.Darskerry.Core.Buildings
{
    public class MeshTest : MonoBehaviour
    {
        [SerializeField] internal LayerMask meshLayerMask;
        [SerializeField] internal string[] meshIgnoreObjects;

        [SerializeField] internal bool createBox;
        [SerializeField] internal Material boxMaterial;
        [SerializeField] internal Vector3 sizeDebug;
        [SerializeField] internal Vector3 centerDebug;
        [SerializeField] internal GameObject boxDebug;

        [Button("Calculate Bounds")]
        private void CalculateBounds()
        {
            Vector3 size = GetMeshBoundsSize(gameObject, meshLayerMask, meshIgnoreObjects);
            Vector3 center = GetMeshBoundsCenter(gameObject, meshLayerMask, meshIgnoreObjects);

            sizeDebug = size;
            centerDebug = center;

            if (createBox)
            {
                DestroyImmediate(boxDebug);
                boxDebug = GameObject.CreatePrimitive(PrimitiveType.Cube);
                boxDebug.GetComponent<MeshRenderer>().material = boxMaterial;
                boxDebug.transform.SetParent(gameObject.transform);
                boxDebug.transform.localPosition = gameObject.transform.InverseTransformPoint(center);
                boxDebug.transform.localScale = size;
            }
        }

        private static Bounds GetMeshBounds(GameObject mainGameObject, LayerMask includeLayerMask,
            string[] excludeGameObjects)
        {
            Bounds combinedBounds = new(Vector3.zero, Vector3.zero);
            bool hasValidRenderer = false;

            foreach (MeshRenderer childRenderer in mainGameObject.GetComponentsInChildren<MeshRenderer>(true))
            {
                if ((includeLayerMask & (1 << childRenderer.gameObject.layer)) != 0 &&
                    (excludeGameObjects.Length == 0 || !excludeGameObjects.ItemInString(childRenderer.gameObject.name)))
                {
                    Bounds meshBounds = childRenderer.bounds;

                    // Initialize or expand the combined bounds
                    if (!hasValidRenderer)
                    {
                        combinedBounds = meshBounds;
                        hasValidRenderer = true;
                    }
                    else
                    {
                        combinedBounds.Encapsulate(meshBounds);
                    }
                }
            }

            return combinedBounds;
        }

        public static Vector3 GetMeshBoundsSize(GameObject mainGameObject, LayerMask includeLayerMask,
            string[] excludeGameObjects)
        {
            return GetMeshBounds(mainGameObject, includeLayerMask, excludeGameObjects).size;
        }

        public static Vector3 GetMeshBoundsCenter(GameObject mainGameObject, LayerMask includeLayerMask,
            string[] excludeGameObjects)
        {
            return GetMeshBounds(mainGameObject, includeLayerMask, excludeGameObjects).center;
        }
    }
}