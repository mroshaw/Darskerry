using System.Linq;
using DaftAppleGames.Editor.Common.Terrains;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;

namespace DaftAppleGames.Editor.Buildings
{
    public class ObjectTerrainAlignerEditor : OdinEditorWindow
    {
        [MenuItem("Window/Buildings/Terrain Aligner")]
        public static void ShowWindow()
        {
            GetWindow(typeof(ObjectTerrainAlignerEditor));
        }
        [BoxGroup("Configuration")]
        [Tooltip("If selected, only the root object will be aligned. Otherwise, all direct children will be aligned.")]
        public bool onlyAlignParent = true;
        [BoxGroup("Search Settings")]
        public string[] nameStringPattern;
        [BoxGroup("Alignment Settings")]
        public bool alignToSlope = true;
        [BoxGroup("Alignment Settings")]
        public bool freezeX;
        [BoxGroup("Alignment Settings")]
        public bool freezeY;
        [BoxGroup("Alignment Settings")]
        public bool freezeZ;
        [BoxGroup("Alignment Settings")]
        public float yOffset = 0.0f;
        
        /// <summary>
        /// Configure button
        /// </summary>
        [Button("Align Objects")]
        [Tooltip("Align objects and children to the terrain")]
        private void AlignObjects()
        {
            if (Selection.gameObjects == null || Selection.gameObjects.Length == 0)
            {
                Debug.Log("Must select at least one root object!");
                return;
            }

            if (onlyAlignParent)
            {
                TerrainTools.AlignObjectsToTerrain(Selection.gameObjects, alignToSlope, freezeX, freezeY, freezeZ);
                return;
            }

            foreach (GameObject parentGameObject in Selection.gameObjects)
            {
                foreach (Transform transform in parentGameObject.transform)
                {
                    if (nameStringPattern.Length == 0 || nameStringPattern.Any(transform.gameObject.name.Contains))
                    {
                        TerrainTools.AlignObjectToTerrain(transform.gameObject, alignToSlope, freezeX, freezeY, freezeZ);
                    }
                }
            }
        }
    }
}
