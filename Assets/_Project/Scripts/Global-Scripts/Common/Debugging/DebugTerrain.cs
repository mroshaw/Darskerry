using UnityEngine;
using Sirenix.OdinInspector;

namespace DaftAppleGames.Common.Debugging
{
    public abstract class DebugTerrain : DebugBase
    {
        [BoxGroup("Terrain Settings")] public Terrain terrain;
        [BoxGroup("Terrain Settings")] public GameObject vegetationEngineGameObject;

        /// <summary>
        /// Configure terrain before any components start
        /// </summary>
        public virtual void Awake()
        {
            terrain.drawTreesAndFoliage = false;
        }
        
        /// <summary>
        /// Set up the components
        /// </summary>
        public virtual void Start()
        {
        }

        /// <summary>
        /// Enable Renderer
        /// </summary>
        public void EnableRenderer(out string logText)
        {
            EnableRendererState();
            logText = "Renderer is ENABLED.";
        }

        /// <summary>
        /// Abstract method to implement Enable the Renderer
        /// </summary>
        protected abstract void EnableRendererState();

        /// <summary>
        /// Disable Nature Renderer
        /// </summary>
        public void DisableRenderer(out string logText)
        {
            DisableRendererState();
            logText = "Nature renderer is DISABLED.";
        }

        /// <summary>
        /// Abstract method to implement Disable the Renderer
        /// </summary>
        protected abstract void DisableRendererState();

        /// <summary>
        /// Toggle the current state of Renderer
        /// </summary>
        /// <param name="logText"></param>
        public void ToggleRenderer(out string logText)
        {
            logText = $"Renderer is {GetStateText(ToggleRendererStateInRenderer())}.";
        }

        /// <summary>
        /// Abstract method to implement Toggle the Renderer
        /// </summary>
        /// <returns></returns>
        protected abstract bool ToggleRendererStateInRenderer();

        /// <summary>
        /// Enable trees in Renderer (terrain draw should be 'false')
        /// </summary>
        public void EnableTrees(out string logText)
        {
            EnableTreesInRenderer();
            logText = "Tree rendering is ENABLED.";
        }

        /// <summary>
        /// Abstract method to implement Enable the Trees
        /// </summary>
        protected abstract void EnableTreesInRenderer();

        /// <summary>
        /// Disable trees in Renderer
        /// </summary>
        public void DisableTrees(out string logText)
        {
            DisableTreesInRenderer();
            logText = "Tree rendering is DISABLED.";
        }

        /// <summary>
        /// Abstract method to implement Disable the Trees
        /// </summary>
        protected abstract void DisableTreesInRenderer();

        /// <summary>
        /// Toggle the current state of trees in Renderer
        /// </summary>
        /// <param name="logText"></param>
        public void ToggleTrees(out string logText)
        {
            logText = $"Tree rendering is {GetStateText(ToggleTreesStateInRenderer())}.";
        }

        /// <summary>
        /// Abstract method to implement Toggle the Trees
        /// </summary>
        /// <returns></returns>
        protected abstract bool ToggleTreesStateInRenderer();

        /// <summary>
        /// Enable details in Renderer
        /// </summary>
        public void EnableDetails(out string logText)
        {
            EnableDetailsInRenderer();
            logText = "Detail rendering is ENABLED.";
        }

        /// <summary>
        /// Abstract method to implement Enable the Details
        /// </summary>
        protected abstract void EnableDetailsInRenderer();

        /// <summary>
        /// Disable details in Renderer
        /// </summary>
        public void DisableDetails(out string logText)
        {
            DisableDetailsInRenderer();
            logText = "Detail rendering is DISABLED.";
        }

        /// <summary>
        /// Abstract method to implement Disable the Details
        /// </summary>
        protected abstract void DisableDetailsInRenderer();

        /// <summary>
        /// Toggle the current state of details in Renderer
        /// </summary>
        /// <param name="logText"></param>
        public void ToggleDetails(out string logText)
        {
            logText = $"Tree rendering is {GetStateText(ToggleDetailsStateInRenderer())}.";
        }

        /// <summary>
        /// Abstract method to implement Toggle the Details
        /// </summary>
        /// <returns></returns>
        protected abstract bool ToggleDetailsStateInRenderer();

        /// <summary>
        /// Toggles terrain detail and tree shadows on
        /// </summary>
        public void EnableShadows(out string logText)
        {
            EnableShadowsInRenderer();
            logText = "Shadow rendering is ENABLED.";
        }

        /// <summary>
        /// Abstract method to implement Enable the Shadows
        /// </summary>
        protected abstract void EnableShadowsInRenderer();

        /// <summary>
        /// Toggles terrain detail and tree shadows off
        /// </summary>
        public void DisableShadows(out string logText)
        {
            DisableShadowsInRenderer();
            logText = "Shadow rendering is DISABLED.";
        }

        /// <summary>
        /// Abstract method to implement Disable the Shadows
        /// </summary>
        protected abstract void DisableShadowsInRenderer();

        /// <summary>
        /// Toggle the current state of shadows in NR
        /// </summary>
        /// <param name="logText"></param>
        public void ToggleShadows(out string logText)
        {
            logText = $"Shadow rendering is {GetStateText(ToggleShadowsInRenderer())}.";
        }

        /// <summary>
        /// Abstract method to implement Toggle the Shadows
        /// </summary>
        /// <returns></returns>
        protected abstract bool ToggleShadowsInRenderer();

        /// <summary>
        /// Sets the terrain shadow distance
        /// </summary>
        /// <param name="distance"></param>
        /// <param name="logText"></param>
        public void SetShadowDistance(float distance, out string logText)
        {
            SetShadowDistanceInRenderer(distance);
            logText = $"Shadow rendering distance is {distance}.";
        }

        /// <summary>
        /// Abstract method to implement Set Shadow Distance
        /// </summary>
        /// <param name="distance"></param>
        protected abstract void SetShadowDistanceInRenderer(float distance);

        /// <summary>
        /// Return the Render Shadow Distance
        /// </summary>
        /// <returns></returns>
        public float GetShadowDistance()
        {
            return GetShadowDistanceInRenderer();
        }

        /// <summary>
        /// Abstract method to implement Get Shadow Distance
        /// </summary>
        /// <returns></returns>
        protected abstract float GetShadowDistanceInRenderer();

        /// <summary>
        /// Set the render distance
        /// </summary>
        /// <param name="distance"></param>
        /// <param name="logText"></param>
        public void SetRenderDistance(float distance, out string logText)
        {
            SetRenderDistanceInRenderer(distance);
            logText = $"Rendering distance is {distance}.";
        }

        /// <summary>
        /// Abstract method to implement Set Render Distance
        /// </summary>
        /// <param name="distance"></param>
        protected abstract void SetRenderDistanceInRenderer(float distance);

        /// <summary>
        /// Return the Render Distance
        /// </summary>
        /// <returns></returns>
        public float GetRenderDistance()
        {
            return GetRenderDistanceInRenderer();
        }

        protected abstract float GetRenderDistanceInRenderer();

        /// <summary>
        /// Sets the detail render distance on the defaults
        /// </summary>
        /// <param name="distance"></param>
        /// <param name="logText"></param>
        public void SetDetailRenderDistance(float distance, out string logText)
        {
            SetDetailRenderDistanceInRenderer(distance);
            logText = $"Detail render distance is {distance}.";
        }

        /// <summary>
        /// Abstract method to implement Set Detail Render Distance
        /// </summary>
        /// <param name="distance"></param>
        protected abstract void SetDetailRenderDistanceInRenderer(float distance);

        /// <summary>
        /// Return the default Detail Render Distance
        /// </summary>
        /// <returns></returns>
        public float GetDetailRenderDistance()
        {
            return GetDetailRenderDistanceInRenderer();
        }

        /// <summary>
        /// Abstract method to implement Get Detail Render Distance
        /// </summary>
        /// <returns></returns>
        protected abstract float GetDetailRenderDistanceInRenderer();

        /// <summary>
        /// Sets the tree render distance on the defaults
        /// </summary>
        /// <param name="distance"></param>
        /// <param name="logText"></param>
        public void SetTreeRenderDistance(float distance, out string logText)
        {
            SetTreeRenderDistanceInRenderer(distance);
            logText = $"Tree render distance is {distance}.";
        }

        /// <summary>
        /// Abstract method to implement Set Tree Render Distance
        /// </summary>
        /// <param name="distance"></param>
        protected abstract void SetTreeRenderDistanceInRenderer(float distance);

        /// <summary>
        /// Return the default Tree Render Distance
        /// </summary>
        /// <returns></returns>
        public float GetTreeRenderDistance()
        {
            return GetTreeRenderDistanceInRenderer();
        }

        /// <summary>
        /// Abstract method to implement Set Tree Render Distance
        /// </summary>
        protected abstract float GetTreeRenderDistanceInRenderer();


        /// <summary>
        /// Sets the detail shadow render distance on the defaults
        /// </summary>
        /// <param name="distance"></param>
        /// <param name="logText"></param>
        public void SetDetailShadowRenderDistance(float distance, out string logText)
        {
            SetDetailShadowRenderDistanceInRenderer(distance);
            logText = $"Detail shadow distance is {distance}.";
        }

        /// <summary>
        /// Abstract method to implement Set Detail Shadow Render Distance
        /// </summary>
        /// <param name="distance"></param>
        protected abstract void SetDetailShadowRenderDistanceInRenderer(float distance);

        /// <summary>
        /// Return the default Detail Shadow Render Distance
        /// </summary>
        /// <returns></returns>
        public float GetDetailShadowRenderDistance()
        {
            return GetDetailShadowRenderDistanceInRenderer();
        }

        /// <summary>
        /// Abstract method to implement Get Detail Shadow Render Distance
        /// </summary>
        protected abstract float GetDetailShadowRenderDistanceInRenderer();

        /// <summary>
        /// Sets the tree shadow render distance on the defaults
        /// </summary>
        /// <param name="distance"></param>
        /// <param name="logText"></param>
        public void SetTreeShadowRenderDistance(float distance, out string logText)
        {
            SetTreeShadowRenderDistanceInRenderer(distance);
            logText = $"Tree shadow distance is {distance}.";
        }

        /// <summary>
        /// Abstract method to implement Set Tree Shadow Render Distance
        /// </summary>
        /// <param name="distance"></param>
        protected abstract void SetTreeShadowRenderDistanceInRenderer(float distance);

        /// <summary>
        /// Return the default Tree Shadow Render Distance
        /// </summary>
        /// <returns></returns>
        public float GetTreeShadowRenderDistance()
        {
            return GetTreeShadowRenderDistanceInRenderer();
        }

        /// <summary>
        /// Abstract method to implement Get Tree Shadow Render Distance
        /// </summary>
        protected abstract float GetTreeShadowRenderDistanceInRenderer();

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