using Sirenix.OdinInspector;
using UnityEngine;

namespace DaftAppleGames.Common.CameraTools
{
    public class LayerCulling : MonoBehaviour
    {
        [Header("Camera Layer Culling Distances")]
        [InlineEditor()]
        public LayerCullingSettings cullingSettings;
    }
}
