using DaftAppleGames.Editor.Buildings;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;

public class MaterialShaderSwapperEditorWindow : OdinEditorWindow
{
    // Display Editor Window
    [MenuItem("Window/Materials/Shader Swapper")]
    public static void ShowWindow()
    {
        GetWindow(typeof(MaterialShaderSwapperEditorWindow));
    }

    /// <summary>
    /// Refresh the selected list when scene object selection changes
    /// </summary>
    private void OnSelectionChange()
    {
        selectedGameObjects = Selection.gameObjects;
    }

    [BoxGroup("Source Objects", centerLabel: true)]
    public GameObject[] selectedGameObjects;

    [BoxGroup("Shaders")]
    public string swapFromShader;
    [BoxGroup("Shaders")]
    public string swapToShader;

    private Shader _swapFromShader;
    private Shader _swapToShader;

    [Button("Swap From and To")]
    private void Swap()
    {
        (swapFromShader, swapToShader) = (swapToShader, swapFromShader);
    }

    [Button("Replace Shaders")]
    public void ReplaceShaders()
    {
        if (!CheckShaders())
        {
            return;
        }

        int counter = 0;
        if (Selection.objects.Length > 0)
        {
            Object[] materials = GetSelectedMaterials();
            if (materials.Length > 0)
            {
                foreach (Material mat in materials)
                {
                    if (mat.shader == _swapFromShader)
                    {
                        Debug.Log($"Swapping Shader on Material: {mat.name}");
                        mat.shader = _swapToShader;
                    }
                    else
                    {
                        Debug.Log($"Skipping Material: {mat.name}");
                    }
                    counter++;
                }
            }
        }
    }

    private bool CheckShaders()
    {
        _swapFromShader = Shader.Find(swapFromShader);
        _swapToShader = Shader.Find(swapToShader);

        return !(_swapFromShader == null || _swapToShader == null);
    }

    static Object[] GetSelectedMaterials()
    {
        return Selection.GetFiltered(typeof(Material), SelectionMode.DeepAssets);
    }
}
