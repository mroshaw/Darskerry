using UnityEngine;

namespace DaftAppleGames.Common.Terrains
{
    [ExecuteInEditMode]
    public class TerrainMaskCollector : MonoBehaviour
    {
        [Header("Settings")]
        public GameObject maskCollectionParent;

        [SerializeField]
        private TerrainMask[] _allMasks;

        /// <summary>
        /// Configure component
        /// </summary>
        private void Start()
        {
            if (Application.isPlaying)
            {
                maskCollectionParent.SetActive(false);
            }
        }
        
        /// <summary>
        /// Updates the Mask Collection Parent with copies of all terrain masks
        /// </summary>
        public void CollectMasks()
        {
            RemoveChildren(maskCollectionParent);
            _allMasks = FindObjectsOfType<TerrainMask>(true);

            foreach (TerrainMask mask in _allMasks)
            {
                GameObject maskCopy = Instantiate(mask.gameObject);
                maskCopy.SetActive(true);
                maskCopy.transform.SetParent(maskCollectionParent.transform);
                maskCopy.transform.position = mask.gameObject.transform.position;
                maskCopy.transform.rotation = mask.gameObject.transform.rotation;
                mask.gameObject.SetActive(false);
            }
        }

        /// <summary>
        /// Destroys all child Game Objects
        /// </summary>
        /// <param name="parent"></param>
        private void RemoveChildren(GameObject parent)
        {
            foreach (Transform child in parent.transform) {
                DestroyImmediate(child.gameObject);
            }
        }

        public void EnableAllMasks()
        {
            _allMasks = FindObjectsOfType<TerrainMask>(true);

            foreach (TerrainMask mask in _allMasks)
            {
                mask.gameObject.SetActive(true);
            }
        }

        public void DisableAllMasks()
        {
            _allMasks = FindObjectsOfType<TerrainMask>(true);

            foreach (TerrainMask mask in _allMasks)
            {
                mask.gameObject.SetActive(false);
            }
            
        }
    }
}