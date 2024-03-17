using UnityEngine;
using Sirenix.OdinInspector;

namespace DaftAppleGames.Common.Buildings
{

    public class BuildingReflectionProbe : MonoBehaviour
    {
        // Public serializable properties
        [BoxGroup("General Settings")] public BuildingLightLocation lightLocation;

        public ReflectionProbe ReflectionProbe => GetComponent<ReflectionProbe>();

        /// <summary>
        /// Turn on the light
        /// </summary>
        ///
        [Button("Refresh")]
        public void Refresh()
        {
            ReflectionProbe.RenderProbe();
        }
    }
}
