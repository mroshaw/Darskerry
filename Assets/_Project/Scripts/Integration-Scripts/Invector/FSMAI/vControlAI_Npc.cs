#if INVECTOR_AI_TEMPLATE
using System.Collections;
using DaftAppleGames.Common.AI;
using DaftAppleGames.Common.AI.Invector.Actions;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Invector.vCharacterController.AI
{
    // Defines the type of work animations supported by the component
    public enum WorkType { None, Blacksmith, Farming, Mining, Gathering, Fishing, Preaching, Shopkeeper }
    
    // Defines the type of movement destinations supported.
    public enum AiDestination { Home, Work }
    
    // Partial implementation of vIControlAI for NPC specific behaviours and settings
    public partial interface vIControlAI
    {
        // These can be accessed from actions and decisions
        public NpcHome home { get; }
        public NpcWorkplace workplace { get; }
        public int workStartHour { get; }
        public int workEndHour { get; }
        public int sleepHour { get; }
        public int wakeHour { get; }
        public float arriveDistance { get; }
        public WorkType workType { get; }
        
        /// <summary>
        /// Method to cause the NPC to walk to their defined home
        /// </summary>
        /// <param name="speed"></param>
        public void GoHome(vAIMovementSpeed speed = vAIMovementSpeed.Walking);

        /// <summary>
        /// Rotate to the work transform rotation
        /// </summary>
        public void RotateToWorkPosition();

        /// <summary>
        /// Rotate to the home transform rotation
        /// </summary>
        public void RotateToHomePosition();

        /// <summary>
        /// Rotate to the transform rotation
        /// </summary>
        public void RotateToPosition(RotateType rotateType);

        /// <summary>
        /// Determines whether the FSM / NPC is at the given destination (e.g. work or home)
        /// </summary>
        public bool IsAtLocation(AiDestination aiDestination);
        
        /// <summary>
        /// Tell NPC to move to work
        /// </summary>
        /// <param name="speed"></param>
        public void GoToWork(vAIMovementSpeed speed = vAIMovementSpeed.Walking);

        /// <summary>
        /// Tell the NPC to stop or pause movement
        /// </summary>
        public void StopMovement();

        /// <summary>
        /// Determines whether NPC is in a conversation
        /// </summary>
        /// <returns></returns>
        public bool IsInConversation { get; set; }

        /// <summary>
        /// Debug to force NPC to go home
        /// </summary>
        public bool goHome { get; set; }
        
        /// <summary>
        /// Debug to force NPC to go to work
        /// </summary>
        public bool goToWork { get; set; }
    }

    // Partial vControlAI implementation, setting up NPC specific behaviours
    public partial class vControlAI
    {
        // Add "NPC Properties" section to the controller
        [vEditorToolbar("NPC Properties")]

        // NPC home location
        [BoxGroup("Work and Home Location Settings")]
        public NpcHome npcHome;
        [BoxGroup("Work and Home Location Settings")]
        public NpcWorkplace npcWorkplace;
        
        [BoxGroup("Work Settings")]
        public int npcWorkStartHour = 8;
        [BoxGroup("Work Settings")]
        public int npcWorkEndHour = 18;
        [BoxGroup("Work Settings")]
        public WorkType npcWorkType;

        [BoxGroup("Sleep Settings")]
        public int npcSleepHour = 22;
        [BoxGroup("Sleep Settings")]
        public int npcWakeHour = 6;

        [BoxGroup("Movement Settings")]
        public float npcArriveDistance = 0.2f;
        
        // Bools to allow us to test from the Editor inspector
        [BoxGroup("Debug Only")]
        public bool npcGoHome;
        [BoxGroup("Debug Only")]
        public bool npcGoToWork;
        
        /// <summary>
        /// Public bool accessible by the FSM AI component.
        /// </summary>
        public bool goHome
        {
            get => npcGoHome;
            set => npcGoHome = value;
        }
        
        /// <summary>
        /// Public bool accessible by the FSM AI component.
        /// </summary>
        public bool goToWork
        {
            get => npcGoToWork;
            set => npcGoToWork = value;
        }

        /// <summary>
        /// Public value accessible by the FSM AI component.
        /// </summary>
        public float arriveDistance => npcArriveDistance;

        /// <summary>
        /// Public properties for the FSM to use - returns the NpcHome instance
        /// </summary>
        public NpcHome home => npcHome;

        /// <summary>
        /// Public properties for the FSM to use - returns the NpcWorkplace instance
        /// </summary>
        public NpcWorkplace workplace => npcWorkplace;
        
        /// <summary>
        /// Returns the hour when the NPC starts work
        /// </summary>
        public int workStartHour => npcWorkStartHour;
        
        /// <summary>
        /// Returns the hour when the NPC finishes work
        /// </summary>
        public int workEndHour => npcWorkEndHour;
        
        /// <summary>
        /// Returns the hour when the NPC goes to sleep
        /// </summary>
        public int sleepHour => npcSleepHour;
        
        /// <summary>
        /// Returns the hour when the NPC wakes
        /// </summary>
        public int wakeHour => npcWakeHour;
        
        /// <summary>
        /// Returns the NPCs work type
        /// </summary>
        public WorkType workType => npcWorkType;

        private NpcWorkSpace _workSpace;
        private NpcHomeSpace _homeSpace;

        private bool _isInConversation;
        
        /*
        protected override void Start()
        {
            base.Start();
            InitNpc();
        }
        */

        /// <summary>
        /// Setup the NPC. Called by the Invector FSM AI component, since we don't have an
        /// abstract method to override
        /// </summary>
        public void InitNpc()
        {
            // Try to allocate a workplace to the NPC
            if (workplace)
            {
                _workSpace = workplace.GetWorkSpace();
                if (_workSpace == null)
                {
                    Debug.Log("NPC cannot allocate workspace!");
                }
            }

            // Try to allocate a homespace to the NPC
            if (home)
            {
                _homeSpace = home.GetHomeSpace();
                if (_homeSpace == null)
                {
                    Debug.Log("NPC cannot allocate homespace!");
                }
            }
        }
        
        /// <summary>
        /// Send the player to their home location
        /// </summary>
        /// <param name="speed"></param>
        public void GoHome(vAIMovementSpeed speed = vAIMovementSpeed.Running)
        {
            if (_homeSpace)
            {
                MoveTo(_homeSpace.idleTransform.position, speed);
            }
        }

        /// <summary>
        /// Send the player to their workplace location
        /// </summary>
        /// <param name="moveSpeed"></param>
        public void GoToWork(vAIMovementSpeed moveSpeed = vAIMovementSpeed.Running)
        {
            if (_workSpace)
            {
                MoveTo(_workSpace.busyTransform.position, moveSpeed);
            }
        }

        /// <summary>
        /// Cause the NPC to stop moving, for example when entering into a conversation
        /// with the player
        /// </summary>
        public void StopMovement()
        {
            navMeshAgent.isStopped = true;
        }

        /// <summary>
        /// Cause the NPC to resume movement, for example once a conversation is complete
        /// </summary>
        public void ResumeMovement()
        {
            navMeshAgent.isStopped = false;
        }

        /// <summary>
        /// Returns whether the NPC in a conversation
        /// </summary>
        /// <returns></returns>
        public bool IsInConversation
        {
            get => _isInConversation;
            set => _isInConversation = value;
        }
        
        /// <summary>
        /// Rotate the character to the rotation of the specified transform
        /// </summary>
        /// <param name="rotateType"></param>
        public void RotateToPosition(RotateType rotateType)
        {
            Vector3 targetRotation = new Vector3(0, 0, 0);
            
            switch (rotateType)
            {
                case RotateType.HomeBusy:
                    targetRotation = _homeSpace.busyTransform.rotation.eulerAngles;
                    break;
                case RotateType.HomeIdle:
                    targetRotation = _homeSpace.idleTransform.rotation.eulerAngles;
                    break;
                
                case RotateType.WorkBusy:
                    targetRotation = _workSpace.busyTransform.rotation.eulerAngles;
                    break;
                
                case RotateType.WorkIdle:
                    targetRotation = _workSpace.idleTransform.rotation.eulerAngles;
                    break;
                
                default:
                    return;
            }
            
            StartCoroutine(RotateAsync(Quaternion.Euler(targetRotation), 2.0f));
        }
        
        /// <summary>
        /// Rotate the character to the working position
        /// </summary>
        public void RotateToWorkPosition()
        {
            Vector3 targetRotation = _workSpace.busyTransform.rotation.eulerAngles;
            StartCoroutine(RotateAsync(Quaternion.Euler(targetRotation), 2.0f));
        }

        /// <summary>
        /// Rotate the character to the home position
        /// </summary>
        public void RotateToHomePosition()
        {
            Vector3 targetRotation = _homeSpace.idleTransform.rotation.eulerAngles;
            StartCoroutine(RotateAsync(Quaternion.Euler(targetRotation), 2.0f));
        }

        /// <summary>
        /// Determine if current AI is at a given destination
        /// </summary>
        /// <param name="aiDestination"></param>
        /// <returns></returns>
        public bool IsAtLocation(AiDestination aiDestination)
        {
            switch (aiDestination)
            {
                case AiDestination.Home:
                    return IsAtTransform(_homeSpace.idleTransform);
                    break;
                case AiDestination.Work:
                    return IsAtTransform(_workSpace.busyTransform);
                    break;
                default:
                    return false;
            }
        }
        
        /// <summary>
        /// Determine if current transform is "close enough" to target
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        private bool IsAtTransform(Transform target)
        {
            float distance = Vector3.Distance(transform.position, target.position);
            return (distance < npcArriveDistance);
        }
        
        /// <summary>
        /// Rotate the AI character smoothly to target rotation
        /// </summary>
        /// <param name="endValue"></param>
        /// <param name="duration"></param>
        /// <returns></returns>
        private IEnumerator RotateAsync(Quaternion endValue, float duration)
        {
            float time = 0;
            Quaternion startValue = transform.rotation;
            while (time < duration)
            {
                transform.rotation = Quaternion.Lerp(startValue, endValue, time / duration);
                time += Time.deltaTime;
                yield return null;
            }
            transform.rotation = endValue;
        }
    }
}
#endif