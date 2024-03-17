using Sirenix.OdinInspector;
using UnityEngine;

namespace DaftAppleGames.Common.CameraTools
{
    public class ShadowLayerCulling : MonoBehaviour
    {
        [Header("Light Layer Shadow Culling Distances")]
        [InlineEditor()]
        public ShadowLayerCullingSettings cullingSettings;
    }
}
