using UnityEngine;
using Sirenix.OdinInspector;

namespace DaftAppleGames.Common.Debugger
{
    public class DebugTerrainUi : DebugBaseUi
    {
        [BoxGroup("Settings")] public string debugTerrainObjectName;
        private DebugTerrain _debugTerrain;

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

        }

        /// <summary>
        /// Handler for enable NR button
        /// </summary>
        public void EnableNatureRendererHandler()
        {
            _debugTerrain.EnableNatureRenderer();
        }

        /// <summary>
        /// Handler for disable NR button
        /// </summary>
        public void DisableNatureRendererHandler()
        {
            _debugTerrain.DisableNatureRenderer();
        }

        /// <summary>
        /// Handle enable shadows button
        /// </summary>
        public void EnableShadowsHandler()
        {
            _debugTerrain.EnableShadows();
        }

        /// <summary>
        /// Handle disable shadows button
        /// </summary>
        public void DisableShadowsHandler()
        {
            _debugTerrain.DisableShadows();
        }

        /// <summary>
        /// Handler for Shadow Distance slider
        /// </summary>
        /// <param name="distance"></param>
        public void SetShadowDistanceHandler(float distance)
        {
            _debugTerrain.SetShadowDistance(distance);
        }

        /// <summary>
        /// Handler for the Detail Distance slider
        /// </summary>
        /// <param name="distance"></param>
        public void SetDetailDistanceHandler(float distance)
        {
            _debugTerrain.SetDetailDistance(distance);
        }

        public void EnableTreesHandler()
        {
            _debugTerrain.EnableTrees();
        }

        public void DisableTreesHandler()
        {
            _debugTerrain.DisableTrees();
        }

        public void EnableDetailsHandler()
        {
            _debugTerrain.EnableDetails();
        }

        public void DisableDetailsHandler()
        {
            _debugTerrain.DisableDetails();
        }
    }
}
