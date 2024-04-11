using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

namespace DaftAppleGames.Editor.ProjectTools
{    public class MissingScriptEditor : UnityEditor.Editor
    {
        [MenuItem("Daft Apple Games/Tools/Editor/Select objects with missing scripts")]
        static void SelectGameObjectsWithMissingScripts()
        {
            //Get the current scene and all top-level GameObjects in the scene hierarchy
            Scene currentScene = SceneManager.GetActiveScene();
            GameObject[] rootObjects = currentScene.GetRootGameObjects();

            List<Object> objectsWithDeadLinks = new List<Object>();
            foreach (GameObject g in rootObjects)
            {
                //Get all components on the GameObject, then loop through them
                Component[] components = g.GetComponents<Component>();
                for (int i = 0; i < components.Length; i++)
                {
                    Component currentComponent = components[i];

                    //If the component is null, that means it's a missing script!
                    if (currentComponent == null)
                    {
                        //Add the sinner to our naughty-list
                        objectsWithDeadLinks.Add(g);
                        Selection.activeGameObject = g;
                        Debug.Log(g + " has a missing script!");
                        break;
                    }
                }
            }

            if (objectsWithDeadLinks.Count > 0)
            {
                //Set the selection in the editor
                Selection.objects = objectsWithDeadLinks.ToArray();
            }
            else
            {
                Debug.Log("No GameObjects in '" + currentScene.name + "' have missing scripts! Yay!");
            }
        }

        [MenuItem("Daft Apple Games/Tools/Editor/Remove Missing Scripts", true, 0)]
        private static bool RemoveMissingScripts_OnPrefabs_Validate()
        {
            return Selection.objects != null && Selection.objects.All(x => x.GetType() == typeof(GameObject));
        }

        [MenuItem("Daft Apple Games/Tools/Editor/Remove Missing Scripts", false, 0)]
        private static void RemoveMissingScripts_OnPrefabs()
        {
            foreach (var obj in Selection.gameObjects)
            {
                RemoveMissingScripts_OnPrefabs_Recursive(obj);
            }
        }

        private static void RemoveMissingScripts_OnPrefabs_Recursive(GameObject obj)
        {
            // list missing on this object
            if (GameObjectUtility.RemoveMonoBehavioursWithMissingScript(obj) != 0)
            {
                Debug.Log($"REMOVED: Missing Scripts on object '{obj.name}'");
            }

            // scan childeren
            foreach (Transform transform in obj.transform)
            {
                RemoveMissingScripts_OnPrefabs_Recursive(transform.gameObject);
            }
        }

        [MenuItem("Daft Apple Games/Tools/Editor/List Missing Scripts", true, 0)]
        private static bool ListMissingScripts_OnPrefabs_Validate()
        {
            return Selection.objects != null && Selection.objects.All(x => x.GetType() == typeof(GameObject));
        }

        [MenuItem("Daft Apple Games/Tools/Editor/List Missing Scripts", false, 0)]
        private static void ListMissingScripts_OnPrefabs()
        {
            foreach (var obj in Selection.gameObjects)
            {
                ListMissingScripts_OnPrefabs_Recursive(obj);
            }
        }

        private static void ListMissingScripts_OnPrefabs_Recursive(GameObject obj)
        {
            // list missing on this object
            if (GameObjectUtility.GetMonoBehavioursWithMissingScriptCount(obj) != 0)
            {
                Debug.Log($"Missing Script on object '{obj.name}'");
            }

            // scan childeren
            foreach (Transform transform in obj.transform)
            {
                ListMissingScripts_OnPrefabs_Recursive(transform.gameObject);
            }
        }
    }
}