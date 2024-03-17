using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.AI;
using MalbersAnimations.Scriptables;

namespace MalbersAnimations.Controller.AI
{
    [CreateAssetMenu(menuName = "Malbers Animations/Pluggable AI/Tasks/Set Random Destination")]
    public class SetRandomDestinationTask : MTask
    {
        public override string DisplayName => "Movement/Set Random Destination";

        [Tooltip("Radius within which to flee")]
        [BoxGroup("Task Settings")]  public float fleeDistance;

        [Tooltip("Slow multiplier to set on the Destination")]
        [BoxGroup("Task Settings")]  public float SlowMultiplier = 0;

        [Tooltip("When a new target is assigned it also sets that the Animal should move to that target")]
        public bool MoveToTarget = true;

        /// <summary>
        /// Overrides the StartTask - selects a random location for the Animal Brain to move to
        /// </summary>
        /// <param name="brain"></param>
        /// <param name="index"></param>
        public override void StartTask(MAnimalBrain brain, int index)
        {
            brain.AIControl.CurrentSlowingDistance = brain.AIControl.StoppingDistance * SlowMultiplier;

            if (GetRandomPosition(brain, out var newPosition))
            {
                brain.AIControl.ClearTarget();
                brain.AIControl.SetDestination(newPosition, MoveToTarget);
                brain.TaskDone(index);
            }
        }

        /// <summary>
        /// Gets a random position on the NavMesh, within the area specified by the radius
        /// </summary>
        /// <param name="brain"></param>
        /// <param name="randomPosition"></param>
        /// <returns></returns>
        private bool GetRandomPosition(MAnimalBrain brain, out Vector3 randomPosition)
        {
            Vector3 target = brain.Position + (Vector3)(fleeDistance * UnityEngine.Random.insideUnitCircle);
            if(NavMesh.SamplePosition(target, out NavMeshHit hit, 100.0f, NavMesh.AllAreas))
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

        void Reset() { Description = "Set a new random destination for the AI Control"; }
    }
#if UNITY_EDITOR
    [UnityEditor.CustomEditor(typeof(SetDestinationTask))]
    public class SetDestinationTaskEditor : UnityEditor.Editor
    {
        UnityEditor.SerializedProperty Description, WaitForPreviousTask, SlowMultiplier, MessageID, targetType, TargetT, TargetG, TargetRG, rtype, RTIndex, RTName, MoveToTarget, Destination;

        private void OnEnable()
        {
            Description = serializedObject.FindProperty("Description");
            SlowMultiplier = serializedObject.FindProperty("SlowMultiplier");
            MessageID = serializedObject.FindProperty("MessageID");
            targetType = serializedObject.FindProperty("targetType");
            Destination = serializedObject.FindProperty("Destination");
            TargetT = serializedObject.FindProperty("TargetT");
            TargetG = serializedObject.FindProperty("TargetG");
            TargetRG = serializedObject.FindProperty("TargetRG");
            rtype = serializedObject.FindProperty("rtype");
            RTIndex = serializedObject.FindProperty("RTIndex");
            RTName = serializedObject.FindProperty("RTName");
            MoveToTarget = serializedObject.FindProperty("MoveToTarget");
            WaitForPreviousTask = serializedObject.FindProperty("WaitForPreviousTask");


        }
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            UnityEditor.EditorGUILayout.PropertyField(Description);
            UnityEditor.EditorGUILayout.PropertyField(MessageID);
            UnityEditor.EditorGUILayout.PropertyField(WaitForPreviousTask);
            UnityEditor.EditorGUILayout.Space();
            UnityEditor.EditorGUILayout.HelpBox("All targets must be set at Runtime. Scriptable asset cannot have scenes References", UnityEditor.MessageType.Info);

            UnityEditor.EditorGUILayout.PropertyField(SlowMultiplier);
            UnityEditor.EditorGUILayout.PropertyField(targetType);

            var tt = (SetDestinationTask.DestinationType)targetType.intValue;

            switch (tt)
            {
                case SetDestinationTask.DestinationType.Transform:
                    UnityEditor.EditorGUILayout.PropertyField(TargetT, new GUIContent("Transform Hook"));
                    break;
                case SetDestinationTask.DestinationType.GameObject:
                    UnityEditor.EditorGUILayout.PropertyField(TargetG, new GUIContent("GameObject Hook"));
                    break;
                case SetDestinationTask.DestinationType.RuntimeGameObjects:
                    UnityEditor.EditorGUILayout.PropertyField(TargetRG, new GUIContent("Runtime Set"));
                    UnityEditor.EditorGUILayout.PropertyField(rtype, new GUIContent("Selection"));

                    var Sel = (RuntimeSetTypeGameObject)rtype.intValue;
                    switch (Sel)
                    {
                        case RuntimeSetTypeGameObject.Index:
                            UnityEditor.EditorGUILayout.PropertyField(RTIndex, new GUIContent("Element Index"));
                            break;
                        case RuntimeSetTypeGameObject.ByName:
                            UnityEditor.EditorGUILayout.PropertyField(RTName, new GUIContent("Element Name"));
                            break;
                        default:
                            break;
                    }
                    break;
                case SetDestinationTask.DestinationType.Vector3:
                    UnityEditor.EditorGUILayout.PropertyField(Destination, new GUIContent("Global Vector3"));

                    break;
                case SetDestinationTask.DestinationType.Name:
                    UnityEditor.EditorGUILayout.PropertyField(RTName, new GUIContent("GameObject name"));

                    break;
                default:
                    break;
            }
            UnityEditor.EditorGUILayout.PropertyField(MoveToTarget);
            serializedObject.ApplyModifiedProperties();
        }
    }

#endif

}
