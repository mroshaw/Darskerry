#if PIXELCRUSHERS
using PixelCrushers;
using System;
using MalbersAnimations.Controller.AI;
using UnityEngine;

namespace DaftAppleGames.Common.LoadSaveGame
{
    public class NPCSaver : Saver
    {
        /// <summary>
        /// Class to store NPC saver details
        /// </summary>
        [Serializable]
        public class NPCSaverData
        {
            public Vector3 Position;
            public Quaternion Rotation;
            public string WaypointParentName;
            public string CurrentWaypointName;
            public string NextWaypointName;
        }

        private MAnimalAIControl _aiControl;

        /// <summary>
        /// Configure component
        /// </summary>
        public override void Start()
        {
            _aiControl = GetComponentInChildren<MAnimalAIControl>(true);
            base.Start();
        }

        /// <summary>
        /// Serialize and save
        /// </summary>
        /// <returns></returns>
        public override string RecordData()
        {
            Debug.Log($"NPCSaver: Recording game data from {gameObject.name}");

            NPCSaverData saverData = new NPCSaverData
            {
                Position = transform.position,
                Rotation = transform.rotation
            };

            if (_aiControl.Target)
            {
                saverData.WaypointParentName = _aiControl.Target.parent.gameObject.name;
                saverData.CurrentWaypointName = _aiControl.Target.gameObject.name;
            }

            if (_aiControl.NextTarget)
            {
                saverData.NextWaypointName = _aiControl.NextTarget.gameObject.name;
            }

            Debug.Log($"NPCSaver: Recorded game data from {gameObject.name}");
            return SaveSystem.Serialize(saverData);
        }

        /// <summary>
        /// Deserialize and load
        /// </summary>
        /// <param name="saveDataString"></param>
        public override void ApplyData(string saveDataString)
        {
            Debug.Log($"NPCSpawner: Applying game save data...{gameObject.name}");

            // Deserialize
            if (string.IsNullOrEmpty(saveDataString))
            {
                Debug.Log($"NPCSpawner: No save data found for {gameObject.name}");
                return; // No data to apply.
            }

            NPCSaverData saverData = SaveSystem.Deserialize<NPCSaverData>(saveDataString);
            if (saverData == null)
            {
                return;
            }

            // Update NPC transform
            transform.position = saverData.Position;
            transform.rotation = saverData.Rotation;

            // Find waypoints, if set
            if (!string.IsNullOrEmpty(saverData.CurrentWaypointName))
            {
                GameObject waypointParentGameObject = transform.parent.Find(saverData.WaypointParentName).gameObject;
                Debug.Log($"NPCSaver: Found waypoint container: {waypointParentGameObject.name}");
                GameObject currentWaypointGameObject =
                    waypointParentGameObject.transform.Find(saverData.CurrentWaypointName).gameObject;
                Debug.Log($"NPCSaver: Found current waypoint: {currentWaypointGameObject.name}");

                _aiControl.Target = currentWaypointGameObject.transform;
                    
                if (!string.IsNullOrEmpty(saverData.NextWaypointName))
                {
                    GameObject nextWaypointGameObject =
                        waypointParentGameObject.transform.Find(saverData.NextWaypointName).gameObject;
                    Debug.Log($"NPCSaver: Found next waypoint: {nextWaypointGameObject.name}");
                    _aiControl.NextTarget = nextWaypointGameObject.transform;
                }
            }
            Debug.Log($"NPCSpawner: Applied game save data to {gameObject.name}");
        }
    }
}
#endif
