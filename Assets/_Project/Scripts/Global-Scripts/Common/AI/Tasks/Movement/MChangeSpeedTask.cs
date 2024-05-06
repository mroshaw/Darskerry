using MalbersAnimations.Controller;
using MalbersAnimations.Scriptables;
using RenownedGames.AITree;
using UnityEngine;
using State = RenownedGames.AITree.State;

namespace Malbers.Integration.AITree.Core.Tasks
{
    [NodeContent("Change Speed", "Animal Controller/Change Speed", IconPath = "Icons/AnimalAI_Icon.png")]
    public class MChangeSpeedNode : MTaskNode
    {
        [Header("Node")]
        [Tooltip("Apply the Task to the Animal(Self) or the Target(Target)")]
        public EffectedTarget affect = EffectedTarget.Self;

        public string speedSet = "Ground";
        public IntReference speedIndex = new(3);
        public bool matchTargetSpeed;

        private MAnimal targetAnimal;

        protected override State OnUpdate()
        {
            if (matchTargetSpeed)
            {
                AIBrain.Animal.SetSprint(targetAnimal.Sprint);
                speedSet = targetAnimal.CurrentSpeedSet.name;
                speedIndex = targetAnimal.CurrentSpeedIndex;
                ChangeSpeed(AIBrain.Animal);
            }
            else
            {
                switch (affect)
                {
                    case EffectedTarget.Self: ChangeSpeed(AIBrain.Animal); break;
                    case EffectedTarget.Target: ChangeSpeed(AIBrain.TargetAnimal); break;
                }
            }
            return State.Success;
        }

        private void ChangeSpeed(MAnimal animal) => animal?.SpeedSet_Set_Active(speedSet, speedIndex);

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