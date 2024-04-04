using UnityEditor;
using UnityEngine;
using DaftAppleGames.Common.Buildings;
using Sirenix.OdinInspector.Editor;

[CustomEditor(typeof(InteriorLightController))]
public class InteriorLightControllerEditor : OdinEditor
{
    public InteriorLightController lightController;

    override public void OnInspectorGUI()
    {
        DrawDefaultInspector();
        lightController = target as InteriorLightController;
        if (GUILayout.Button("All Lights On"))
        {
            lightController.TurnOnAllLights();
        }

        if (GUILayout.Button("All Lights Off"))
        {
            lightController.TurnOffAllLights();
        }

        if (GUILayout.Button("All Lights Toggle"))
        {
            lightController.ToggleAllLights();
        }

        if (GUILayout.Button("Register All in Scene"))
        {
            RegisterAllInScene();
        }

        if (GUILayout.Button("(Re)Configure"))
        {
            Configure();
        }
    }

    /// <summary>
    /// Run the Configuration
    /// </summary>
    private void Configure()
    {
    }

    /// <summary>
    /// Run the Configuration
    /// </summary>
    private void RegisterAllInScene()
    {
        lightController.RegisterAllLightsInScene();
    }
}