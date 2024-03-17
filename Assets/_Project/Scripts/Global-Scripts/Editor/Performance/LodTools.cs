using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DaftAppleGames.Editor.Common.Performance
{
    public class LodTools : MonoBehaviour
    {
        /// <summary>
        /// Creates a LODGroup and configures it based on the passed parameters
        /// </summary>
        /// <param name="parentGameObject"></param>
        /// <param name="editorSettings"></param>
        public static void UpdateLodGroup(GameObject parentGameObject, LodEditorSettings editorSettings)
        {
            // First up, create or update a LodGroup component on parent
            LODGroup lodGroup = parentGameObject.GetComponent<LODGroup>();
            if (!lodGroup)
            {
                lodGroup = parentGameObject.AddComponent<LODGroup>();
            }

            int numLodGroups = editorSettings.lodGroupSettings.Count;
            LOD[] lods = new LOD[numLodGroups];
            
            // Set up LOD Groups
            
            for(int currentGroup=0; currentGroup<editorSettings.lodGroupSettings.Count; currentGroup++)
            {
                lods[currentGroup] = new LOD(editorSettings.lodGroupSettings[currentGroup].lodRelativeHeight,
                    FindRenderers(parentGameObject, editorSettings.lodGroupSettings[currentGroup]));
            }
            lodGroup.SetLODs(lods);
            lodGroup.RecalculateBounds();
        }

        /// <summary>
        /// Find all renderers with matching criteria in parent Game Object and children
        /// </summary>
        /// <param name="parentGameObject"></param>
        /// <param name="setting"></param>
        /// <returns></returns>
        private static Renderer[] FindRenderers(GameObject parentGameObject, LodGroupSetting setting)
        {
            // List of renderers
            List<Renderer> lodRenderers = new();
            
            // Iterate over child Game Objects
            Transform[] allChildren = parentGameObject.GetComponentsInChildren<Transform>(true);
            foreach (Transform child in allChildren)
            {
                // If we encounter a child Go matching our search string
                if (setting.lodSearchStrings.Any(child.gameObject.name.Contains))
                {
                    // Add all the child renderers to the LodGroup
                    Renderer[] allRenderers = child.gameObject.GetComponentsInChildren<Renderer>();
                    foreach (Renderer rendererItem in allRenderers)
                    {
                        lodRenderers.Add(rendererItem);
                    }
                }
            }

            return lodRenderers.ToArray();
        }

        public static void RefreshLodOne(GameObject parentGameObject)
        {
            LODGroup lodGroup = parentGameObject.GetComponent<LODGroup>();
            LOD[] lods = lodGroup.GetLODs();

            GameObject exterior = parentGameObject.transform.Find("Interiors").gameObject;
            GameObject interior = parentGameObject.transform.Find("Exteriors").gameObject;
            GameObject props = parentGameObject.transform.Find("Props").gameObject;
            
            Renderer[] exteriorRenderers = exterior.gameObject.GetComponentsInChildren<Renderer>();
            Renderer[] interiorRenderers = interior.gameObject.GetComponentsInChildren<Renderer>();
            Renderer[] propsRenderers = props.gameObject.GetComponentsInChildren<Renderer>();
            
            // Add all the child renderers to the LodGroup
            Renderer[] allRenderers = exteriorRenderers.Concat(interiorRenderers).Concat(propsRenderers).ToArray();
            lods[0].renderers = allRenderers;

            lodGroup.SetLODs(lods);
            
        }
        
        private static void SetupLodGroup(GameObject parentGameObject, int groupNum, LodGroupSetting setting)
        {

        }
        
        private void DoStuff()
        {
            
        }
    }
}
