using System;
using DaftAppleGames.Common.GameControllers;
using UnityEngine;
using Sirenix.OdinInspector;

namespace DaftAppleGames.Common.Debugging
{
    public class DebugTeleporter : DebugBase
    {
        [BoxGroup("Teleport Settings")] public bool isPlayerObject;
        [BoxGroup("Teleport Settings")] public GameObject teleportObject;
        [BoxGroup("Teleport Settings")] public TeleportTarget[] teleportTargets;

        [Serializable]
        public class TeleportTarget
        {
            public string TargetName;
            public Transform TargetTransform;
        }

        public void Teleport(string teleportTargetName)
        {
            foreach (TeleportTarget teleportTarget in teleportTargets)
            {
                if (teleportTarget.TargetName == teleportTargetName)
                {
                    TeleportObject(teleportTarget);
                    return;
                }
            }
            Debug.Log($"DebugTeleporter: cannot find teleport target named {teleportTargetName}. Check config on {gameObject.name}!");
        }

        /// <summary>
        /// Teleport target object to target transform
        /// </summary>
        /// <param name="teleportTarget"></param>
        private void TeleportObject(TeleportTarget teleportTarget)
        {
            if (isPlayerObject)
            {
                teleportObject = PlayerCameraManager.Instance.PlayerGameObject;
            }
            
            teleportObject.transform.position = teleportTarget.TargetTransform.position;
            teleportObject.transform.rotation = teleportTarget.TargetTransform.rotation;
        }
    }
}