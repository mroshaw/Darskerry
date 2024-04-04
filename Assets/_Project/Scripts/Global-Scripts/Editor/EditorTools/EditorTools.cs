using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace DaftAppleGames.Editor
{
    public class EditorTools : MonoBehaviour
    {
        [Serializable]
        public class EditorSearchCriteria
        {
            public string[] ParentGameObjectNames;
            public string[] ParentGameObjectLayers;
            public string[] ParentGameObjectTags;
            public string[] ComponentNames;

            // Default constructor
            public EditorSearchCriteria(string[] parentGameObjectNames, string[] parentGameObjectLayers,
                string[] parentGameObjectTags, string[] componentNames)
            {
                ParentGameObjectNames = parentGameObjectNames;
                ParentGameObjectLayers = parentGameObjectLayers;
                ParentGameObjectTags = parentGameObjectTags;
                ComponentNames = componentNames;
            }
        }
        
        /// <summary>
        /// Forces a save of any modified prefabs
        /// </summary>
        public static void ForcePrefabSave()
        {
            var prefabStage = PrefabStageUtility.GetCurrentPrefabStage();
            if (prefabStage != null)
            {
                EditorSceneManager.MarkSceneDirty(prefabStage.scene);
            }
        }
        
        public static GameObject[] GetAllSelectedGameObjects()
        {
            return Selection.gameObjects;
        }
        
        public static Collider[] FindCollidersInGameObjects(GameObject[] gameObjects, bool includeInactive,
            EditorSearchCriteria searchCriteria)
        {
            Collider[] allCollidersInGameObjects = new Collider[0];
            
            foreach (GameObject childGameObject in gameObjects)
            {
                Collider[] allColliders = childGameObject.GetComponentsInChildren<Collider>(includeInactive);
                allCollidersInGameObjects = allCollidersInGameObjects.Concat(allColliders).ToArray();
            }

            return allCollidersInGameObjects;
        }

    }
}
