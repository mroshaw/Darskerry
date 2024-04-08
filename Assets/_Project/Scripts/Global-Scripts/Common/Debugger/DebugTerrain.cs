using DaftAppleGames.Common.GameControllers;
using UnityEngine;
using Sirenix.OdinInspector;
using VisualDesignCafe.Rendering.Nature;

namespace DaftAppleGames.Common.Debugger
{
    public class DebugTerrain : DebugBase
    {
        [BoxGroup("Terrain Settings")] public Terrain terrain;
        [BoxGroup("Terrain Settings")] public GameObject vegetationEngineGameObject;
        [BoxGroup("Terrain Settings")] public NatureRendererDefaultSettings natureRendererDefaultSettings;

        private NatureRendererCameraSettings _cameraSettings;
        private NatureRenderer _natureRenderer;

        /// <summary>
        /// Configure terrain before any components start
        /// </summary>
        private void Awake()
        {
            terrain.drawTreesAndFoliage = false;
            _natureRenderer = terrain.GetComponent<NatureRenderer>();
            _cameraSettings = PlayerCameraManager.Instance.MainCamera.GetComponent<NatureRendererCameraSettings>();
        }
        
        /// <summary>
        /// Set up the components
        /// </summary>
        public void Start()
        {


        }


        /// <summary>
        /// Enable Nature Renderer
        /// </summary>
        public void EnableNatureRenderer(out string logText)
        {
            _cameraSettings.Render = true;
            logText = "Nature renderer is ENABLED on MainCamera.";
        }

        /// <summary>
        /// Disable Nature Renderer
        /// </summary>
        public void DisableNatureRenderer(out string logText)
        {
            _cameraSettings.Render = false;
            logText = "Nature renderer is DISABLED on MainCamera.";

        }

        /// <summary>
        /// Toggle the current state of Nature Renderer
        /// </summary>
        /// <param name="logText"></param>
        public void ToggleNatureRenderer(out string logText)
        {
            _cameraSettings.Render = !_cameraSettings.Render;
            logText = $"Nature renderer is {GetStateText(_cameraSettings.Render)} on MainCamera.";
        }

        /// <summary>
        /// Enable trees in NR (terrain draw should be 'false')
        /// </summary>
        public void EnableTrees(out string logText)
        {
            _natureRenderer.RenderTreesWithNatureRenderer = true;
            _natureRenderer.Restart();
            terrain.drawTreesAndFoliage = false;
            logText = "Tree rendering is ENABLED on NatureRenderer";

        }

        /// <summary>
        /// Disable trees in NR
        /// </summary>
        public void DisableTrees(out string logText)
        {
            _natureRenderer.RenderTreesWithNatureRenderer = false;
            _natureRenderer.Restart();
            terrain.drawTreesAndFoliage = false;
            Debug.Log("NatureRenderer Trees disabled.");
            logText = "Tree rendering is DISABLED on NatureRenderer";
        }

        /// <summary>
        /// Toggle the current state of trees in NR
        /// </summary>
        /// <param name="logText"></param>
        public void ToggleTrees(out string logText)
        {
            _natureRenderer.RenderTreesWithNatureRenderer = !_natureRenderer.RenderTreesWithNatureRenderer;
            _natureRenderer.Restart();
            terrain.drawTreesAndFoliage = false;
            logText = $"Tree rendering is {GetStateText(_natureRenderer.RenderTreesWithNatureRenderer)} on NatureRenderer.";
        }

        /// <summary>
        /// Enable details in NR
        /// </summary>
        public void EnableDetails(out string logText)
        {
            _natureRenderer.RenderDetailsWithNatureRenderer = true;
            _natureRenderer.Restart();
            terrain.drawTreesAndFoliage = false;
            logText = "Detail rendering is ENABLED on NatureRenderer";
        }

        /// <summary>
        /// Disable details in NR
        /// </summary>
        public void DisableDetails(out string logText)
        {
            _natureRenderer.RenderDetailsWithNatureRenderer = false;
            _natureRenderer.Restart();
            terrain.drawTreesAndFoliage = false;
            logText = "Detail rendering is DISABLED on NatureRenderer";
        }

        /// <summary>
        /// Toggle the current state of details in NR
        /// </summary>
        /// <param name="logText"></param>
        public void ToggleDetails(out string logText)
        {
            _natureRenderer.RenderDetailsWithNatureRenderer = !_natureRenderer.RenderDetailsWithNatureRenderer;
            _natureRenderer.Restart();
            terrain.drawTreesAndFoliage = false;
            logText = $"Tree rendering is {GetStateText(_natureRenderer.RenderDetailsWithNatureRenderer)} on NatureRenderer.";
        }

        /// <summary>
        /// Toggles terrain detail and tree shadows on
        /// </summary>
        public void EnableShadows(out string logText)
        {
            _cameraSettings.RenderShadows = true;
            logText = "Shadow rendering is ENABLED on NatureRenderer";
        }

        /// <summary>
        /// Toggles terrain detail and tree shadows off
        /// </summary>
        public void DisableShadows(out string logText)
        {
            _cameraSettings.RenderShadows = false;
            logText = "Shadow rendering is DISABLED on NatureRenderer";
        }

        /// <summary>
        /// Toggle the current state of shadows in NR
        /// </summary>
        /// <param name="logText"></param>
        public void ToggleShadows(out string logText)
        {
            _cameraSettings.RenderShadows = !_cameraSettings.RenderShadows;
            logText = $"Shadow rendering is {GetStateText(_cameraSettings.RenderShadows)} on MainCamera.";
        }

        /// <summary>
        /// Sets the terrain shadow distance
        /// </summary>
        /// <param name="distance"></param>
        /// <param name="logText"></param>
        public void SetShadowDistance(float distance, out string logText)
        {
            _cameraSettings.ShadowDistance = distance;
            logText = $"Shadow rendering distance is {distance} on NatureRenderer";
        }


        /// <summary>
        /// Return the Render Shadow Distance
        /// </summary>
        /// <returns></returns>
        public float GetShadowDistance()
        {
            if (_cameraSettings.ShadowDistance == null)
            {
                return 0.0f;
            }

            return (float)_cameraSettings.ShadowDistance;
        }

        /// <summary>
        /// Set the render distance
        /// </summary>
        /// <param name="distance"></param>
        /// <param name="logText"></param>
        public void SetRenderDistance(float distance, out string logText)
        {
            _cameraSettings.RenderDistance = distance;
            logText = $"Detail rendering distance is {distance} on NatureRenderer";
        }

        /// <summary>
        /// Return the Render Distance
        /// </summary>
        /// <returns></returns>
        public float GetRenderDistance()
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
        /// Sets the detail render distance on the defaults
        /// </summary>
        /// <param name="distance"></param>
        /// <param name="logText"></param>
        public void SetDetailRenderDistance(float distance, out string logText)
        {
            natureRendererDefaultSettings.DetailRenderDistance = distance;
            logText = $"Detail render distance is {distance} on NatureRenderer";
        }

        /// <summary>
        /// Return the default Detail Render Distance
        /// </summary>
        /// <returns></returns>
        public float GetDetailRenderDistance()
        {
            return natureRendererDefaultSettings.DetailRenderDistance;
        }

        /// <summary>
        /// Sets the tree render distance on the defaults
        /// </summary>
        /// <param name="distance"></param>
        /// <param name="logText"></param>
        public void SetTreeRenderDistance(float distance, out string logText)
        {
            natureRendererDefaultSettings.TreeRenderDistance = distance;
            logText = $"Tree render distance is {distance} on NatureRenderer";
        }

        /// <summary>
        /// Return the default Tree Render Distance
        /// </summary>
        /// <returns></returns>
        public float GetTreeRenderDistance()
        {
            return natureRendererDefaultSettings.TreeRenderDistance;
        }

        /// <summary>
        /// Sets the detail shadow render distance on the defaults
        /// </summary>
        /// <param name="distance"></param>
        /// <param name="logText"></param>
        public void SetDetailShadowRenderDistance(float distance, out string logText)
        {
            natureRendererDefaultSettings.DetailShadowDistance = distance;
            logText = $"Detail shadow distance is {distance} on NatureRenderer";
        }

        /// <summary>
        /// Return the default Detail Shadow Render Distance
        /// </summary>
        /// <returns></returns>
        public float GetDetailShadowRenderDistance()
        {
            return natureRendererDefaultSettings.DetailShadowDistance;
        }

        /// <summary>
        /// Sets the tree shadow render distance on the defaults
        /// </summary>
        /// <param name="distance"></param>
        /// <param name="logText"></param>
        public void SetTreeShadowRenderDistance(float distance, out string logText)
        {
            natureRendererDefaultSettings.TreeShadowDistance = distance;
            logText = $"Tree shadow distance is {distance} on NatureRenderer";
        }

        /// <summary>
        /// Return the default Tree Shadow Render Distance
        /// </summary>
        /// <returns></returns>
        public float GetTreeShadowRenderDistance()
        {
            return natureRendererDefaultSettings.TreeShadowDistance;
        }

        /// <summary>
        /// Toggle the state of TVE
        /// </summary>
        /// <param name="logText"></param>
        public void ToggleVegetationEngine(out string logText)
        {
            vegetationEngineGameObject.SetActive(!vegetationEngineGameObject.activeSelf);
            logText = $"TVE is {GetStateText(vegetationEngineGameObject.activeSelf)}";
        }

        /// <summary>
        /// Returns a bool state as text (ENABLED or DISABLED)
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        private string GetStateText(bool state)
        {
            return (state) ? "ENABLED" : "DISABLED";
        }

        /// <summary>
        /// Returns a nullable bool state as text
        /// </summary>
        /// <param name="nullableState"></param>
        /// <returns></returns>
        private string GetStateText(bool? nullableState)
        {
            if (nullableState == null)
            {
                return GetStateText(false);
            }
            else
            {
                return GetStateText((bool)nullableState);
            }
        }
    }
}