using MalbersAnimations.Controller.AI;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;
using UnityEngine.AI;

namespace DaftAppleGames.Common.AI
{

	[Category("Malbers AI")]
	[Description("Stops the animal movement")]
	public class Flee : ActionTask
    {
        [RequiredField] public BBParameter<MalbersAiHelper> malbersHelperBBParam;
        [RequiredField] public float fleeDistance;
        public string fleeSpeedSet = "Ground";
        public int speedIndex = 2;

        //Use for initialization. This is called only once in the lifetime of the task.
        //Return null if init was successfull. Return an error string otherwise
        protected override string OnInit()
        {
			return null;
		}

		//This is called once each time the task is enabled.
		//Call EndAction() to mark the action as finished, either in success or failure.
		//EndAction can be called from anywhere.
		protected override void OnExecute()
        {
            Transform fleeTransform;

            if(GetRandomPosition(agent.transform, out Vector3 newPosition))
            {
                malbersHelperBBParam.value.Animal.SpeedSet_Set_Active(fleeSpeedSet, speedIndex);
                malbersHelperBBParam.value.AnimalAiControl.SetDestination(newPosition, true);
            }
            EndAction(true);
		}

		//Called once per frame while the action is active.
		protected override void OnUpdate()
        {
			
		}

		//Called when the task is disabled.
		protected override void OnStop()
        {
			
		}

		//Called when the task is paused.
		protected override void OnPause()
        {
			
		}

        /// <summary>
        /// Gets a random position on the NavMesh, within the area specified by the radius
        /// </summary>
        /// <param name="brain"></param>
        /// <param name="randomPosition"></param>
        /// <returns></returns>
        private bool GetRandomPosition(Transform currTransform, out Vector3 randomPosition)
        {
            Vector3 target = currTransform.position + (Vector3)(fleeDistance * UnityEngine.Random.insideUnitCircle);
            if (NavMesh.SamplePosition(target, out NavMeshHit hit, 100.0f, NavMesh.AllAreas))
            {
                randomPosition = hit.position;
                return true;
            }
            else
            {
                randomPosition = Vector3.zero;
                return false;
            }
        }
    }
}