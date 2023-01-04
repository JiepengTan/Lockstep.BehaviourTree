using System.Collections.Generic;
using System.Linq;
using Lockstep.AI;
using Lockstep.AI.Editor;
using Lockstep.Tools.CodeGen;
using UnityEditor;
using UnityEngine;

namespace AIToolkitDemo
{
    public class EditorCodeGenTools
    {
        [MenuItem("Tools/GenCode/LockstepBT_Demo01")]
        public static void GenCode()
        {
            var dir = "Assets/Samples/Lockstep.BehaviourTree/0.1.0/Zombie example/Scripts/CodeGen/";
            var type = typeof(AIToolkitDemo.AIEntityWorkingData).Assembly.GetTypes().Where(
                a=>!a.IsAbstract 
                && a.IsSubclassOf(typeof(BTNode)
                )).ToList();
            CodeGenBTInjecter.GenCode( new CodeGenInfo()
            {
                Namespace = nameof(AIToolkitDemo),
                OutputPath = dir+"_CodeGen_BTNodeExt.cs",
                AllTypes = type
            });
            
            CodeGenBTSerialization.GenCode( new CodeGenInfo()
            {
                Namespace = nameof(AIToolkitDemo),
                OutputPath =dir+"_CodeGen_BTNodeExtSerialization.cs",
                AllTypes = type
            });
         
        }




        [MenuItem("Tools/BehaviourTree/ExportTxt")]
        public static void Export()
        {
            BTGraph graph = null;
            var info = BTFactory.GetOrCreateInfo(graph);
            
            // edges 
            // nodes
            BTNode root = info.RootNode;
            List<BTNode> nodes = new List<BTNode>();
            Queue<BTNode> expendingNodes = new Queue<BTNode>();
            expendingNodes.Enqueue(root);
            while (expendingNodes.Count>0)
            {
                var node = expendingNodes.Dequeue();
                nodes.Add(node);
                
            }  
        }
    }

}