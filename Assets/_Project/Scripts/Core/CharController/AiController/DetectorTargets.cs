using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DaftAppleGames.Darskerry.Core.CharController.AiController
{
    public class DetectorTargets : IEnumerable<KeyValuePair<string, DetectorTarget>>
    {
        private readonly Dictionary<string, DetectorTarget> _targets = new();

        internal void AddTarget(string guid, GameObject target, float distance)
        {
            DetectorTarget newTarget = new()
            {
                Target = target,
                Distance = distance,
            };
            _targets.Add(guid, newTarget);
        }

        internal void RemoveTarget(string guid)
        {
            _targets.Remove(guid);
        }

        internal bool HasGuid(string guid)
        {
            return _targets.ContainsKey(guid);
        }

        internal GameObject GetTargetGameObject(string guid)
        {
            return _targets[guid].Target;
        }

        internal bool HasTargets()
        {
            return _targets.Count > 0;
        }

        internal bool HasTargetWithTag(string tag)
        {
            foreach (var entry in _targets)
            {
                if (entry.Value.Target.CompareTag(tag))
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
            foreach (var entry in _targets)
            {
                if (entry.Value.Distance < minDistance)
                {
                    minDistance = entry.Value.Distance;
                    minTarget = entry;
                }
            }
            return minTarget.Value.Target;
        }

        internal GameObject GetClosestTargetWithTag(string tag)
        {
            KeyValuePair<string, DetectorTarget> minTarget = default;
            float minDistance = float.MaxValue;
            foreach (var entry in _targets)
            {
                if (entry.Value.Target.CompareTag(tag) && entry.Value.Distance < minDistance)
                {
                    minDistance = entry.Value.Distance;
                    minTarget = entry;
                }
            }

            // If target found, return closest. Otherwise, return null
            return minDistance < float.MaxValue ? minTarget.Value.Target : null;
        }

        internal GameObject[] GetAllTargetGameObjects()
        {
            int numTargets = _targets.Count;
            GameObject[] allGameObjects = new GameObject[numTargets];
            int currTargetIndex = 0;

            foreach (KeyValuePair<string, DetectorTarget> currTarget in _targets)
            {
                allGameObjects[currTargetIndex] = currTarget.Value.Target;
            }

            return allGameObjects;
        }

        public IEnumerator<KeyValuePair<string, DetectorTarget>> GetEnumerator()
        {
            return _targets.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    public struct DetectorTarget
    {
        internal GameObject Target;
        internal float Distance;
    }
}