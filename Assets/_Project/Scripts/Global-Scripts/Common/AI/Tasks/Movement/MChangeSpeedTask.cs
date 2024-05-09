using MalbersAnimations.Controller;
using MalbersAnimations.Scriptables;
using RenownedGames.AITree;
using UnityEngine;
using State = RenownedGames.AITree.State;

namespace Malbers.Integration.AITree.Core.Tasks
{
    [NodeContent("Change Speed", "Animal Controller/Movement/Change Speed", IconPath = "Icons/AnimalAI_Icon.png")]
    public class MChangeSpeedNode : MTaskNode
    {
        [Header("Node")]
        [Tooltip("Apply the Task to the Animal(Self) or the Target(Target)")]
        public EffectedTarget affect = EffectedTarget.Self;

        public string speedSet = "Ground";
        public IntReference speedIndex = new(3);
        public bool matchTargetSpeed;

        private MAnimal _targetAnimal;

        /// <summary>
        /// Identify the target animal component, if the target has one
        /// Decide whether to match the speed, and set the appropriate self animal speed
        /// </summary>
        protected override void OnEntry()
        {
            base.OnEntry();
            _targetAnimal = AIBrain.AIControl.Target.GetComponent<MAnimal>();
        }

        /// <summary>
        /// Set the new speed on the first tick
        /// </summary>
        /// <returns></returns>
        protected override State OnUpdate()
        {
            if (matchTargetSpeed && _targetAnimal)
            {
                AIBrain.Animal.SetSprint(_targetAnimal.Sprint);
                speedSet = _targetAnimal.CurrentSpeedSet.name;
                speedIndex = _targetAnimal.CurrentSpeedIndex;
                ChangeSpeed(AIBrain.Animal);
            }
            else
            {
                switch (affect)
                {
                    case EffectedTarget.Self:
                        ChangeSpeed(AIBrain.Animal);
                        break;
                    case EffectedTarget.Target:
                        if (_targetAnimal)
                        {
                            ChangeSpeed(_targetAnimal);
                        }
                        break;
                }
            }
            return State.Success;
        }

        /// <summary>
        /// Sets the speed of the given AI Animal
        /// </summary>
        /// <param name="animal"></param>
        private void ChangeSpeed(MAnimal animal)
        {
            animal?.SpeedSet_Set_Active(speedSet, speedIndex);
        }

        /// <summary>
        /// Return the task description
        /// </summary>
        /// <returns></returns>
        public override string GetDescription()
        {
            return $"{base.GetDescription()}\n{affect.ToString()}\nSpeedSet: {speedSet}\nSpeedIndex: {speedIndex.Value}\nMatch Target Speed: {matchTargetSpeed}\n";
        }
    }
}