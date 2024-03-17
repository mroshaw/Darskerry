using Sirenix.OdinInspector;
using UnityEngine;

namespace DaftAppleGames.Common.AI 
{
    /// <summary>
    /// MonoBehaviour defining an NPC workplace. That is, a structure or area
    /// where there are one or more workspaces for NPCs to perform work actions.
    /// </summary>
    public class NpcWorkplace : MonoBehaviour
    {
        [BoxGroup("Workplace Settings")]
        public NpcWorkSpace[] workSpaces;

        private int _numSpaces;
        private int _numSpacesAllocated = 0;
        
        /// <summary>
        /// Initialise the component
        /// </summary>
        private void Awake()
        {
            _numSpaces = workSpaces.Length;
        }
    
        /// <summary>
        /// Attempts to allocate and return a workplace
        /// </summary>
        /// <returns></returns>
        public NpcWorkSpace GetWorkSpace()
        {
            if (_numSpacesAllocated >= _numSpaces)
            {
                return null;
            }

            _numSpacesAllocated++;
            return workSpaces[_numSpacesAllocated-1];
        }
    }
}
