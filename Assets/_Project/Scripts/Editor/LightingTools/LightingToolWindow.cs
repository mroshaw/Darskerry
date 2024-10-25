using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using System.Reflection;
using UnityEditor;

using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

namespace Assets._Project.Scripts.Editor.LightingTools
{
    public class LightingToolWindow : OdinEditorWindow
    {
        [MenuItem("Daft Apple Games/Lighting/Light Baking Tool")]
        public static void ShowWindow()
        {
            GetWindow(typeof(LightingToolWindow));
        }
        
        [Button("Bake Lighting", ButtonSizes.Large), GUIColor(0, 1, 0)]
        private void BigButton()
        {
            BakeLightingWithShadowmap();
        }
        
        [Button("Enable Selected Light")]
        private void EnableSelected()
        {
            GameObject selected = Selection.gameObjects[0];
            Light light = selected.GetComponent<Light>();
            SetShadowMapEnabled(light, true);
        }

        [Button("Disable Selected Light")]
        private void DisableSelected()
        {
            GameObject selected = Selection.gameObjects[0];
            Light light = selected.GetComponent<Light>();
            SetShadowMapEnabled(light, false);
        }


        public static void BakeLightingWithShadowmap()
        {
            // Enable Shadow Maps on all lights
            Light[] lights = FindObjectsByType<Light>(FindObjectsSortMode.None);
            foreach (Light light in lights)
            {
                SetShadowMapEnabled(light, true);
            }

            // Bake Lighting
            Lightmapping.Bake();

            // Disable Shadow Maps after baking
            foreach (Light light in lights)
            {
                SetShadowMapEnabled(light, false);
            }
            Debug.Log("Lighting bake complete and shadow maps disabled.");
        }

        private static void SetShadowMapEnabled(Light light, bool enabled)
        {
            // Check if the light is Mixed or Baked
            if (light.lightmapBakeType == LightmapBakeType.Mixed || light.lightmapBakeType == LightmapBakeType.Baked)
            {
                // Enable shadow map
                HDAdditionalLightData hdLightData = light.GetComponent<HDAdditionalLightData>();
                if (hdLightData != null)
                {
                    hdLightData.EnableShadows(enabled);
                }
            }
        }
    }
}
