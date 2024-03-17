using MalbersAnimations.HAP;
using UnityEngine;
using Sirenix.OdinInspector;

namespace DaftAppleGames.Common.Debugger
{
    public class HorseDebugger : MonoBehaviour
    {
        [BoxGroup("Debug")] public bool canCallAnimal;

        private MRider _rider;

        private void Start()
        {
            _rider = GetComponent<MRider>();
        }

        private void Update()
        {
            canCallAnimal = _rider.CanCallAnimal;
        }
    }
}
