using System;
using UnityEngine;
namespace DaftAppleGames.Darskerry.Core.PlayerController
{
    public enum MovementSpeed { Walking = 2, Running = 4, Sprinting = 6 }

    [Serializable]
    public class MoveSpeeds
    {
        [SerializeField] private float walkSpeed = 2.0f;
        [SerializeField] private float runSpeed = 5.0f;
        [SerializeField] private float sprintSpeed = 8.0f;
        [SerializeField] private MovementSpeed movementSpeed = MovementSpeed.Walking;

        public float GetMoveSpeed()
        {
            switch (movementSpeed)
            {
                case MovementSpeed.Walking:
                    return walkSpeed;
                case MovementSpeed.Running:
                    return runSpeed;
                case MovementSpeed.Sprinting:
                    return sprintSpeed;
                default:
                    return 0;
            }
        }

        public float GetSprintSpeed()
        {
            return sprintSpeed;
        }

        public float GetSprintSpeedDifference()
        {
            return sprintSpeed - walkSpeed;
        }
    }
}
