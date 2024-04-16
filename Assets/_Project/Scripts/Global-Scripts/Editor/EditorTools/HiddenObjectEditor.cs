using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DaftAppleGames.Editor.ProjectTools
{
    public class HiddenObjectEditor : UnityEditor.Editor
    {
        /// <summary>
        /// Go through all open scenes and show any hidden Game Objects
        /// </summary>
        [MenuItem("Daft Apple Games/Tools/Project/Show hidden objects")]
        private static void ShowHiddenGameObjects()
        {
            var scene = SceneManager.GetActiveScene();
            foreach(var gameObject in scene.GetRootGameObjects())
            {
                ShowHiddenGameObject(gameObject);
            }
        }

        /// <summary>
        /// Finds and shows hidden game objects in the scene
        /// </summary>
        /// <param name="gameObject"></param>
        private static void ShowHiddenGameObject(GameObject gameObject)
        {
            if (gameObject.hideFlags.HasFlag(HideFlags.HideInHierarchy))
            {
                Debug.Log("Revealing hidden GameObject " + gameObject.name, gameObject);
                gameObject.hideFlags &= ~HideFlags.HideInHierarchy;
            }

            foreach (Transform child in gameObject.transform)
            {
                ShowHiddenGameObject(child.gameObject);
            }
        }
    }
}