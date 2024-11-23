using UnityEngine;
using UnityEditor;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEngine.Rendering.HighDefinition;

public class ConvertMaterialsToBetterLitWindow : OdinEditorWindow
{
    public string folderPath = "Assets/Art/Buildings/3DForge"; // Default path; modify as needed.

    [MenuItem("Daft Apple Games/Materials/Better Lit Converter")]
    private static void ShowWindow()
    {
        GetWindow<ConvertMaterialsToBetterLitWindow>("Replace HDRP Lit Shader");
    }

    [Button("Replace Shaders")]
    private void ReplaceShaders()
    {
        ReplaceShadersInFolder(folderPath);
    }

    [Button("Set Texture Swizzle")]
    private void UpdateTextureSwizzle()
    {
        SetAlphaSwizzleInFolder(folderPath);
    }

    private static void ReplaceShadersInFolder(string folderPath)
    {
        Shader targetShader = Shader.Find("Better Lit/Lit");
        Shader originalShader = Shader.Find("HDRP/Lit");

        if (targetShader == null || originalShader == null)
        {
            Debug.LogError("Target shader or original shader not found. Make sure 'Better Lit/Lit' and 'HDRP/Lit' exist.");
            return;
        }

        string[] materialGUIDs = AssetDatabase.FindAssets("t:Material", new[] { folderPath });

        foreach (string guid in materialGUIDs)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(guid);
            Material material = AssetDatabase.LoadAssetAtPath<Material>(assetPath);

            if (material != null && material.shader == originalShader)
            {
                material.shader = targetShader;
                Debug.Log($"Shader replaced in material: {assetPath}");
            }

            HDMaterial.ValidateMaterial(material);
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log("Shader replacement complete.");
    }

    private static void SetAlphaSwizzleInFolder(string folderPath)
    {
        string[] textureGUIDs = AssetDatabase.FindAssets("t:Texture", new[] { folderPath });

        foreach (string guid in textureGUIDs)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(guid);
            TextureImporter textureImporter = AssetImporter.GetAtPath(assetPath) as TextureImporter;

            if (textureImporter != null && assetPath.Contains("Mask"))
            {
                // Modify the alpha swizzle to "1-A"
                // textureImporter.alphaSource = TextureImporterAlphaSource.FromInput;
                // textureImporter.alphaIsTransparency = true; // Enable transparency on alpha
                // textureImporter.userData = "SwizzleAlpha:1-A"; // Metadata (not directly supported)
                textureImporter.swizzleR = TextureImporterSwizzle.R;
                textureImporter.swizzleG = TextureImporterSwizzle.G;
                textureImporter.swizzleB = TextureImporterSwizzle.B;
                textureImporter.swizzleA = TextureImporterSwizzle.OneMinusA;
                Debug.Log($"Alpha swizzle set to '1-A' for texture: {assetPath}");

                AssetDatabase.ImportAsset(assetPath, ImportAssetOptions.ForceUpdate);
            }
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log("Alpha swizzle setting complete.");
    }

    private Texture2D ConvertRoughnessToSmoothness(Texture2D roughnessMap)
    {
        Texture2D smoothnessMap = new(roughnessMap.width, roughnessMap.height);
        Color[] pixels = roughnessMap.GetPixels();

        for (int i = 0; i < pixels.Length; i++)
        {
            // Invert the color: black (rough) becomes white (smooth), white becomes black
            pixels[i].r = 1f - pixels[i].r;
            pixels[i].g = 1f - pixels[i].g;
            pixels[i].b = 1f - pixels[i].b;
            // Set alpha to 1 as we're assuming the roughness map doesn't contain transparency
            pixels[i].a = 1f;
        }

        smoothnessMap.SetPixels(pixels);
        smoothnessMap.Apply();

        return smoothnessMap;
    }
}