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
        [BoxGroup("Spawn Settings")] [SerializeField] private float fadeTime = 10.0f;
        [BoxGroup("Setup")] [SerializeField] private Material referenceMaterial;
        [BoxGroup("Setup")] [SerializeField] private Material[] fadeMaterials;
        [BoxGroup("Setup")] [SerializeField] private List<ShaderProperty> properties = new();
        [BoxGroup("Events")] public UnityEvent FadeInCompleteEvent;
        [BoxGroup("Events")] public UnityEvent FadeOutCompleteEvent;
        private List<SwapBackData> _swapBackData = new List<SwapBackData>();

        private Dictionary<Material, Material> _swapMaterialCache =
            new Dictionary<Material, Material>();

        [Button("Fade In")]
        public void FadeIn()
        {
            SwapMaterials();
            StartCoroutine(FadeMaterialAlpha(0, 1, SwapBackMaterials));
        }

        [Button("Fade Out")]
        public void FadeOut()
        {
            SwapMaterials();
            StartCoroutine(FadeMaterialAlpha(1, 0, SwapBackMaterials));

        }

        private IEnumerator FadeMaterialAlpha(float start, float end,Action onComplete)
        {
            float time = 0;

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
            foreach (Material mat in fadeMaterials)
            {
                Color fadedColor = mat.color;
                fadedColor.a = alpha;
                mat.color = fadedColor;
            }
        }

        [Button("Swap Materials")]
        private void SwapMaterials()
        {
            SwapBackMaterials();
            foreach (SkinnedMeshRenderer r in gameObject.GetComponentsInChildren<SkinnedMeshRenderer>(true))
            {
                SwapMaterialsInternal(r);
            }
        }

        private void SwapMaterialsInternal(SkinnedMeshRenderer renderer)
        {
            Material[] mats = renderer.materials;
            Material[] oldMats = new Material[mats.Length];
            mats.CopyTo(oldMats, 0);
            _swapBackData.Add(new SwapBackData(renderer, oldMats));

            for (int i = 0; i < mats.Length; i++)
            {
                Material mat = mats[i];
                if (!_swapMaterialCache.TryGetValue(mat, out Material swapMat))
                {
                    swapMat = GameObject.Instantiate(referenceMaterial);
                    CopyMaterialValues(mat, swapMat);

                    Color mainColor = swapMat.GetColor("_BaseColor");
                    mainColor.a = 0.4f;
                    swapMat.SetColor("_BaseColor", mainColor);
                    swapMat.SetFloat("_EmissiveExposureWeight", 1);

                    _swapMaterialCache[mat] = swapMat;
                }
                mats[i] = swapMat;
            }
            renderer.sharedMaterials = mats;
        }
        private bool CopyMaterialValues(Material from, Material to)
        {
            if (from.shader.name != to.shader.name)
            {
                Debug.LogError("Shaders don't match. " +
                    "FromShader: " + from.shader.name +
                    ", ToShader: " + to.shader.name);
                return false;
            }

            foreach (ShaderProperty prop in properties)
            {
                switch (prop.type)
                {
                    case ShaderPropertyType.Color:
                        to.SetColor(prop.name, from.GetColor(prop.name));
                        break;
                    case ShaderPropertyType.Vector:
                        to.SetVector(prop.name, from.GetVector(prop.name));
                        break;
                    case ShaderPropertyType.Float:
                        to.SetFloat(prop.name, from.GetFloat(prop.name));
                        break;
                    case ShaderPropertyType.Range:
                        to.SetFloat(prop.name, from.GetFloat(prop.name));
                        break;
                    case ShaderPropertyType.Texture:
                        to.SetTexture(prop.name, from.GetTexture(prop.name));
                        break;
                }
            }


            return true;
        }

        [Button("Swap Back Materials")]
        private void SwapBackMaterials()
        {
            foreach (SwapBackData swapBack in _swapBackData)
            {
                swapBack.renderer.sharedMaterials = swapBack.mats;
            }

            _swapBackData.Clear();
        }

        [Button("Get Materials")]
        private void GetMaterials()
        {
            #if UNITY_EDITOR

            fadeMaterials = GetComponentInChildren<SkinnedMeshRenderer>(true).sharedMaterials;
            referenceMaterial = fadeMaterials[0];
#endif
        }

        [Button("Get Shader Properties")]
        private void GetShaderProperties()
        {
#if UNITY_EDITOR
            properties.Clear();

            Shader shader = fadeMaterials[0].shader;
            int count = UnityEditor.ShaderUtil.GetPropertyCount(shader);
            for (int i = 0; i < count; i++)
            {
                ShaderProperty shaderProperty = new ShaderProperty()
                {
                    name = UnityEditor.ShaderUtil.GetPropertyName(shader, i),
                    type = (ShaderPropertyType)UnityEditor.ShaderUtil
                                .GetPropertyType(shader, i)
                };

                properties.Add(shaderProperty);
            }
#endif
        }

        struct SwapBackData
        {
            public Renderer renderer;
            public Material[] mats;

            public SwapBackData(Renderer renderer, Material[] mats)
            {
                this.renderer = renderer;
                this.mats = mats;
            }
        }

        [System.Serializable]
        public struct ShaderProperty
        {
            public string name;
            public ShaderPropertyType type;
        }

        public enum ShaderPropertyType
        {
            Color,
            Vector,
            Float,
            Range,
            Texture,
        }
    }
}