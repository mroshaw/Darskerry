using DaftAppleGames.Extensions;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;

namespace DaftAppleGames.Darskerry.Editor.ObjectTools
{
    public class TerrainAlignEditorWindow : OdinEditorWindow
    {
        [BoxGroup("Settings")] public bool alignPosition = true;
        [BoxGroup("Settings")] public bool alignRotation = true;
        [BoxGroup("Settings")] public bool alignX = true;
        [BoxGroup("Settings")] public bool alignY = true;
        [BoxGroup("Settings")] public bool alignZ = true;

        [SerializeField]
        [BoxGroup("Selected Objects")] private GameObject[] selectedGameObjects;

        [MenuItem("Daft Apple Games/Terrains/Object Aligner")]
        public static void ShowWindow()
        {
            EditorWindow editorWindow = GetWindow(typeof(TerrainAlignEditorWindow));
            editorWindow.titleContent = new GUIContent("Terrain Aligner");
            editorWindow.Show();
        }

        /// <summary>
        /// Update the list of selected objects
        /// </summary>
        private void OnSelectionChange()
        {
            selectedGameObjects = Selection.gameObjects;
        }

        [Button("Align selected")]
        private void AlignSelected()
        {
            foreach (GameObject currGameObject in Selection.gameObjects)
            {
                AlignGameObject(currGameObject);
            }
        }

        [Button("Align children of selected")]
        private void AlignChildrenOfSelected()
        {
            foreach (GameObject currGameObject in Selection.gameObjects)
            {
                for (int currChildIndex = 0; currChildIndex < currGameObject.transform.childCount; currChildIndex++)
                {
                    AlignGameObject(currGameObject.transform.GetChild(currChildIndex).gameObject);
                }
            }
        }

        /// <summary>
        /// Align the selected GameObject to the terrain
        /// </summary>
        /// <param name="targetGameObject"></param>
        private void AlignGameObject(GameObject targetGameObject)
        {
            Terrain.activeTerrain.AlignObject(targetGameObject, alignPosition, alignRotation, alignX, alignY, alignZ);
        }
    }
}