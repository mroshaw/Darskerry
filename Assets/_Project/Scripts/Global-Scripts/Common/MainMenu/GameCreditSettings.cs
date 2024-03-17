using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DaftAppleGames.Common.MainMenu 
{
    public enum CreditAssetType {Models, Audio, VFX, Tools, PlayerAndAI}
    
    /// <summary>
    /// Game Credit Settings
    /// </summary>
    [CreateAssetMenu(fileName = "GameCreditSettings", menuName = "Settings/Game/GameCredits", order = 1)]
    public class GameCreditSettings : ScriptableObject
    {
        // Public fields
        [Header("Game Credits")]
        public CreditDetails[] gameCredits;
        
        [Header("Third Party Assets")]
        public AssetDetails[] thirdPartyAssets;
        
        public AssetDetails[] SortedThirdPartyAssets
        {
            get
            {
                // return thirdPartyAssets;
                Array.Sort(thirdPartyAssets, new AssetDetailComparer());
                return thirdPartyAssets;
            }
        }
        
        /// <summary>
        /// Sort by asset type
        /// </summary>
        public class AssetDetailComparer : IComparer
        {
            public int Compare(object x, object y)
            {
                return (new CaseInsensitiveComparer()).Compare(((AssetDetails)x).assetType, ((AssetDetails)y).assetType);
            }
        }
        
        /// <summary>
        /// Private class for Asset Store package details
        /// </summary>
        [Serializable]
        public class AssetDetails
        {
            public CreditAssetType assetType;
            public string assetName;
            public string assetAuthor;

            public string FriendlyAssetName
            {
                get
                {
                    return GetAssetTypeName(assetType);
                }
            }
            
            /// <summary>
            /// Friendly names for each asset type
            /// </summary>
            private static Dictionary<CreditAssetType, string> _assetTypeLookup = new Dictionary<CreditAssetType, string>
            {
                { CreditAssetType.Models, "3D Models" },
                { CreditAssetType.Audio, "Audio and Effects"},
                { CreditAssetType.VFX, "Visual Effects" },
                { CreditAssetType.Tools, "Tools and World Building"},
                { CreditAssetType.PlayerAndAI, "Player and AI"}
            };
            
            /// <summary>
            /// Returns a friendly name for each asset type
            /// </summary>
            /// <param name="assetType"></param>
            /// <returns></returns>
            public static string GetAssetTypeName(CreditAssetType assetType)
            {
                if (_assetTypeLookup.TryGetValue(assetType, out string assetTypeName))
                {
                    return assetTypeName;
                }
                else
                {
                    return "Other";
                }
            }
        }
       
        /// <summary>
        /// Private class for game developer details
        /// </summary>
        [Serializable]
        public class CreditDetails
        {
            public string role;
            public string name;
        }
    }
    
    
}
