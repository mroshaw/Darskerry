using MalbersAnimations.Controller.AI;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace DaftAppleGames.Common.AI
{

	[Category("Malbers AI")]
	[Description("Moves the AI to a given transform location")]
	public class MoveTo : ActionTask
    {
        [RequiredField] public BBParameter<MalbersAiHelper> malbersHelperBBParam;
        [RequiredField] public BBParameter<GameObject> targetTransformBBParam;

        public string moveSpeedSet = "Ground";
        public int speedIndex = 1; // Walk

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
            malbersHelperBBParam.value.Animal.SpeedSet_Set_Active(moveSpeedSet, speedIndex);

            Transform nextTarget = malbersHelperBBParam.value.AnimalAiControl.NextTarget;

            if (nextTarget)
            {
                malbersHelperBBParam.value.AnimalAiControl.SetTarget(nextTarget);
            }
            else
            {
                malbersHelperBBParam.value.AnimalAiControl.SetTarget(targetTransformBBParam.value.transform);

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
	}
}