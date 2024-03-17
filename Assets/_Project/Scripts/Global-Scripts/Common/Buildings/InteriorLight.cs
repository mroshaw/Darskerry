using System.Collections.Generic;
using UnityEngine;
#if HDPipeline
using UnityEngine.Rendering.HighDefinition;
#endif

namespace DaftAppleGames.Common.Buildings
{
    public class InteriorLight : MonoBehaviour
    {
        [Header("Light Configuration")]
        public List<Light> lights = new();
        public float radius = 0.025f;
        public float range = 1.0f;
        public float intensity = 30.0f;

        [SerializeField]
        private InteriorLightController _interiorLightController;

        /// <summary>
        /// Configure the Light based on settings
        /// </summary>
        public virtual void Start()
        {
            ConfigureLights();

            if(_interiorLightController)
            {
                _interiorLightController.RegisterLight(this);
            }
        }

        /// <summary>
        /// Set the Controller
        /// </summary>
        /// <param name="controller"></param>
        internal void SetLightController(InteriorLightController controller)
        {
            _interiorLightController = controller;
        }

        /// <summary>
        /// Clear the light list
        /// </summary>
        public void ClearLights()
        {
            lights.Clear();
        }

        /// <summary>
        /// Turn on the Light
        /// </summary>
        public virtual void TurnOnLight()
        {
            foreach(Light light in lights)
            {
                light.enabled = true;
            }
        }

        /// <summary>
        /// Turn off the Light
        /// </summary>
        public virtual void TurnOffLight()
        {
            foreach (Light light in lights)
            {
                light.enabled = false;
            }
        }

        /// <summary>
        /// Toggle light from current state
        /// </summary>
        public virtual void ToggleLight()
        {
            foreach (Light light in lights)
            {
                light.enabled = !light.enabled;
            }
        }

        /// <summary>
        /// Configure all Lights in the GameObject
        /// </summary>
        private void ConfigureLights()
        {
            foreach(Light light in lights)
            {
                ConfigureLight(light);
            }
        }

        /// <summary>
        /// Configure individual Light
        /// </summary>
        /// <param name="light"></param>
        private void ConfigureLight(Light light)
        {
#if HDPipeline
            HDAdditionalLightData hdLight = light.GetComponent<HDAdditionalLightData>();
            hdLight.intensity = intensity;
            hdLight.range = range;
            hdLight.shapeRadius = radius;
#endif
        }
    }
}