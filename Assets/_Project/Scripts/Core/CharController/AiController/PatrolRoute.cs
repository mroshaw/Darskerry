using UnityEngine;
using Sirenix.OdinInspector;

namespace DaftAppleGames.Darskerry.Core.CharController.AiController
{
    public class PatrolRoute : MonoBehaviour
    {
        #region Class Variables
        [SerializeField]private PatrolParams patrolParams;

        private int _currentPatrolIndex;
        private int _numberOfPatrolPoints;
        private bool _isValid;
        #endregion

        #region Startup
        private void Start()
        {
            _numberOfPatrolPoints = patrolParams.NumberOfPatrolPoints;
            _isValid = _numberOfPatrolPoints > 0;
            if (!_isValid)
            {
                Debug.LogError($"PatrolRoute: There are no patrol points for GameObject {gameObject.name}");
            }
        }
        #endregion

        #region Class Methods

        public Transform GetFirstDestination()
        {
            _currentPatrolIndex = 0;
            return GetNextDestination();
        }

        public Transform GetNextDestination()
        {
            if (_currentPatrolIndex >= _numberOfPatrolPoints - 1)
            {
                _currentPatrolIndex = 0;
            }
            return patrolParams.PatrolPoints[_currentPatrolIndex];
        }
        #endregion

    }
}