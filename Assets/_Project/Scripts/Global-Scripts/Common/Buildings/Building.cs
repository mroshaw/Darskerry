using Sirenix.OdinInspector;
using UnityEngine;

namespace DaftAppleGames.Common.Buildings 
{
    public class Building : Structure
    {
        [BoxGroup("Custom Components")]
        public GameObject customComponents;

        [BoxGroup("Meshes")]
        public GameObject[] interiors;
        [BoxGroup("Meshes")]
        public GameObject[] exteriors;
        [BoxGroup("Meshes")]
        public GameObject[] interiorProps;
        [BoxGroup("Meshes")]
        public GameObject[] exteriorProps;
    }
}
