using Sirenix.OdinInspector;
using UnityEngine;

namespace DaftAppleGames.Common.AI
{
    /// <summary>
    /// MonoBehaviour defining an NPC home. That being one or more homespaces where an NPC
    /// can perform home based actions, such as sitting, sleeping, idling.
    /// </summary>
    public class NpcHome : MonoBehaviour
    {
        [BoxGroup("Home Settings")]
        public NpcHomeSpace[] homeSpaces;

        private int _numSpaces;
        private int _numSpacesAllocated;
        
        /// <summary>
        /// Initialise the component
        /// </summary>
        private void Awake()
        {
            _numSpaces = homeSpaces.Length;
        }

        /// <summary>
        /// Allocate a free homespace, if there is one
        /// </summary>
        /// <returns></returns>
        public NpcHomeSpace GetHomeSpace()
        {
            if (_numSpacesAllocated >= _numSpaces)
            {
                return null;
            }

            _numSpacesAllocated++;
            return homeSpaces[_numSpacesAllocated-1];
        }
    }
}
