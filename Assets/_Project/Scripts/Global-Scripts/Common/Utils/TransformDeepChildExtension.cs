using System.Collections.Generic;
using UnityEngine;

namespace DaftAppleGames.Common.Utils
{
    public static class TransformDeepChildExtension
    {
        //Breadth-first search
        public static Transform FindDeepChild(this Transform aParent, string aName)
        {
            Queue<Transform> queue = new Queue<Transform>();
            queue.Enqueue(aParent);
            while (queue.Count > 0)
            {
                var c = queue.Dequeue();
                if (c.name.ToLower().Contains(aName.ToLower()))
                    return c;
                foreach (Transform t in c)
                    queue.Enqueue(t);
            }

            return null;
        }

    }
}
