using System.Collections.Generic;
using System.Linq;
using GraphProcessor;
using Lockstep.AI;
using Lockstep.AI.Editor;
using Unity.Plastic.Antlr3.Runtime.Misc;
using UnityEditor;
using UnityEngine;

namespace AIToolkitDemo
{
    public class EditorCodeGenTools
    {
        [MenuItem("Tools/GenCode/LockstepBT_Demo01")]
        public static void GenCode()
        {
            var type = typeof(AIToolkitDemo.AIEntityWorkingData).Assembly.GetTypes().Where(
                a=>!a.IsAbstract 
                && a.IsSubclassOf(typeof(BTNode)
                )).ToList();
            var info = new CodeGenInfo()
            {
                Namespace = nameof(AIToolkitDemo),
                OutputPath =
                    "Assets/Samples/GamesTan.BehaviourTree/0.1.0/Zombie example/Scripts/CodeGen/_CodeGen_BTNodeExt.cs",
                AllTypes = type
            };
            BTCodeGenerator.GenCode(info);
            Debug.Log("Done " + info.OutputPath);
        }
    }

}