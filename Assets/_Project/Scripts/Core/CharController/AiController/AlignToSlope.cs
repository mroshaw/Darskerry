using DaftAppleGames.Darskerry.Core.Extensions;
using UnityEngine;
using Sirenix.OdinInspector;

namespace DaftAppleGames.Darskerry.Core.CharController.AiController
{
    public class AlignToSlope : MonoBehaviour
    {
        #region Class Variables

        [BoxGroup("Settings")] [SerializeField] private bool alignX = false;
        [BoxGroup("Settings")] [SerializeField] private bool alignY = true;
        [BoxGroup("Settings")] [SerializeField] private bool alignZ = true;
        [BoxGroup("Settings")] [SerializeField] private int alignEveryFrames = 5;
        #endregion
        #region Update Logic
        private void Update()
        {
            if (Time.frameCount % alignEveryFrames == 0)
            {
                Align();
            }
        }

        private void LateUpdate()
        {
            
        }
        #endregion
        #region Class Methods

        private void Align()
        {
            Terrain.activeTerrain.AlignObject(gameObject, false, true, alignX, alignY, alignZ);
        }
        #endregion
    }
}