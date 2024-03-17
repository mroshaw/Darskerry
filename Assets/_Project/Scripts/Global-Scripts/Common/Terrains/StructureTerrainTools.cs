#if GENA_PRO
using System;
using DaftAppleGames.Common.Buildings;
using GeNa.Core;
using UnityEngine;

namespace DaftAppleGames.Common.Terrains
{
    public enum DecoratorType { ClearDetails, ClearTrees, ClearGameObjects, FlattenTerrain }
    
    [ExecuteInEditMode]
    public class StructureTerrainTools : BaseTerrainTools
    {
        // Public serializable properties
        [Header("Building Settings")]
        public float frontMargin = 1.5f;
        public float rearMargin = 0.5f;
        public float sideMargin = 0.5f;
        
        public bool clearDetail = true;
        public bool clearTrees = true;
        public bool clearGameObject = true;
        public bool flatten = true;
        public Texture2D brushTexture;
        
        [Header("Clear GameObject Settings")]
        public string[] namePatterns;

        [Header("Other Settings")]
        public bool apply = true;
        public bool destroyGameObjects = true;
        
        // Private fields
        private Structure _structure;
#region UNITY_EVENTS
        /// <summary>
        /// Subscribe to events
        /// </summary>   
        private void OnEnable()
        {
            
        }
        
        /// <summary>
        /// Unsubscribe from events
        /// </summary>   
        private void OnDisable()
        {
            
        }

        /// <summary>
        /// Configure the component on awake
        /// </summary>   
        private void Awake()
        {
            
        }
    
        /// <summary>
        /// Configure the component on start
        /// </summary>
        private void Start()
        {
            
        }
#endregion

        /// <summary>
        /// Create and apply all decorators 
        /// </summary>
        public void ApplyDecorators()
        {
            
            _structure = GetComponent<Structure>();
            
            // Create parent Game Object
            GameObject decoratorParentGameObject = new GameObject();
            decoratorParentGameObject.transform.SetParent(this.transform);
            decoratorParentGameObject.transform.localPosition = new Vector3(0, 0, 0);
            decoratorParentGameObject.transform.localRotation = new Quaternion(0, 0, 0, 0);
            decoratorParentGameObject.name = "Decorators";
            
            if (clearDetail)
            {
                CreateDecorator(DecoratorType.ClearDetails, decoratorParentGameObject);                
            }

            if (clearTrees)
            {
                CreateDecorator(DecoratorType.ClearTrees, decoratorParentGameObject);
            }

            if (flatten)
            {
                CreateDecorator(DecoratorType.FlattenTerrain, decoratorParentGameObject);
            }
            
            if (destroyGameObjects)
            {
                Debug.Log($"Destroying {decoratorParentGameObject}...");
                DestroyImmediate(decoratorParentGameObject);
                Debug.Log("Done destroying.");
            }
        }

        public void CreateDecorator(DecoratorType type, GameObject decoratorParentGameObject)
        {
            float decoratorWidth = CalcDecoratorWidth();
            float gameObjectWidth = CalcGameObjectWidth();
            Debug.Log($"Decorator Width: {decoratorWidth}");
            int numDecorators = CalcNumDecorators(gameObjectWidth);
            Debug.Log($"Num Decorators: {numDecorators}");

            
            for (int currDecorator = 1; currDecorator <= numDecorators; currDecorator++)
            {
                GameObject newDecoratorGameObject = CreateGameObject(type, currDecorator, decoratorParentGameObject);
                GeNaTerrainDecorator decorator =  CreateComponents(type, newDecoratorGameObject, decoratorWidth);
                Vector3 newPosition = new Vector3();
                if(currDecorator == 1)
                {
                    newPosition.x = (-rearMargin);
                }
                else if(currDecorator == numDecorators)
                {
                    newPosition.x = (_structure.length - (decoratorWidth / 2) + frontMargin);
                }
                newDecoratorGameObject.transform.localPosition = newPosition;

                if (apply)
                {
                    Debug.Log($"Applying {type}...");
                    decorator.TerrainModifier.ApplyToTerrain();
                    Debug.Log($"Done applying.");
                }

                if (destroyGameObjects)
                {
                    Debug.Log($"Destroying {newDecoratorGameObject}...");
                    DestroyImmediate(newDecoratorGameObject);
                    Debug.Log("Done destroying.");
                }
            }
        }

        /// <summary>
        /// Create the required components, based on the Decorator Type
        /// </summary>
        /// <param name="type"></param>
        /// <param name="gameObject"></param>
        private GeNaTerrainDecorator CreateComponents(DecoratorType type, GameObject gameObject, float decoratorWidth)
        {
            GeNaTerrainDecorator decorator = gameObject.AddComponent<GeNaTerrainDecorator>();
            TerrainModifier modifier = decorator.TerrainModifier;
            modifier.Range = decoratorWidth;
            modifier.NoiseEnabled = true;
            modifier.AddBrushTexture(brushTexture);
            modifier.BrushIndex = 0;
            
            switch (type)
            {
                case DecoratorType.ClearDetails:
                    modifier.EffectType = EffectType.ClearDetails;
                    break;
                case DecoratorType.ClearTrees:
                    modifier.EffectType = EffectType.ClearTrees;
                    break;
                case DecoratorType.ClearGameObjects:
                    break;
                case DecoratorType.FlattenTerrain:
                    modifier.EffectType = EffectType.Flatten;
                    break;
            }
            return decorator;
        }
        
        /// <summary>
        /// Create, name and parent the decorator game objects
        /// </summary>
        /// <param name="type"></param>
        private GameObject CreateGameObject(DecoratorType type, int index, GameObject parentGameObject)
        {
            GameObject newGameObject = new GameObject();
            newGameObject.transform.SetParent(parentGameObject.transform);
            newGameObject.transform.localPosition = new Vector3(0, 0, 0);
            newGameObject.transform.localRotation = new Quaternion(0, 0, 0, 0);
            switch (type)
            {
                case DecoratorType.ClearDetails:
                    newGameObject.name = $"Clear Details {index}";
                    break;
                case DecoratorType.ClearTrees:
                    newGameObject.name = $"Clear Trees {index}";
                    break;
                case DecoratorType.ClearGameObjects:
                    newGameObject.name = $"Clear Game Objects {index}";
                    break;
                case DecoratorType.FlattenTerrain:
                    newGameObject.name = $"Flatten Terrain {index}";
                    break;
            }

            return newGameObject;
        }
        
        public void UpdateDecorators()
        {
            
        }

        private float CalcGameObjectWidth()
        {
            return _structure.width + sideMargin;
        }
        
        /// <summary>
        /// Determine the width of the decorator
        /// </summary>
        /// <returns></returns>
        private float CalcDecoratorWidth()
        {
            return (_structure.width * 3) + sideMargin;
        }

        /// <summary>
        /// Based on number width of square decorator, calc how many to cover the length
        /// </summary>
        /// <param name="decoratorWidth"></param>
        /// <returns></returns>
        private int CalcNumDecorators(float gameObjectWidth)
        {
            return (int)(Math.Ceiling(_structure.length / gameObjectWidth));
        }

        /// <summary>
        /// Determine what offset to move the decorator to, to ensure the entire
        /// length is covered
        /// </summary>
        /// <param name="decoratorNum"></param>
        /// <param name="totalDecorators"></param>
        /// <returns></returns>
        private float CalcDecoratorOffset(int decoratorNum, int totalDecorators)
        {
            return 1.0f;
        }
    }
}
#endif