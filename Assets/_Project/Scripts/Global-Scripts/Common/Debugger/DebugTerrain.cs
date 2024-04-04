using DaftAppleGames.Common.GameControllers;
using UnityEngine;
using Sirenix.OdinInspector;
using VisualDesignCafe.Rendering.Nature;

namespace DaftAppleGames.Common.Debugger
{
    public class DebugTerrain : DebugBase
    {
        [BoxGroup("Terrain Settings")] public Terrain terrain;

        private NatureRendererCameraSettings _cameraSettings;
        private NatureRenderer _natureRenderer;

        /// <summary>
        /// Configure terrain before any components start
        /// </summary>
        private void Awake()
        {
            terrain.drawTreesAndFoliage = false;
        }
        
        /// <summary>
        /// Set up the components
        /// </summary>
        public void Start()
        {
            _cameraSettings = PlayerCameraManager.Instance.MainCamera.GetComponent<NatureRendererCameraSettings>();
            _natureRenderer = terrain.GetComponent<NatureRenderer>();
        }

        /// <summary>
        /// Enable Nature Renderer
        /// </summary>
        public void EnableNatureRenderer()
        {
            _cameraSettings.Render = true;
        }

        /// <summary>
        /// Disable Nature Renderer
        /// </summary>
        public void DisableNatureRenderer()
        {
            _cameraSettings.Render = false;
        }

        /// <summary>
        /// Enable trees in NR (terrain draw should be 'false')
        /// </summary>
        public void EnableTrees()
        {
            _natureRenderer.RenderTreesWithNatureRenderer = true;
            _natureRenderer.Restart();
        }

        /// <summary>
        /// Disable trees in NR
        /// </summary>
        public void DisableTrees()
        {
            _natureRenderer.RenderTreesWithNatureRenderer = false;
            _natureRenderer.Restart();
        }

        /// <summary>
        /// Enable details in NR
        /// </summary>
        public void EnableDetails()
        {
            _natureRenderer.RenderDetailsWithNatureRenderer = true;
            _natureRenderer.Restart();
        }

        /// <summary>
        /// Disable details in NR
        /// </summary>
        public void DisableDetails()
        {
            _natureRenderer.RenderDetailsWithNatureRenderer = false;
            _natureRenderer.Restart();
        }
        
        /// <summary>
        /// Toggles terrain detail and tree shadows on
        /// </summary>
        public void EnableShadows()
        {
            _cameraSettings.RenderShadows = true;
        }

        /// <summary>
        /// Toggles terrain detail and tree shadows off
        /// </summary>
        public void DisableShadows()
        {
            _cameraSettings.RenderShadows =false;
        }


        /// <summary>
        /// Sets the terrain shadow distance
        /// </summary>
        /// <param name="distance"></param>
        public void SetShadowDistance(float distance)
        {
            _cameraSettings.ShadowDistance = distance;
        }

        /// <summary>
        /// Set the terrain detail render distance
        /// </summary>
        /// <param name="distance"></param>
        public void SetDetailDistance(float distance)
        {
            _cameraSettings.RenderDistance = distance;
        }

        /// <summary>
        /// Sets the density in distance
        /// </summary>
        /// <param name="density"></param>
        public void SetDistanceRender(float density)
        {
            _cameraSettings.DensityInDistance = density;
        }

        /// <summary>
        /// Sets the density in distance falloff
        /// </summary>
        /// <param name="falloff"></param>
        public void SetDistanceFalloff(Vector2 falloff)
        {
            _cameraSettings.DensityInDistanceFalloff = falloff;
        }
    }
}
