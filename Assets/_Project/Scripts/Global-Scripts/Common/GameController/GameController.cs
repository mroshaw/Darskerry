using Sirenix.OdinInspector;
using UnityEngine;

namespace DaftAppleGames.Common.GameControllers
{
    public enum CharSelection { Callum, Emily }

    public class GameController : MonoBehaviour
    {
        [BoxGroup("Character Settings")] [SerializeField] private CharSelection _selectedCharacter;
        [BoxGroup("Save Settings")] [SerializeField] private bool _isLoadingFromSave;
        [BoxGroup("Save Settings")] [SerializeField] private int _loadSlot;
        
        // Public properties
        public CharSelection SelectedCharacter { get => _selectedCharacter; set => _selectedCharacter = value; }

        public bool IsLoadingFromSave
        {
            get => _isLoadingFromSave;
            set => _isLoadingFromSave = value;
        }
        public int LoadSlot
        {
            get => _loadSlot;
            set => _loadSlot = value;
        }
        // Singleton static instance
        private static GameController _instance;
        public static GameController Instance => _instance;

        /// <summary>
        /// Initialise the GameController Singleton instance
        /// </summary>
        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(this.gameObject);
            }
            else
            {
                _instance = this;
            }
        }
    }
}