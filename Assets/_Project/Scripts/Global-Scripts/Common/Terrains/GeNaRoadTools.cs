#if GENA_PRO
using System.Collections.Generic;
using UnityEngine;
using GeNa.Core;

namespace DaftAppleGames.Editor
{
    public class GeNaRoadTools : MonoBehaviour
    {
        private string _name;

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public void ApplyRoads()
        {
            GeNaManager genaManager = FindObjectOfType<GeNaManager>();

            foreach(GeNaSpline roadSpline in genaManager.Splines)
            {
                Debug.Log($"Processing Road: {roadSpline.name}");

                roadSpline.gameObject.SetActive(true);

                GeNaCarveExtension carve = roadSpline.GetExtension<GeNaCarveExtension>();
                List<GeNaTerrainExtension> terrains = GetTerrainExtensions(roadSpline);
                GeNaClearDetailsExtension details = roadSpline.GetExtension<GeNaClearDetailsExtension>();
                GeNaClearTreesExtension trees = roadSpline.GetExtension<GeNaClearTreesExtension>();
                GeNaRoadExtension road = roadSpline.GetExtension<GeNaRoadExtension>();

                if (details)
                {
                    Debug.Log("...Clearing Terrain Details...");
                    details.Clear();
                }

                if (trees)
                {
                    Debug.Log("...Clearing Trees...");
                    trees.Clear();
                }

                if (terrains.Count != 0)
                {
                    Debug.Log("...Applying Terrain Changes...");
                    foreach (GeNaTerrainExtension terrain in terrains)
                    {
                        if (terrain.EffectType == EffectType.Texture)
                        {
                            Debug.Log($"... Applying Texture...");
                            terrain.Clear();
                        }
                    }
                }

                roadSpline.gameObject.SetActive(false);

                Debug.Log($"Done Processing Road: {roadSpline.name}");
            }
        }
        
        /// <summary>
        /// Find ALL terrain extensions of given type
        /// </summary>
        /// <param name="spline"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        private List<GeNaTerrainExtension> GetTerrainExtensions(GeNaSpline spline)
        {
            List<GeNaTerrainExtension> extensions = new List<GeNaTerrainExtension>();
            
            foreach (ExtensionEntry extension in spline.Extensions)
            {
                GeNaSplineExtension splineExtension = extension.Extension;
                if(splineExtension.GetType() == typeof(GeNaTerrainExtension))
                {
                    extensions.Add(splineExtension as GeNaTerrainExtension);
                }
            }
            return extensions;
        }
    }
}
#endif