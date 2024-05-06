using DaftAppleGames.Common.UI;
using UnityEngine;
using Sirenix.OdinInspector;

namespace DaftAppleGames.Common.Debugging
{
    public class DebugTerrainUi : DebugBaseUi
    {
        [BoxGroup("Settings")] public string debugTerrainObjectName;

        [BoxGroup("UI Settings")] public UiSlider renderDistanceSlider;
        [BoxGroup("UI Settings")] public UiSlider shadowDistanceSlider;
        [BoxGroup("UI Settings")] public UiSlider detailRenderDistanceSlider;
        [BoxGroup("UI Settings")] public UiSlider detailShadowRenderDistanceSlider;
        [BoxGroup("UI Settings")] public UiSlider treeRenderDistanceSlider;
        [BoxGroup("UI Settings")] public UiSlider treeShadowRenderDistanceSlider;

        private DebugTerrain _debugTerrain;
        private DebuggerUi _debuggerUi;

        /// <summary>
        /// Set up the component
        /// </summary>
        public override void Start()
        {
            base.Start();

            if (string.IsNullOrEmpty(debugTerrainObjectName))
            {
                Debug.LogError($"DebugSkyUi: Please set the debugSkyObjectName property on {gameObject.name}!");
            }
            else
            {
                _debugTerrain = (DebugTerrain)base.FindDebugObject<DebugTerrain>(debugTerrainObjectName);
            }
            _debuggerUi = GetComponentInParent<DebuggerUi>();
            SetValues();
        }

        /// <summary>
        /// Sets the control defaults
        /// </summary>
        public void SetValues()
        {
            // renderDistanceSlider.SetValueWithoutNotify(_debugTerrain.GetRenderDistance());
            // shadowDistanceSlider.SetValueWithoutNotify(_debugTerrain.GetShadowDistance());
            detailRenderDistanceSlider.SetValueWithoutNotify(_debugTerrain.GetDetailRenderDistance());
            detailShadowRenderDistanceSlider.SetValueWithoutNotify(_debugTerrain.GetDetailShadowRenderDistance());
            treeRenderDistanceSlider.SetValueWithoutNotify(_debugTerrain.GetTreeRenderDistance());
            treeShadowRenderDistanceSlider.SetValueWithoutNotify(_debugTerrain.GetTreeShadowRenderDistance());
        }

        /// <summary>
        /// Handler for enable NR button
        /// </summary>
        public void EnableRendererHandler()
        {
            _debugTerrain.EnableRenderer(out string logText);
            _debuggerUi.ShowLog(logText);
        }

        /// <summary>
        /// Handler for disable NR button
        /// </summary>
        public void DisableRendererHandler()
        {
            _debugTerrain.DisableRenderer(out string logText);
            _debuggerUi.ShowLog(logText);
        }

        /// <summary>
        /// Handler for Toggle NR button
        /// </summary>
        public void ToggleRendererHandler()
        {
            _debugTerrain.ToggleRenderer(out string logText);
            _debuggerUi.ShowLog(logText);
        }

        /// <summary>
        /// Handle enable shadows button
        /// </summary>
        public void EnableShadowsHandler()
        {
            _debugTerrain.EnableShadows(out string logText);
            _debuggerUi.ShowLog(logText);;
        }

        /// <summary>
        /// Handle disable shadows button
        /// </summary>
        public void DisableShadowsHandler()
        {
            _debugTerrain.DisableShadows(out string logText);
            _debuggerUi.ShowLog(logText);;
        }

        /// <summary>
        /// Handler for Toggle Shadows button
        /// </summary>
        public void ToggleShadowsHandler()
        {
            _debugTerrain.ToggleShadows(out string logText);
            _debuggerUi.ShowLog(logText);
        }

        /// <summary>
        /// Handler for Shadow Distance slider
        /// </summary>
        /// <param name="distance"></param>
        public void SetShadowDistanceHandler(float distance)
        {
            _debugTerrain.SetShadowDistance(distance, out string logText);
            _debuggerUi.ShowLog(logText);
        }

        /// <summary>
        /// Handler for the Render Distance slider
        /// </summary>
        /// <param name="distance"></param>
        public void SetRenderDistanceHandler(float distance)
        {
            _debugTerrain.SetRenderDistance(distance, out string logText);
            _debuggerUi.ShowLog(logText);
        }

        /// <summary>
        /// Handler for Detail Render Distance slider
        /// </summary>
        /// <param name="distance"></param>
        public void SetDetailRenderDistanceHandler(float distance)
        {
            _debugTerrain.SetDetailRenderDistance(distance, out string logText);
            _debuggerUi.ShowLog(logText);
        }

        /// <summary>
        /// Handler for Detail Shadow Distance slider
        /// </summary>
        /// <param name="distance"></param>
        public void SetDetailShadowRenderDistanceHandler(float distance)
        {
            _debugTerrain.SetDetailShadowRenderDistance(distance, out string logText);
            _debuggerUi.ShowLog(logText);
        }

        /// <summary>
        /// Handler for Tree Render Distance slider
        /// </summary>
        /// <param name="distance"></param>
        public void SetTreeRenderDistanceHandler(float distance)
        {
            _debugTerrain.SetTreeRenderDistance(distance, out string logText);
            _debuggerUi.ShowLog(logText);
        }

        /// <summary>
        /// Handler for Tree Shadow Render Distance slider
        /// </summary>
        /// <param name="distance"></param>
        public void SetTreeShadowRenderDistanceHandler(float distance)
        {
            _debugTerrain.SetTreeShadowRenderDistance(distance, out string logText);
            _debuggerUi.ShowLog(logText);
        }

        /// <summary>
        /// Handler for Enable Trees button
        /// </summary>
        public void EnableTreesHandler()
        {
            _debugTerrain.EnableTrees(out string logText);
            _debuggerUi.ShowLog(logText);
        }

        /// <summary>
        /// Handler for Disable Tree button
        /// </summary>
        public void DisableTreesHandler()
        {
            _debugTerrain.DisableTrees(out string logText);
            _debuggerUi.ShowLog(logText);
        }

        /// <summary>
        /// Handler for Render Trees button
        /// </summary>
        public void ToggleTreesHandler()
        {
            _debugTerrain.ToggleTrees(out string logText);
            _debuggerUi.ShowLog(logText);
        }

        /// <summary>
        /// Handler for Enable Details button
        /// </summary>
        public void EnableDetailsHandler()
        {
            _debugTerrain.EnableDetails(out string logText);
            _debuggerUi.ShowLog(logText);
        }

        /// <summary>
        /// Handler for Disable Details button
        /// </summary>
        public void DisableDetailsHandler()
        {
            _debugTerrain.DisableDetails(out string logText);
            _debuggerUi.ShowLog(logText);
        }

        /// <summary>
        /// Handler for Render Details button
        /// </summary>
        public void ToggleDetailsHandler()
        {
            _debugTerrain.ToggleDetails(out string logText);
            _debuggerUi.ShowLog(logText);
        }

        /// <summary>
        /// Handler for Toggle TVE button
        /// </summary>
        public void ToggleTVEHandler()
        {
            _debugTerrain.ToggleVegetationEngine(out string logText);
            _debuggerUi.ShowLog(logText);
        }

    }
}