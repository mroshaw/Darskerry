#if GENA_PRO
using UnityEngine;
using UnityEditor;

#if UNITY_EDITOR

namespace DaftAppleGames.Editor
{
    public class GaiaMeshExport : MonoBehaviour
    {
        public void ExportMesh()
        {
            MeshFilter meshFilter = GetComponent<MeshFilter>();
            if(!meshFilter)
            {
                Debug.Log("Cannot find MeshFilter!");
                return;
            }
            Mesh mesh = meshFilter.sharedMesh;
            if(!mesh)
            {
                Debug.Log("Cannot find Mesh!");
                return;
            }
            AssetDatabase.CreateAsset( mesh, "Assets/GaiaMeshExport.asset");
            AssetDatabase.SaveAssets();
        }
    }
}
#endif
#endif