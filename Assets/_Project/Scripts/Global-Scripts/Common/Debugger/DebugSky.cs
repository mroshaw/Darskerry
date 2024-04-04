using System;
using UnityEngine;
using Sirenix.OdinInspector;

namespace DaftAppleGames.Common.Debugger
{
    public enum DebugSkyOptions { None, Simple, Default }

    public class DebugSky : DebugBase
    {
        [BoxGroup("Sky Settings")] public GameObject expanseRootGameObject;

        /// <summary>
        /// Set the Sky detail level
        /// </summary>
        /// <param name="level"></param>
        public void ApplyOption(DebugSkyOptions option)
        {
            Debug.Log($"DebugSky: Applying option: {option.ToString()}");
        }
    }
}
