using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace DaftAppleGames.Editor.ProjectSetup
{
    public class SetupMaterialShaderEditor : EditorWindow
    {
        private string shaderToFind = "NatureManufacture/HDRP/Foliage/Foliage";
        private string shaderToReplace = "NatureManufacture/HDRP/Foliage/Foliage Snow";
        private bool fadeEnabled = false;
        private float fadeStart = 1500.0f;
        private float fadeDistance = 50.0f;
        private string foundShadersArea = "Empty List";
        private bool findOnly = true;

        [MenuItem("Window/Project Setup/Set Up Material Shaders")]
        public static void ShowWindow()
        {
            EditorWindow.GetWindow(typeof(SetupMaterialShaderEditor));
        }

        public void OnGUI()
        {
            GUILayout.Label("Find only?");
            findOnly = EditorGUILayout.Toggle(findOnly);

            GUILayout.Label("Enter shader to find:");
            shaderToFind = GUILayout.TextField(shaderToFind);

            GUILayout.Label("Enter shader to replace:");
            shaderToReplace = GUILayout.TextField(shaderToReplace);

            GUILayout.Label("Enable fade??");
            fadeEnabled = EditorGUILayout.Toggle(fadeEnabled);

            GUILayout.Label("Material fade start:");
            fadeStart = EditorGUILayout.FloatField(fadeStart);

            GUILayout.Label("Material fade distance:");
            fadeDistance = EditorGUILayout.FloatField(fadeDistance);

            if (GUILayout.Button("Find Materials"))
            {
                FindShader(shaderToFind);
            }
            GUILayout.Label(foundShadersArea);
        }

        private void FindShader(string shaderName)
        {
            int count = 0;
            foundShadersArea = "Materials using shader " + shaderName + ":\n\n";
            Shader newShader = Shader.Find(shaderToReplace);
            if (!newShader)
            {
                foundShadersArea += $"Can't find shader: {shaderToReplace}";
                return;
            }

            List<Material> armat = new List<Material>();

            Renderer[] arrend = (Renderer[])Resources.FindObjectsOfTypeAll(typeof(Renderer));
            foreach (Renderer rend in arrend)
            {
                foreach (Material mat in rend.sharedMaterials)
                {
                    if (!armat.Contains(mat))
                    {
                        armat.Add(mat);
                    }
                }
            }

            foreach (Material mat in armat)
            {
                if (mat != null && mat.shader != null && mat.shader.name != null && mat.shader.name == shaderName)
                {
                    foundShadersArea += ">" + mat.name + "\n";

                    if (!findOnly)
                    {
                        mat.shader = newShader;
                        mat.SetFloat("_DISTANCEBLEND", fadeEnabled ? 1 : 0);
                        mat.SetFloat("_CullFarStart", fadeStart);
                        mat.SetFloat("_CullFarDistance", fadeDistance);
                    }
                    count++;
                }
            }

            foundShadersArea += "\n" + count + " materials using shader " + shaderName + " found.";
        }
    }
}