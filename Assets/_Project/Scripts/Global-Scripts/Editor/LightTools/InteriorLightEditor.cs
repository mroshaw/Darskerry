using UnityEditor;
using UnityEngine;
using DaftAppleGames.Common.Buildings;

[CustomEditor(typeof(InteriorLight), true)]
public class InteriorLightEditor : Editor
{
    override public void OnInspectorGUI()
    {
        DrawDefaultInspector();
        InteriorLight myScript = target as InteriorLight;
        if (GUILayout.Button("Light On"))
        {
            myScript.TurnOnLight();
        }

        if (GUILayout.Button("Light Off"))
        {
            myScript.TurnOffLight();
        }

        if (GUILayout.Button("(Re)Configure"))
        {
            Configure();
        }
    }

    private void Configure()
    {

    }
}
