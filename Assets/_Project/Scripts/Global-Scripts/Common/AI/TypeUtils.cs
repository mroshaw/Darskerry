using MalbersAnimations.Scriptables;
using RenownedGames.AITree;
using UnityEngine;

namespace Malbers.Integration.AITree.Core
{
    /// <summary>
    /// Static class methods for transforming various parameter types suitable for both AI Tree and Malbers
    /// </summary>
    public static class TypeUtils
    {
        // Int
        public static int GetIntParam(IntVar intVar) => intVar.Value;
        public static int GetIntParam(IntKey intKey) => intKey.GetValue();

        // Float
        public static float GetFloatParam(IntVar floatVar) => floatVar.Value;
        public static float GetFloatParam(IntKey floatKey) => floatKey.GetValue();

        // Bool
        public static bool GetBoolParam(BoolVar boolVar) => boolVar.Value;
        public static bool GetBoolParam(BoolKey boolKey) => boolKey.GetValue();
        
        // Transform
        public static Transform GetTransformParam(TransformVar transformVar) => transformVar.Value;
        public static Transform GetTransformParam(TransformKey transformKey) => transformKey.GetValue();

        // Vector3
        public static Vector3 GetVector3Param(TransformVar transformVar) => transformVar.Value.position;
        public static Vector3 GetVector3Param(TransformKey transformKey) => transformKey.GetValue().position;

    }
}