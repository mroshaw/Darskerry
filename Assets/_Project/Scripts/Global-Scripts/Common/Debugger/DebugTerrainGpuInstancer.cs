#if GPU_INSTANCER
using System.Collections.Generic;
using GPUInstancer;
using Sirenix.OdinInspector;

namespace DaftAppleGames.Common.Debugger
{
    public class DebugTerrainGpuInstancer : DebugTerrain
    {
        private List<GPUInstancerManager> _allManagers;
        private List<GPUInstancerManager> _detailManagers;
        private List<GPUInstancerManager> _treeManagers;
        private List<GPUInstancerPrototype> _detailPrototypes;
        private List<GPUInstancerPrototype> _treePrototypes;

        private bool _rendererState = true;
        private bool _shadowState = true;
        private bool _detailState = true;
        private bool _treeState = true;
        private bool _detailShadowState = true;
        private bool _treeShadowState = true;

        /// <summary>
        /// Configure terrain before any components start
        /// </summary>
        public override void Awake()
        {
        }

        /// <summary>
        /// Initialise GPU Instancer info
        /// </summary>
        public override void Start()
        {
            base.Start();
            InitGpuInstancer();

            // SetManagerState(_allManagers, RendererState);
            // SetShadowState(_allManagers, ShadowState);
        }

        /// <summary>
        /// Gather everything we need to manage GPU Instancer settings
        /// </summary>
        private void InitGpuInstancer()
        {
            _detailManagers = new List<GPUInstancerManager>();
            _treeManagers = new List<GPUInstancerManager>();

            _detailPrototypes = new List<GPUInstancerPrototype>();
            _treePrototypes = new List<GPUInstancerPrototype>();

            _allManagers = GPUInstancerAPI.GetActiveManagers();

            foreach (GPUInstancerManager gpuInstancerManager in _allManagers)
            {
                if (gpuInstancerManager.GetType() == typeof(GPUInstancerDetailManager))
                {
                    _detailManagers.Add(gpuInstancerManager);
                    AddPrototypes(gpuInstancerManager, _detailPrototypes);
                }

                if (gpuInstancerManager.GetType() == typeof(GPUInstancerTreeManager))
                {
                    _treeManagers.Add(gpuInstancerManager);
                    AddPrototypes(gpuInstancerManager, _treePrototypes);
                }
            }
        }

        /// <summary>
        /// Adds prototypes from the given manager onto the given list of prototypes
        /// </summary>
        /// <param name="manager"></param>
        /// <param name="prototypes"></param>
        private void AddPrototypes(GPUInstancerManager manager, List<GPUInstancerPrototype> prototypes)
        {
            prototypes.AddRange(GPUInstancerAPI.GetPrototypeList(manager));
        }

         /// <summary>
        /// Implementation of abstract method to Enable Renderer
        /// </summary>
        protected override void EnableRendererState()
        {
            _rendererState = SetAllManagerState(true);
        }

        /// <summary>
        /// Implementation of abstract method to Disable Renderer
        /// </summary>
        protected override void DisableRendererState()
        {
            _rendererState = SetAllManagerState(false);
        }

        /// <summary>
        /// Implementation of abstract method to Toggle Renderer
        /// </summary>
        /// <returns></returns>
        protected override bool ToggleRendererStateInRenderer()
        {
            _rendererState = SetManagerState(_allManagers, !_rendererState);
            return _rendererState;
        }

        /// <summary>
        /// Implementation of abstract method to Enable Trees
        /// </summary>
        protected override void EnableTreesInRenderer()
        {
            _treeState = SetManagerState(_treeManagers, true);
        }

        /// <summary>
        /// Implementation of abstract method to Disable Trees
        /// </summary>
        protected override void DisableTreesInRenderer()
        {
            _treeState = SetManagerState(_treeManagers, false);
        }

        /// <summary>
        /// Implementation of abstract method to Toggle Trees
        /// </summary>
        /// <returns></returns>
        protected override bool ToggleTreesStateInRenderer()
        {
            _treeState = SetManagerState(_treeManagers, !_treeState);
            return _treeState;
        }

        /// <summary>
        /// Implementation of abstract method to Enable Details
        /// </summary>
        protected override void EnableDetailsInRenderer()
        {
            _detailState = SetManagerState(_detailManagers, true);
        }

        /// <summary>
        /// Implementation of abstract method to Disable Details
        /// </summary>
        protected override void DisableDetailsInRenderer()
        {
            _detailState = SetManagerState(_detailManagers, false);
        }

        /// <summary>
        /// Implementation of abstract method to Toggle Details
        /// </summary>
        /// <returns></returns>
        protected override bool ToggleDetailsStateInRenderer()
        {
            _detailState = SetManagerState(_detailManagers, !_detailState);
            return _detailState;
        }

        /// <summary>
        /// Implementation of abstract method to Enable Shadows
        /// </summary>
        protected override void EnableShadowsInRenderer()
        {
            _shadowState = SetShadowState(_allManagers, true);
        }

        /// <summary>
        /// Implementation of abstract method to Disable Shadows
        /// </summary>
        protected override void DisableShadowsInRenderer()
        {
            _shadowState = SetShadowState(_allManagers, false);
        }

        /// <summary>
        /// Implementation of abstract method to Toggle Shadows
        /// </summary>
        /// <returns></returns>
        protected override bool ToggleShadowsInRenderer()
        {
            _shadowState = SetShadowState(_allManagers, !_shadowState);
            return _shadowState;
        }

        /// <summary>
        /// Implementation of abstract method to Set Shadow Distance
        /// </summary>
        /// <param name="distance"></param>
        protected override void SetShadowDistanceInRenderer(float distance)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Implementation of abstract method to Get Shadow Distance
        /// </summary>
        /// <returns></returns>
        protected override float GetShadowDistanceInRenderer()
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Implementation of abstract method to Set Render Distance
        /// </summary>
        /// <param name="distance"></param>
        protected override void SetRenderDistanceInRenderer(float distance)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Implementation of abstract method to Get Render Distance
        /// </summary>
        /// <returns></returns>
        protected override float GetRenderDistanceInRenderer()
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Sets the density in distance
        /// </summary>
        /// <param name="density"></param>
        /// <param name="logText"></param>
        public void SetDensityInDistance(float density, out string logText)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Return the Density in Distance
        /// </summary>
        /// <returns></returns>
        public float GetDensityInDistance()
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Implementation of abstract method to Set Detail Render Distance
        /// </summary>
        /// <param name="distance"></param>
        protected override void SetDetailRenderDistanceInRenderer(float distance)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Implementation of abstract method to Get Detail Render Distance
        /// </summary>
        /// <returns></returns>
        protected override float GetDetailRenderDistanceInRenderer()
        {
            return 0.0f;
        }

        /// <summary>
        /// Implementation of abstract method to Set Tree Render Distance
        /// </summary>
        /// <param name="distance"></param>
        protected override void SetTreeRenderDistanceInRenderer(float distance)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Implementation of abstract method to Get Tree Render Distance
        /// </summary>
        /// <returns></returns>
        protected override float GetTreeRenderDistanceInRenderer()
        {
            return 0.0f;
        }

        /// <summary>
        /// Implementation of abstract method to Set Detail Shadow Render Distance
        /// </summary>
        /// <param name="distance"></param>
        protected override void SetDetailShadowRenderDistanceInRenderer(float distance)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Implementation of abstract method to Get Detail Shadow Render Distance
        /// </summary>
        /// <returns></returns>
        protected override float GetDetailShadowRenderDistanceInRenderer()
        {
            return 0.0f;
        }

        /// <summary>
        /// Implementation of abstract method to Set Tree Shadow Distance
        /// </summary>
        /// <param name="distance"></param>
        protected override void SetTreeShadowRenderDistanceInRenderer(float distance)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Implementation of abstract method to Get Tree Shadow Distance
        /// </summary>
        /// <returns></returns>
        protected override float GetTreeShadowRenderDistanceInRenderer()
        {
            return 0.0f;
        }

        /// <summary>
        /// Enables or disables all GPUI Managers
        /// </summary>
        /// <param name="state"></param>
        private bool SetAllManagerState(bool state)
        {
            foreach (GPUInstancerManager manager in _allManagers)
            {
                manager.enabled = state;
            }
            return state;
        }

        /// <summary>
        /// Sets the state of a list of GPUI managers
        /// </summary>
        /// <param name="managers"></param>
        /// <param name="state"></param>
        private bool SetManagerState(List<GPUInstancerManager> managers, bool state)
        {
            foreach (GPUInstancerManager manager in managers)
            {
                manager.enabled = state;
            }

            return state;
        }

        /// <summary>
        /// Sets the Shadow State on a list of managers prototypes
        /// </summary>
        /// <param name="managers"></param>
        /// <param name="state"></param>
        private bool SetShadowState(List<GPUInstancerManager> managers, bool state)
        {
            foreach (GPUInstancerManager manager in managers)
            {
                foreach (GPUInstancerPrototype prototype in GPUInstancerAPI.GetPrototypeList(manager))
                {
                    prototype.isShadowCasting = state;
                }
                RefreshManager(manager);
            }

            return state;
        }

        /// <summary>
        /// Refresh the manager instance
        /// </summary>
        /// <param name="manager"></param>
        private void RefreshManager(GPUInstancerManager manager)
        {
            if (manager.GetType() == typeof(GPUInstancerDetailManager))
            {
                GPUInstancerAPI.UpdateDetailInstances((GPUInstancerDetailManager)manager);
            }
        }
    }
}
#endif