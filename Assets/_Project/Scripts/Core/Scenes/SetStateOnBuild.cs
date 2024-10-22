using UnityEngine;

namespace DaftAppleGames.Darskerry.Core.Scenes
{
    public class SetStateOnBuild : MonoBehaviour, IBuildApplier
    {
        [SerializeField] private bool buildState;

        private bool _currentState;

        public void BuildStart()
        {
            _currentState = gameObject.activeSelf;
            gameObject.SetActive(buildState);
        }

        public void BuildFinished()
        {
            gameObject.SetActive(_currentState);
        }
    }
}