#if AMPLIFY
using AmplifyImpostors;
using UnityEditor;
using UnityEngine;

namespace DaftAppleGames.Editor.Common.Buildings
{
    public class ImposterTools
    {
        public static void ConfigureImposter(GameObject buildingGameObject, string imposterPath)
        {
            UpdateImposter(buildingGameObject, imposterPath);
            UpdateLodGroup(buildingGameObject);
        }

        public static void UpdateImposter(GameObject buildingGameObject, string imposterPath)
        {
            AmplifyImpostor imposter = buildingGameObject.GetComponent<AmplifyImpostor>();
            if (!imposter)
            {
                imposter = buildingGameObject.AddComponent<AmplifyImpostor>();
            }

            imposter.LodGroup = buildingGameObject.GetComponent<LODGroup>();

            
            string folderPath = imposterPath;
            string fileName = buildingGameObject.name + "_Imposter";
            // imposter.m_folderPath = imposterPath;
            // imposter.m_impostorName = buildingGameObject.name + "_Imposter";
            
            AmplifyImpostorAsset existingAsset = AssetDatabase.LoadAssetAtPath<AmplifyImpostorAsset>( folderPath + fileName + ".asset" );
            if( existingAsset != null )
            {
                imposter.Data = existingAsset;
            }
            else
            {
                imposter.Data = ScriptableObject.CreateInstance<AmplifyImpostorAsset>();
                AssetDatabase.CreateAsset( imposter.Data, folderPath + fileName + ".asset" );
            }
            
            imposter.GenerateAutomaticMesh(imposter.Data);
        }
        
        public static void UpdateLodGroup(GameObject buildingGameObject)
        {
            LODGroup lodGroup = buildingGameObject.GetComponent<LODGroup>();
            if (!lodGroup)
            {
                lodGroup = buildingGameObject.AddComponent<LODGroup>();
            }
            LOD[] lods = new LOD[2];
            lods[0].screenRelativeTransitionHeight = 0.9f;
            lods[1].screenRelativeTransitionHeight = 0.1f;
            Renderer[] allRenderers = buildingGameObject.GetComponentsInChildren<Renderer>(true);
            
            lods[0].renderers = allRenderers;
            
            lodGroup.SetLODs(lods);
            lodGroup.RecalculateBounds();
        }
    }
}
#endif
