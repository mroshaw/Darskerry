using System;
using UnityEngine;

namespace DaftAppleGames.Common.Terrains
{
    public class TerrainMask : MonoBehaviour
    {
        [Header("Settings")]
        public TerrainMaskSettings TerrainMaskSettings;
    }

    [Serializable]
    public class TerrainMaskSettings
    {
        public bool noDetail = true;
        public bool noTrees = true;
        public bool dirtTexture = false;
        public bool pavingTexture = false;
    }
}