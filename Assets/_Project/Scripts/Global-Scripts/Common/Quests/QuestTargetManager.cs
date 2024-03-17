#if PIXELCRUSHERS
using System;
using PixelCrushers.QuestMachine;
using UnityEngine;

namespace DaftAppleGames.Common.Quests 
{
    public class QuestTargetManager : MonoBehaviour
    {
        public QuestTarget[] questTargets;
        public string questMessage;
        
        [Serializable]
        public class QuestTarget
        {
            public Quest quest;
            public Transform questTargetTransform;
            public GameObject hudNavPrefab;
        }
    }
}
#endif