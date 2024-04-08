using System.Collections;
using System.Collections.Generic;
using Magique.SoulLink;
using Sirenix.OdinInspector;
using UnityEngine;
using static Magique.SoulLink.ShaderUtils;

namespace DaftAppleGames.Common.AI
{
    public class SpawnAction_HdrpFader : BaseSpawnAction
    {
        [Tooltip("How long to take for the spawn to fully fade in.")]
        public float fadeInLength = 2f;

        [Tooltip("How long to take for the spawn to fully fade out.")]
        public float fadeOutLength = 2f;

        private string _texColorName = "_Color";

        private Dictionary<Material, Material> _materials = new Dictionary<Material, Material>();
        private List<Renderer> _renderers = new List<Renderer>();

        /// <summary>
        /// Prepare this object for fading in/out
        /// </summary>
        private void Awake()
        {
            // Get all the materials in this object so we can control the alpha fading
            Renderer[] renderers = GetComponentsInChildren<Renderer>(true);
            foreach (Renderer currRenderer in renderers)
            {
                if (currRenderer.enabled)
                {
                    _renderers.Add(currRenderer);

                    foreach (var material in currRenderer.materials)
                    {
                        if (material.HasProperty(_texColorName))
                        {
                            _materials[material] = new Material(material);
                        }
                    }
                }
            }

            SetInvisible(_renderers, _materials, _texColorName);
        }

        public void SetTexColorName(string value)
        {
            _texColorName = value;
        }

        public string GetTexColorName()
        {
            return _texColorName;
        }

        public void SetFadeInLength(float value)
        {
            fadeInLength = value;
        }

        public float GetFadeInLength()
        {
            return fadeInLength;
        }

        public void SetFadeOutLength(float value)
        {
            fadeOutLength = value;
        }

        public float GetFadeOutLength()
        {
            return fadeOutLength;
        }
        override public void PerformSpawnAction()
        {
            if (_performing) return;

            if (OnSpawnActionStarted != null)
            {
                OnSpawnActionStarted.Invoke();
            }

            if (_spawnActionEnabled)
            {
                _performing = true;

                FadeIn();
            }
            else
            {
                SpawnActionComplete();
            }
        }

        public override void PerformDespawnAction()
        {
            if (_performing) return;

            if (OnDespawnActionStarted != null)
            {
                OnDespawnActionStarted.Invoke();
            }

            if (_despawnActionEnabled)
            {
                _performing = true;

                FadeOut();
            }
            else
            {
                DespawnActionComplete();
            }
        }

        /// <summary>
        /// Fade in the spawn
        /// </summary>
        [Button("Fade In")]
        void FadeIn()
        {
            foreach (var renderer in _renderers)
            {
                renderer.enabled = true;
            }

            var i = 0;
            foreach (var material in _materials)
            {
                // Start alpha value at 0
                ChangeRenderMode(material.Key, BlendMode.Transparent);
                Color color = material.Key.GetColor(_texColorName);
                material.Key.SetColor(_texColorName, new Color(color.r, color.g, color.b, 0f));

                if (i == _materials.Count - 1)
                {
                    StartCoroutine(DoFadeIn(material.Key, _texColorName, fadeInLength, true));
                }
                else
                {
                    StartCoroutine(DoFadeIn(material.Key, _texColorName, fadeInLength));
                }

                ++i;
            }
        }

        /// <summary>
        /// Fade in the spawn's materials
        /// </summary>
        /// <param name="material"></param>
        /// <param name="texColorName"></param>
        /// <param name="duration"></param>
        /// <param name="callCompleteHandler"></param>
        /// <returns></returns>
        IEnumerator DoFadeIn(Material material, string texColorName, float duration, bool callCompleteHandler = false)
        {
            float timeElapsed = 0;
            float alpha;

            while (timeElapsed < duration)
            {
                alpha = Mathf.Lerp(0f, 1f, timeElapsed / duration);
                material.SetColor(texColorName, new Color(material.color.r, material.color.g, material.color.b, alpha));
                timeElapsed += Time.deltaTime;

                yield return null;
            }

            // Snap to final value
            alpha = 1f;
            material.SetColor(texColorName, new Color(material.color.r, material.color.g, material.color.b, alpha));

            if (callCompleteHandler)
            {
                SpawnActionComplete();
            }
        }

        /// <summary>
        /// Fade out the spawn
        /// </summary>
        [Button("Fade Out")]
        void FadeOut()
        {
            var i = 0;
            foreach (var material in _materials)
            {
                ChangeRenderMode(material.Key, BlendMode.Transparent);
                if (i == _materials.Count - 1) // if on last material to fade out
                {
                    StartCoroutine(DoFadeOut(material.Key, _texColorName, fadeOutLength, true));
                }
                else
                {
                    StartCoroutine(DoFadeOut(material.Key, _texColorName, fadeOutLength));
                }

                ++i;
            }
        }

        /// <summary>
        /// Fade out the spawn's materials
        /// </summary>
        /// <param name="material"></param>
        /// <param name="texColorName"></param>
        /// <param name="duration"></param>
        /// <param name="callCompleteHandler"></param>
        /// <returns></returns>
        IEnumerator DoFadeOut(Material material, string texColorName, float duration, bool callCompleteHandler = false)
        {
            float timeElapsed = 0;
            float alpha;

            while (timeElapsed < duration)
            {
                alpha = Mathf.Lerp(1f, 0f, timeElapsed / duration);
                material.SetColor(texColorName, new Color(material.color.r, material.color.g, material.color.b, alpha));
                timeElapsed += Time.deltaTime;

                yield return null;
            }

            // Snap to final value
            alpha = 0f;
            material.SetColor(texColorName, new Color(material.color.r, material.color.g, material.color.b, alpha));

            if (callCompleteHandler)
            {
                DespawnActionComplete();
            }
        }

        override public void SpawnActionComplete()
        {
            foreach (var material in _materials)
            {
                Material origMaterial = material.Value;
                material.Key.CopyPropertiesFromMaterial(origMaterial);
            }
            base.SpawnActionComplete();
        }

        override public void FinalSpawnState()
        {
            SetVisible(_renderers, _materials);
        }

        override public void FinalDespawnState()
        {
            // nothing required at this time
        }

        override public float GetSpawnActionDuration()
        {
            return fadeInLength;
        }

        override public float GetDespawnActionDuration()
        {
            return fadeOutLength;
        }
    }
}