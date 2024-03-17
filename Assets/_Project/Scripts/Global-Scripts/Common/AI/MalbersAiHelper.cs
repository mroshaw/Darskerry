using MalbersAnimations.Controller;
using MalbersAnimations.Controller.AI;
using UnityEngine;
using UnityEngine.AI;

namespace DaftAppleGames.Common.AI
{
    public class MalbersAiHelper : MonoBehaviour
    {
        public MAnimalAIControl AnimalAiControl => _animalAiControl;
        public MAnimal Animal => _animal;

        public NavMeshAgent NavMeshAgent => _navMeshAgent;

        private MAnimalAIControl _animalAiControl;
        private MAnimal _animal;
        private NavMeshAgent _navMeshAgent;

        private void Awake()
        {
            _animalAiControl = GetComponentInChildren<MAnimalAIControl>(true);
            _animal = GetComponent<MAnimal>();
            _navMeshAgent = GetComponentInChildren<NavMeshAgent>(true);
        }
    }
}
