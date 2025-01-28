using System;
using DaftAppleGames.Extensions;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using RenderingLayerMask = UnityEngine.Rendering.HighDefinition.RenderingLayerMask;

namespace DaftAppleGames.Editor.Mesh
{
    [Serializable]
    internal class MeshSettings
    {
        [BoxGroup("Lighting")] [SerializeField] internal bool isGIEnabled = true;
        [BoxGroup("Lighting")] [SerializeField] internal ReceiveGI receiveGlobalIllumination = ReceiveGI.LightProbes;
        [BoxGroup("Shadows")] [SerializeField] internal bool isStaticShadowCaster = true;
        [BoxGroup("Shadows")] [SerializeField] internal ShadowCastingMode shadowCastingMode = ShadowCastingMode.Off;
        [BoxGroup("Light Layers")] [SerializeField] internal RenderingLayerMask lightLayerMask;
        [BoxGroup("Additional")] [SerializeField] internal MotionVectorGenerationMode motionVectorMode = MotionVectorGenerationMode.Camera;
        [BoxGroup("Additional")] [SerializeField] internal bool allowDynamicOcclusion = false;


        internal void Apply(GameObject[] itemGameObjects)
        {
            foreach (GameObject itemGameObject in itemGameObjects)
            {
                foreach (MeshRenderer renderer in itemGameObject.GetComponentsInChildren<MeshRenderer>(true))
                {
                    renderer.shadowCastingMode = shadowCastingMode;
                    renderer.staticShadowCaster = isStaticShadowCaster;
                    StaticEditorFlags flags = GameObjectUtility.GetStaticEditorFlags(renderer.gameObject);
                    StaticEditorFlags newFlags;
                    if (isGIEnabled)
                    {
                        newFlags = flags | StaticEditorFlags.ContributeGI;
                    }
                    else
                    {
                        newFlags = flags & ~StaticEditorFlags.ContributeGI;
                    }

                    GameObjectUtility.SetStaticEditorFlags(renderer.gameObject, newFlags);
                    renderer.receiveGI = receiveGlobalIllumination;
                    renderer.renderingLayerMask = (uint)lightLayerMask;
                }
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

        public static Vector3 GetMeshBoundsSize(GameObject mainGameObject)
        {
            LayerMask everything = ~0;
            string[] excludeGameObjects = Array.Empty<string>();
            return GetMeshBounds(mainGameObject, everything, excludeGameObjects).size;
        }

        public static Vector3 GetMeshBoundsSize(GameObject mainGameObject, LayerMask includeLayerMask,
            string[] excludeGameObjects)
        {
            return GetMeshBounds(mainGameObject, includeLayerMask, excludeGameObjects).size;
        }

        public static Vector3 GetMeshBoundsCenter(GameObject mainGameObject)
        {
            LayerMask everything = ~0;
            string[] excludeGameObjects = Array.Empty<string>();
            return GetMeshBounds(mainGameObject, everything, excludeGameObjects).center;
        }

        public static Vector3 GetMeshBoundsCenter(GameObject mainGameObject, LayerMask includeLayerMask,
            string[] excludeGameObjects)
        {
            return GetMeshBounds(mainGameObject, includeLayerMask, excludeGameObjects).center;
        }
    }
}