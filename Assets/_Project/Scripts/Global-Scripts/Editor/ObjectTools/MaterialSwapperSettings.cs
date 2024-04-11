using System;
using UnityEngine;
using Sirenix.OdinInspector;

namespace DaftAppleGames.Editor.Meshes
{
    /// <summary>
    /// Scriptable Object: TODO Purpose and Summary
    /// </summary>
    [CreateAssetMenu(fileName = "MaterialSwapperSettings", menuName = "Meshes/Material Swapper Settings", order = 1)]
    [ExecuteInEditMode]
    public class MaterialSwapperSettings : ScriptableObject
    {
        [BoxGroup("Settings")]
        public MaterialPair[] materialPairs;

        /// <summary>
        /// Swap the two arrays around
        /// </summary>
        [Button("Swap Source and Target")]
        private void SwapSourceAndTarget()
        {
            foreach (MaterialPair materialPair in materialPairs)
            {
                (materialPair.sourceMaterial, materialPair.targetMaterial) = (materialPair.targetMaterial, materialPair.sourceMaterial);
            }
        }

        /// <summary>
        /// Return all of the "source" materials
        /// </summary>
        /// <returns></returns>
        public void GetMaterials(out Material[] sourceMaterials, out Material[] targetMaterials, out string[] sourceMaterialNames, out string[] targetMaterialNames)
        {
            targetMaterials = new Material[materialPairs.Length];
            sourceMaterials = new Material[materialPairs.Length];
            sourceMaterialNames = new string[materialPairs.Length];
            targetMaterialNames = new string[materialPairs.Length];

            int count = 0;
            foreach (MaterialPair materialPair in materialPairs)
            {
                sourceMaterials[count] = materialPair.sourceMaterial;
                targetMaterials[count] = materialPair.targetMaterial;
                sourceMaterialNames[count] = materialPair.sourceMaterial.name;
                targetMaterialNames[count] = materialPair.targetMaterial.name;
            }
        }

        [Serializable]
        public class MaterialPair
        {
            public Material sourceMaterial;
            public Material targetMaterial;
        }
    }
}
