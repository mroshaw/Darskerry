using Sirenix.OdinInspector;
using UnityEngine;

namespace DaftAppleGames.Darskerry.Core
{
    [ExecuteInEditMode]
    public class ObjectGuid : MonoBehaviour
    {
        [SerializeField] private string guid;
        private string _guid;

        public string Guid => _guid;

        private void OnEnable()
        {
            Generate();
        }

        [Button("Generate Now")]
        private void Generate()
        {
            if (string.IsNullOrEmpty(_guid))
            {
                _guid = System.Guid.NewGuid().ToString();
                guid = _guid;
            }

        }
    }
}