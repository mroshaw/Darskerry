using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.Events;

namespace DaftAppleGames.Darskerry.Core.Spawning
{
    public class MaterialFader : MonoBehaviour
    {
        [BoxGroup("Spawn Settings")] [SerializeField] private bool fadeInOnStart = true;
        [BoxGroup("Spawn Settings")] [SerializeField] private float fadeTime = 10.0f;
        [BoxGroup("Material Settings")] [SerializeField] private List<MaterialMap> materialMap;
        [BoxGroup("Material Settings")] [SerializeField] private List<SwapData> swapData;
#if UNITY_EDITOR
        [SerializeField] private List<ShaderProperty> shaderProperties = new();
#endif
        [BoxGroup("Events")] public UnityEvent fadeInCompleteEvent;
        [BoxGroup("Events")] public UnityEvent fadeOutCompleteEvent;

        private static readonly int AlphaRemapMin = Shader.PropertyToID("_AlphaRemapMin");

        private void Start()
        {
            if (fadeInOnStart)
            {
                FadeIn();
            }
        }

        [Button("Fade In")]
        public void FadeIn()
        {
            SetFadeMaterials();
            StartCoroutine(FadeMaterialsAsync(0, 1, FadeInComplete));
        }

        [Button("Fade Out")]
        public void FadeOut()
        {
            SetFadeMaterials();
            StartCoroutine(FadeMaterialsAsync(1, 0, FadeOutComplete));
        }

        private void FadeInComplete()
        {
            SetOriginalMaterials();
            fadeInCompleteEvent.Invoke();
        }

        private void FadeOutComplete()
        {
            SetOriginalMaterials();
            fadeOutCompleteEvent.Invoke();
        }

        [Button("Original Materials")]
        private void SetOriginalMaterials()
        {
            foreach (SwapData currSwapData in swapData)
            {
                currSwapData.SetOriginalMaterial();
            }
        }

        [Button("Fade Materials")]
        private void SetFadeMaterials()
        {
            foreach (SwapData currSwapData in swapData)
            {
                currSwapData.SetFadeMaterial();
            }
        }

        private IEnumerator FadeMaterialsAsync(float start, float end, Action onComplete)
        {
            float time = 0;
            SetMaterialsAlpha(start);
            while (time < fadeTime)
            {
                SetMaterialsAlpha(Mathf.Lerp(start, end, time / fadeTime));
                time += Time.deltaTime;
                yield return null;
            }

            SetMaterialsAlpha(end);
            onComplete.Invoke();
        }

        private void SetMaterialsAlpha(float alpha)
        {
            foreach (MaterialMap matMap in materialMap)
            {
                matMap.fadeMaterial.SetFloat(AlphaRemapMin, alpha);
            }
        }

        [Serializable]
        private struct MaterialMap
        {
            public Material origMaterial;
            public Material fadeMaterial;

            public MaterialMap(Material newOrigMat)
            {
                origMaterial = newOrigMat;
                fadeMaterial = null;
            }
        }

        [Serializable]
        private class SwapData
        {
            public SkinnedMeshRenderer renderer;
            public List<MaterialMap> materialMap;

            public SwapData(SkinnedMeshRenderer newRenderer)
            {
                renderer = newRenderer;
                materialMap = new List<MaterialMap>();
            }

            internal Material GetFadeMaterial(Material lookupMaterial)
            {
                foreach (MaterialMap currMaterialMap in materialMap)
                {
                    if (currMaterialMap.origMaterial == lookupMaterial)
                    {
                        return currMaterialMap.fadeMaterial;
                    }
                }

                return null;
            }

            internal void SetOriginalMaterial()
            {
                Material[] newMaterials = new Material[renderer.materials.Length];

                for (int matIndex = 0; matIndex < renderer.materials.Length; matIndex++)
                {
                    // Debug.Log($"Render {renderer.gameObject.name} set Material {matIndex} from {renderer.materials[matIndex].name} to {materialMap[matIndex].origMaterial.name}");
                    newMaterials[matIndex] = materialMap[matIndex].origMaterial;
                }

                renderer.materials = newMaterials;
            }

            internal void SetFadeMaterial()
            {
                Material[] newMaterials = new Material[renderer.materials.Length];

                for (int matIndex = 0; matIndex < renderer.materials.Length; matIndex++)
                {
                    // Debug.Log($"Render {renderer.gameObject.name} set Material {matIndex} from {renderer.materials[matIndex].name} to {materialMap[matIndex].fadeMaterial.name}");
                    newMaterials[matIndex] = materialMap[matIndex].fadeMaterial;
                }
                renderer.materials = newMaterials;
            }

            internal void SwapMaterials()
            {
                for (int matIndex = 0; matIndex < renderer.materials.Length; matIndex++)
                {
                    renderer.sharedMaterials[matIndex] = renderer.sharedMaterials[matIndex] == materialMap[matIndex].origMaterial ? materialMap[matIndex].fadeMaterial : materialMap[matIndex].origMaterial;
                }
            }
        }
        #region Editor methods
        #if UNITY_EDITOR
        [Button("Create Material Map")]
        private void CreateMaterialMap()
        {
            materialMap = new List<MaterialMap>();
            List<Material> materials = new List<Material>();
            SkinnedMeshRenderer[] allRenderers = GetComponentsInChildren<SkinnedMeshRenderer>(true);
            foreach (SkinnedMeshRenderer meshRenderer in allRenderers)
            {
                foreach (Material material in meshRenderer.sharedMaterials)
                {
                    if (!materials.Contains(material))
                    {
                        materials.Add(material);
                    }
                }
            }

            foreach (Material material in materials)
            {
                materialMap.Add(new MaterialMap(material));
            }
        }

        [Button("Create Swap Data")]
        private void CreateSwapData()
        {
            swapData = new List <SwapData>();
            SkinnedMeshRenderer[] allRenderers = GetComponentsInChildren<SkinnedMeshRenderer>(true);
            foreach (SkinnedMeshRenderer meshRenderer in allRenderers)
            {
                SwapData newSwapData = new(meshRenderer);
                foreach (Material mat in meshRenderer.sharedMaterials)
                {
                    MaterialMap newMatMap = new(mat)
                    {
                        fadeMaterial = GetFadeMaterial(mat)
                    };
                    newSwapData.materialMap.Add(newMatMap);
                }
                swapData.Add(newSwapData);
            }
        }

        private Material GetFadeMaterial(Material origMaterial)
        {
            foreach (MaterialMap matMap in materialMap)
            {
                if (matMap.origMaterial == origMaterial)
                {
                    return matMap.fadeMaterial;
                }
            }
            return null;
        }


        internal enum ShaderPropertyType
        {
            Color,
            Vector,
            Float,
            Range,
            Texture,
        }

        [Serializable]
        internal struct ShaderProperty
        {
            public string name;
            public ShaderPropertyType type;
        }

        [Button("Get Shader Properties")]
        public void Populate()
        {
            shaderProperties.Clear();

            Shader shader = materialMap[0].fadeMaterial.shader;
            int count = UnityEditor.ShaderUtil.GetPropertyCount(shader);
            for (int i = 0; i < count; i++)
            {
                ShaderProperty shaderProperty = new ShaderProperty()
                {
                    name = UnityEditor.ShaderUtil.GetPropertyName(shader, i),
                    type = (ShaderPropertyType)UnityEditor.ShaderUtil
                        .GetPropertyType(shader, i)
                };

                shaderProperties.Add(shaderProperty);
            }
        }

#endif
        #endregion
    }
}