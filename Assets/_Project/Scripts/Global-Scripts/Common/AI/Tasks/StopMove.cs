#if NODECANVAS
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace DaftAppleGames.Common.AI
{

	[Category("Malbers AI")]
	[Description("Stops the animal movement")]
	public class StopMove : ActionTask
    {
        [RequiredField] public BBParameter<MalbersAiHelper> malbersHelperBBParam;

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
            malbersHelperBBParam.value.AnimalAiControl.Stop();
            malbersHelperBBParam.value.AnimalAiControl.SetTarget((Transform)null);
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
#endif