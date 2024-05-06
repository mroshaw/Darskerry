#if NATURE_RENDERER
using DaftAppleGames.Common.GameControllers;
using UnityEngine;
using Sirenix.OdinInspector;
using VisualDesignCafe.Rendering.Nature;

namespace DaftAppleGames.Common.Debugging
{
    public class DebugTerrainNatureRenderer : DebugTerrain
    {
        [BoxGroup("NR Settings")] public NatureRendererDefaultSettings natureRendererDefaultSettings;

        private NatureRendererCameraSettings _cameraSettings;
        private NatureRenderer _natureRenderer;

        /// <summary>
        /// Configure terrain before any components start
        /// </summary>
        public override void Awake()
        {
            base.Awake();
            _natureRenderer = terrain.GetComponent<NatureRenderer>();
            _cameraSettings = PlayerCameraManager.Instance.MainCamera.GetComponent<NatureRendererCameraSettings>();
        }
        
        /// <summary>
        /// Set up the components
        /// </summary>
        public override void Start()
        {
            base.Start();
        }

        /// <summary>
        /// Implementation of abstract method to Enable Renderer
        /// </summary>
        protected override void EnableRendererState()
        {
            _cameraSettings.Render = true;
        }

        /// <summary>
        /// Implementation of abstract method to Disable Renderer
        /// </summary>
        protected override void DisableRendererState()
        {
            _cameraSettings.Render = false;
        }

        /// <summary>
        /// Implementation of abstract method to Toggle Renderer
        /// </summary>
        /// <returns></returns>
        protected override bool ToggleRendererStateInRenderer()
        {
            _cameraSettings.Render = !_cameraSettings.Render;
            if (_cameraSettings.Render == null)
            {
                return false;
            }
            return ((bool)_cameraSettings.Render);
        }

        /// <summary>
        /// Implementation of abstract method to Enable Trees
        /// </summary>
        protected override void EnableTreesInRenderer()
        {
            _natureRenderer.RenderTreesWithNatureRenderer = true;
            _natureRenderer.Restart();
        }

        /// <summary>
        /// Implementation of abstract method to Disable Trees
        /// </summary>
        protected override void DisableTreesInRenderer()
        {
            _natureRenderer.RenderTreesWithNatureRenderer = false;
            _natureRenderer.Restart();
        }

        /// <summary>
        /// Implementation of abstract method to Toggle Trees
        /// </summary>
        /// <returns></returns>
        protected override bool ToggleTreesStateInRenderer()
        {
            _natureRenderer.RenderTreesWithNatureRenderer = !_natureRenderer.RenderTreesWithNatureRenderer;
            _natureRenderer.Restart();
            return _natureRenderer.RenderTreesWithNatureRenderer;
        }

        /// <summary>
        /// Implementation of abstract method to Enable Details
        /// </summary>
        protected override void EnableDetailsInRenderer()
        {
            _natureRenderer.RenderDetailsWithNatureRenderer = true;
            _natureRenderer.Restart();
        }

        /// <summary>
        /// Implementation of abstract method to Disable Details
        /// </summary>
        protected override void DisableDetailsInRenderer()
        {
            _natureRenderer.RenderDetailsWithNatureRenderer = false;
            _natureRenderer.Restart();
        }

        /// <summary>
        /// Implementation of abstract method to Toggle Details
        /// </summary>
        /// <returns></returns>
        protected override bool ToggleDetailsStateInRenderer()
        {
            _natureRenderer.RenderDetailsWithNatureRenderer = !_natureRenderer.RenderDetailsWithNatureRenderer;
            _natureRenderer.Restart();
            return _natureRenderer.RenderDetailsWithNatureRenderer;
        }

        /// <summary>
        /// Implementation of abstract method to Enable Shadows
        /// </summary>
        protected override void EnableShadowsInRenderer()
        {
            _cameraSettings.RenderShadows = true;
        }

        /// <summary>
        /// Implementation of abstract method to Disable Shadows
        /// </summary>
        protected override void DisableShadowsInRenderer()
        {
            _cameraSettings.RenderShadows = false;
        }

        /// <summary>
        /// Implementation of abstract method to Toggle Shadows
        /// </summary>
        /// <returns></returns>
        protected override bool ToggleShadowsInRenderer()
        {
            _cameraSettings.RenderShadows = !_cameraSettings.RenderShadows;
            if (_cameraSettings.RenderShadows == null)
            {
                return false;
            }
            return (bool)_cameraSettings.RenderShadows;
        }

        /// <summary>
        /// Implementation of abstract method to Set Shadow Distance
        /// </summary>
        /// <param name="distance"></param>
        protected override void SetShadowDistanceInRenderer(float distance)
        {
            _cameraSettings.ShadowDistance = distance;
        }

        /// <summary>
        /// Implementation of abstract method to Get Shadow Distance
        /// </summary>
        /// <returns></returns>
        protected override float GetShadowDistanceInRenderer()
        {
            if (_cameraSettings.ShadowDistance == null)
            {
                return 0.0f;
            }

            return (float)_cameraSettings.ShadowDistance;
        }

        /// <summary>
        /// Implementation of abstract method to Set Render Distance
        /// </summary>
        /// <param name="distance"></param>
        protected override void SetRenderDistanceInRenderer(float distance)
        {
            _cameraSettings.RenderDistance = distance;
        }

        /// <summary>
        /// Implementation of abstract method to Get Render Distance
        /// </summary>
        /// <returns></returns>
        protected override float GetRenderDistanceInRenderer()
        {
            if (_cameraSettings.RenderDistance == null)
            {
                return 0.0f;
            }
            return (float)_cameraSettings.RenderDistance;
        }

        /// <summary>
        /// Sets the density in distance
        /// </summary>
        /// <param name="density"></param>
        /// <param name="logText"></param>
        public void SetDensityInDistance(float density, out string logText)
        {
            _cameraSettings.DensityInDistance = density;
            logText = $"Render distance is {density} on NatureRenderer";
        }

        /// <summary>
        /// Return the Density in Distance
        /// </summary>
        /// <returns></returns>
        public float GetDensityInDistance()
        {
            if (_cameraSettings.DensityInDistance == null)
            {
                return 0.0f;
            }
            return (float)_cameraSettings.DensityInDistance;
        }

        /// <summary>
        /// Sets the density in distance falloff
        /// </summary>
        /// <param name="falloff"></param>
        /// <param name="logText"></param>
        public void SetDistanceFalloff(Vector2 falloff, out string logText)
        {
            _cameraSettings.DensityInDistanceFalloff = falloff;
            logText = $"Density in distance falloff is {falloff.x},{falloff.y} on NatureRenderer";
        }

        /// <summary>
        /// Return the Density Falloff
        /// </summary>
        /// <returns></returns>
        public Vector2 GetDistanceFalloff()
        {
            if (_cameraSettings.DensityInDistanceFalloff == null)
            {
                return Vector2.zero;
            }
            return (Vector2)_cameraSettings.DensityInDistanceFalloff;
        }

        /// <summary>
        /// Implementation of abstract method to Set Detail Render Distance
        /// </summary>
        /// <param name="distance"></param>
        protected override void SetDetailRenderDistanceInRenderer(float distance)
        {
            natureRendererDefaultSettings.DetailRenderDistance = distance;
        }

        /// <summary>
        /// Implementation of abstract method to Get Detail Render Distance
        /// </summary>
        /// <returns></returns>
        protected override float GetDetailRenderDistanceInRenderer()
        {
            return natureRendererDefaultSettings.DetailRenderDistance;
        }

        /// <summary>
        /// Implementation of abstract method to Set Tree Render Distance
        /// </summary>
        /// <param name="distance"></param>
        protected override void SetTreeRenderDistanceInRenderer(float distance)
        {
            natureRendererDefaultSettings.TreeRenderDistance = distance;
        }

        /// <summary>
        /// Implementation of abstract method to Get Tree Render Distance
        /// </summary>
        /// <returns></returns>
        protected override float GetTreeRenderDistanceInRenderer()
        {
            return natureRendererDefaultSettings.TreeRenderDistance;
        }

        /// <summary>
        /// Implementation of abstract method to Set Detail Shadow Render Distance
        /// </summary>
        /// <param name="distance"></param>
        protected override void SetDetailShadowRenderDistanceInRenderer(float distance)
        {
            natureRendererDefaultSettings.DetailShadowDistance = distance;
        }

        /// <summary>
        /// Implementation of abstract method to Get Detail Shadow Render Distance
        /// </summary>
        /// <returns></returns>
        protected override float GetDetailShadowRenderDistanceInRenderer()
        {
            return natureRendererDefaultSettings.DetailShadowDistance;
        }

        /// <summary>
        /// Implementation of abstract method to Set Tree Shadow Distance
        /// </summary>
        /// <param name="distance"></param>
        protected override void SetTreeShadowRenderDistanceInRenderer(float distance)
        {
            natureRendererDefaultSettings.TreeShadowDistance = distance;
        }

        /// <summary>
        /// Implementation of abstract method to Get Tree Shadow Distance
        /// </summary>
        /// <returns></returns>
        protected override float GetTreeShadowRenderDistanceInRenderer()
        {
            return natureRendererDefaultSettings.TreeShadowDistance;
        }
    }
}
#endif