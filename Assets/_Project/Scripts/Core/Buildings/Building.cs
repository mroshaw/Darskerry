using Sirenix.OdinInspector;
using UnityEngine;

namespace DaftAppleGames.Darskerry.Core.Buildings
{
    public class Building : MonoBehaviour
    {
        [BoxGroup("Meshes")] [SerializeField] public GameObject[] interiors;

        [BoxGroup("Meshes")] [SerializeField] public GameObject[] exteriors;

        [BoxGroup("Meshes")] [SerializeField] public GameObject[] interiorProps;

        [BoxGroup("Meshes")] [SerializeField] public GameObject[] exteriorProps;
    }
}