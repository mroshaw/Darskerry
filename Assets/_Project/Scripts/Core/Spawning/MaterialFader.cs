using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEditor.TerrainTools;
using UnityEngine.Events;

namespace DaftAppleGames.Darskerry.Core.Spawning
{
    public class MaterialFader : MonoBehaviour
    {
        private static readonly int AlphaRemapMin = Shader.PropertyToID("_AlphaRemapMin");
        [BoxGroup("Spawn Settings")] [SerializeField] private bool fadeInOnStart = true;
        [BoxGroup("Spawn Settings")] [SerializeField] private float fadeTime = 10.0f;
        [BoxGroup("Material Settings")] [SerializeField] private List<MaterialMap> materialMap;
        [BoxGroup("Material Settings")] [SerializeField] private List<SwapData> swapData;
        [SerializeField] public List<ShaderProperty> _properties = new List<ShaderProperty>();
        [BoxGroup("Events")] public UnityEvent FadeInCompleteEvent;
        [BoxGroup("Events")] public UnityEvent FadeOutCompleteEvent;

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
            FadeMaterials();
            StartCoroutine(FadeMaterials(0, 1, FadeInComplete));
        }

        [Button("Fade Out")]
        public void FadeOut()
        {
            FadeMaterials();
            StartCoroutine(FadeMaterials(1, 0, FadeOutComplete));
        }

        private void FadeInComplete()
        {
            OriginalMaterials();
        }

        private void FadeOutComplete()
        {
            OriginalMaterials();
        }

        private IEnumerator FadeMaterials(float start, float end, Action onComplete)
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

        [Button("Original Materials")]
        public void OriginalMaterials()
        {
            foreach (SwapData currSwapData in swapData)
            {
                currSwapData.SetOriginalMaterial();
            }
        }

        [Button("Fade Materials")]
        public void FadeMaterials()
        {
            foreach (SwapData currSwapData in swapData)
            {
                currSwapData.SetFadeMaterial();
            }
        }



        [Button("Swap Materials")]
        public void SwapMaterials()
        {
            foreach (SwapData currSwapData in swapData)
            {
                currSwapData.SwapMaterials();
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

        [Serializable]
        public struct MaterialMap
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
        public class SwapData
        {
            public SkinnedMeshRenderer renderer;
            public List<MaterialMap> materialMap;

            public SwapData(SkinnedMeshRenderer newRenderer)
            {
                renderer = newRenderer;
                materialMap = new List<MaterialMap>();
            }

            public Material GetFadeMaterial(Material lookupMaterial)
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

            public void SetOriginalMaterial()
            {
                Material[] newMaterials = new Material[renderer.materials.Length];

                for (int matIndex = 0; matIndex < renderer.materials.Length; matIndex++)
                {
                    Debug.Log($"Render {renderer.gameObject.name} set Material {matIndex} from {renderer.materials[matIndex].name} to {materialMap[matIndex].origMaterial.name}");
                    newMaterials[matIndex] = materialMap[matIndex].origMaterial;
                }

                renderer.materials = newMaterials;
            }

            public void SetFadeMaterial()
            {
                Material[] newMaterials = new Material[renderer.materials.Length];

                for (int matIndex = 0; matIndex < renderer.materials.Length; matIndex++)
                {
                    Debug.Log($"Render {renderer.gameObject.name} set Material {matIndex} from {renderer.materials[matIndex].name} to {materialMap[matIndex].fadeMaterial.name}");
                    newMaterials[matIndex] = materialMap[matIndex].fadeMaterial;
                }
                renderer.materials = newMaterials;
            }

            public void SwapMaterials()
            {
                for (int matIndex = 0; matIndex < renderer.materials.Length; matIndex++)
                {
                    renderer.sharedMaterials[matIndex] = renderer.sharedMaterials[matIndex] == materialMap[matIndex].origMaterial ? materialMap[matIndex].fadeMaterial : materialMap[matIndex].origMaterial;
                }
            }
        }

        public enum ShaderPropertyType
        {
            Color,
            Vector,
            Float,
            Range,
            Texture,
        }

        [System.Serializable]
        public struct ShaderProperty
        {
            public string name;
            public ShaderPropertyType type;
        }

        [Button("Get Shader Properties")]
        public void Populate()
        {
            _properties.Clear();

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

                _properties.Add(shaderProperty);
            }
        }
    }
}