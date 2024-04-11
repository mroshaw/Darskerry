using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DaftAppleGames.Editor.BuildingTools
{
    /// <summary>
    /// Settings to identify buildings and building elements from a given Asset provider
    /// </summary>
    [CreateAssetMenu(fileName = "BuildingAssetSettings", menuName = "Daft Apple Games/Buildings/Building asset settings", order = 1)]
    public class BuildingAssetSettings : ScriptableObject
    {
        // Public serializable properties
        [BoxGroup("Asset Settings")] public string assetName;
        [BoxGroup("Asset Settings")] public string assetVersion;
        [BoxGroup("Asset Settings")] public GameObjectIdentifier assetBuildingIdentifier;
        [BoxGroup("Part Settings")] public GameObjectIdentifier exteriorIdentifier;
        [BoxGroup("Part Settings")] public GameObjectIdentifier interiorIdentifier;
        [BoxGroup("Props Settings")] public GameObjectIdentifier exteriorPropsIdentifier;
        [BoxGroup("Props Settings")] public GameObjectIdentifier interiorPropsIdentifier;
        [BoxGroup("Lighting Settings")] public GameObjectIdentifier interiorCandleIdentifier;
        [BoxGroup("Lighting Settings")] public GameObjectIdentifier interiorCookingFireIdentifier;
        [BoxGroup("Lighting Settings")] public GameObjectIdentifier interiorFireplaceIdentifier;
        [BoxGroup("Lighting Settings")] public GameObjectIdentifier exteriorTorchIdentifier;
        [BoxGroup("Lighting Settings")] public GameObjectIdentifier exteriorStreetLightIdentifier;
        
        [BoxGroup("Building Dimension Settings")] public GameObjectIdentifier dimensionExcludeIdentifier;

        [BoxGroup("Building Dimension Settings")] public LayerMask dimensionIncludeLayerMask;
       
        [BoxGroup("Alignment Settings")] public float terrainOffset;

        [BoxGroup("Door Settings")] public GameObjectIdentifier doorIdentifier;
        

        // Private fields
        private string _privateField;

        [Serializable]
        public class GameObjectIdentifier
        {
            public IdentifierPattern[] IdentifierPatterns;

            /// <summary>
            /// Retrieves just the Name part of the identifier pattern
            /// </summary>
            /// <returns></returns>
            public string[] GetNamePattern()
            {
                int arrayLength = IdentifierPatterns.Length;
                string[] namePatternArray = new string[arrayLength];
                for(int currItemIndex=0; currItemIndex < arrayLength; currItemIndex++)
                {
                    namePatternArray[currItemIndex] = IdentifierPatterns[currItemIndex].NamePattern;
                }

                return namePatternArray;
            }
        }
        
        /// <summary>
        /// Used to identify a given game object, either by name or by a combination of name and direct parent name
        /// </summary>
        [Serializable]
        public class IdentifierPattern
        {
            public string NamePattern;
            public string ParentNamePattern;
            public bool CheckAllParents = false;
        }
        
        #region UNITY_EVENTS
        /// <summary>
        /// Scriptable Object is enabled
        /// </summary>   
        private void OnEnable()
        {
            
        }
        
        /// <summary>
        /// Scriptable Object is disabled
        /// </summary>   
        private void OnDisable()
        {
            
        }
        /// <summary>
        /// Scriptable Object is destroyed
        /// </summary>   
        private void OnDestroy()
        {
            
        }

        /// <summary>
        /// Configure the component on awake
        /// </summary>   
        private void Awake()
        {
            
        }
        
        /// <summary>
        /// Configure the component on awake
        /// </summary>   
        private void OnValidate()
        {
            
        }
        #endregion
    }
}