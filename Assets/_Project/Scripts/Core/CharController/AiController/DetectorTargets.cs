using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DaftAppleGames.Darskerry.Core.CharController.AiController
{
    internal class DetectorTargets : IEnumerable<KeyValuePair<string, DetectorTarget>>
    {
        private Dictionary<string, DetectorTarget> targets;

        public DetectorTargets()
        {
            targets = new Dictionary<string, DetectorTarget>();
        }

        internal void AddTarget(string guid, GameObject target, float distance)
        {
            DetectorTarget newTarget = new()
            {
                target = target,
                distance = distance,
            };
            targets.Add(guid, newTarget);
        }

        internal void RemoveTarget(string guid)
        {
            targets.Remove(guid);
        }

        internal bool HasGuid(string guid)
        {
            return targets.ContainsKey(guid);
        }

        internal GameObject GetTargetGameObject(string guid)
        {
            return targets[guid].target;
        }

        internal bool HasTargets()
        {
            return targets.Count > 0;
        }

        internal bool HasTargetWithTag(string tag)
        {
            foreach (var entry in targets)
            {
                if (entry.Value.target.CompareTag(tag))
                {
                    return true;
                }
            }
            return false;
        }

        internal GameObject GetClosestTargetGameObject()
        {
            KeyValuePair<string, DetectorTarget> minTarget = default;
            float minDistance = float.MaxValue;
            foreach (var entry in targets)
            {
                if (entry.Value.distance < minDistance)
                {
                    minDistance = entry.Value.distance;
                    minTarget = entry;
                }
            }
            return minTarget.Value.target;
        }

        internal GameObject GetClosestTargetGameObjectWithTag(string tag)
        {
            KeyValuePair<string, DetectorTarget> minTarget = default;
            float minDistance = float.MaxValue;
            foreach (var entry in targets)
            {
                if (entry.Value.target.CompareTag(tag) && entry.Value.distance < minDistance)
                {
                    minDistance = entry.Value.distance;
                    minTarget = entry;
                }
            }
            return minTarget.Value.target;
        }

        internal GameObject[] GetAllTargetGameObjects()
        {
            int numTargets = targets.Count;
            GameObject[] allGameObjects = new GameObject[numTargets];
            int currTargetIndex = 0;

            foreach (KeyValuePair<string, DetectorTarget> currTarget in targets)
            {
                allGameObjects[currTargetIndex] = currTarget.Value.target;
            }

            return allGameObjects;
        }

        public IEnumerator<KeyValuePair<string, DetectorTarget>> GetEnumerator()
        {
            return targets.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    internal class DetectorTarget
    {
        internal GameObject target;
        internal float distance;
    }
}
