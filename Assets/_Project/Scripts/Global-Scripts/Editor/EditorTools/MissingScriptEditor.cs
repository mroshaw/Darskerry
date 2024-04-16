using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DaftAppleGames.Editor.ProjectTools
{
    public class MissingScriptEditor : UnityEditor.Editor
    {

        [MenuItem("Daft Apple Games/Tools/Editor/List objects with missing scripts")]
        static void ListGameObjectsWithMissingScripts()
        {
            FindMissingScriptsInAllScenes(false, false);
        }

        [MenuItem("Daft Apple Games/Tools/Editor/Select objects with missing scripts")]
        static void SelectGameObjectsWithMissingScripts()
        {
            FindMissingScriptsInAllScenes(false, true);
        }

        [MenuItem("Daft Apple Games/Tools/Editor/Delete missing scripts")]
        static void DeleteMissingScripts()
        {
            FindMissingScriptsInAllScenes(true, false);
        }

        /// <summary>
        /// Process all open scenes
        /// </summary>
        /// <param name="delete"></param>
        /// <param name="select"></param>
        static void FindMissingScriptsInAllScenes(bool delete = false, bool select = false)
        {
            List<GameObject> objectsMissingScripts = new List<GameObject>();

            for (int currSceneIndex = 0; currSceneIndex < SceneManager.loadedSceneCount; currSceneIndex++)
            {
                // Debug.Log($"... searching in {SceneManager.GetSceneAt(currSceneIndex).name}");
                objectsMissingScripts.AddRange(FindMissingScriptsInScene(SceneManager.GetSceneAt(currSceneIndex)));
            }

            // Process the results
            if (objectsMissingScripts.Count == 0)
            {
                Debug.Log("No GameObjects in currently open scenes have missing scripts!");
                return;
            }

            foreach (GameObject currObject in objectsMissingScripts)
            {
                Debug.Log($"Missing script found on: {currObject.name}");
                if (delete)
                {
                    GameObjectUtility.RemoveMonoBehavioursWithMissingScript(currObject);
                    Debug.Log($"Missing script deleted from: {currObject.name}");
                }
            }

            if (select)
            {
                Selection.objects = objectsMissingScripts.ToArray();
            }
        }

        /// <summary>
        /// Select GameObjects with missing scripts within the given scene
        /// </summary>
        /// <param name="scene"></param>
        static List<GameObject> FindMissingScriptsInScene(Scene scene)
        {
            //Get the current scene and all top-level GameObjects in the scene hierarchy
            GameObject[] rootObjects = scene.GetRootGameObjects();

            List<GameObject> objectsWithMissingScripts = new List<GameObject>();
            foreach (GameObject currGameObject in rootObjects)
            {
                objectsWithMissingScripts.AddRange(FindMissingScriptsRecursive(currGameObject));
            }

            return objectsWithMissingScripts;
        }

        /// <summary>
        /// Find all missing scripts on given root object
        /// </summary>
        /// <param name="rootObject"></param>
        /// <returns></returns>
        private static List<GameObject> FindMissingScriptsRecursive(GameObject rootObject)
        {
            // Debug.Log($"... searching in {rootObject.name}");
            List<GameObject> currResults = new List<GameObject>();
            // list missing on this object
            if (GameObjectUtility.GetMonoBehavioursWithMissingScriptCount(rootObject) != 0)
            {
                currResults.Add(rootObject);
            }

            // Scan children
            foreach (Transform transform in rootObject.transform)
            {
                currResults.AddRange(FindMissingScriptsRecursive(transform.gameObject));
            }

            return currResults;
        }
    }
}