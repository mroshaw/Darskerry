using UnityEngine;
using Sirenix.OdinInspector;

namespace DaftAppleGames.Common.AI
{
    public abstract class AiAction : MonoBehaviour
    {
        [BoxGroup("AI Action Config")]
        [Range(0, 100)]
        public float chance;

        public abstract void DoAction();

    }
}
