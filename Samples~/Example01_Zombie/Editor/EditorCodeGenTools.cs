using System.Collections.Generic;
using System.IO;
using System.Linq;
using Lockstep.AI;
using Lockstep.AI.Editor;
using Lockstep.Serialization;
using Lockstep.Tools.CodeGen;
using UnityEditor;
using UnityEditor.Graphs.AnimationBlendTree;
using UnityEngine;
using FileUtil = Lockstep.Tools.CodeGen.FileUtil;

namespace AIToolkitDemo
{
    public class EditorCodeGenTools
    {
        [MenuItem("Tools/GenCode/LockstepBT_Demo01")]
        public static void GenCode()
        {
            var dir = "Assets/Samples/Lockstep.BehaviourTree/0.1.0/Zombie example/Scripts/CodeGen/";
            var type = typeof(AIToolkitDemo.AIEntityWorkingData).Assembly.GetTypes().Where(
                a => !a.IsAbstract
                     && a.IsSubclassOf(typeof(BTNode)
                     )).ToList();
            CodeGenBTInjecter.GenCode(new CodeGenInfo()
            {
                Namespace = nameof(AIToolkitDemo),
                OutputPath = dir + "_CodeGen_BTNodeExt.cs",
                AllTypes = type
            });

            CodeGenBTSerialization.GenCode(new CodeGenInfo()
            {
                Namespace = nameof(AIToolkitDemo),
                OutputPath = dir + "_CodeGen_BTNodeExtSerialization.cs",
                AllTypes = type
            });
        }


        [MenuItem("Tools/BehaviourTree/ExportTxt")]
        public static void Export()
        {
            string SaveDir = "Assets/Samples/Lockstep.BehaviourTree/0.1.0/Zombie example/Configs/ExportBT";
            var guids = AssetDatabase.FindAssets($"t:{nameof(BTGraph)}");
            if (guids.Length > 1)
            {
                Debug.LogWarning($"Found multiple settings files, using the first.");
            }
            foreach (var guid in guids)
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                if(!path.StartsWith("Assets")) continue;
                var graph = AssetDatabase.LoadAssetAtPath<BTGraph>(path);
                var info = BTFactory.GetOrCreateInfo(graph);
                BTNode root = info.RootNode;
                List<BTNode> nodes = new List<BTNode>();
                Queue<BTNode> expendingNodes = new Queue<BTNode>();
                expendingNodes.Enqueue(root);
                while (expendingNodes.Count > 0)
                {
                    var node = expendingNodes.Dequeue();
                    nodes.Add(node);
                    var count = node.GetChildCount();
                    for (int i = 0; i < count; i++)
                    {
                        expendingNodes.Enqueue(node.GetChild(i));
                    }
                }

                var writer = new Serializer();
                writer.Write(nodes.Count);
                foreach (var node in nodes)
                {
                    writer.Write(node.TypeId);
                    node.Serialize(writer);
                }

                var bytes = writer.CopyData();
                FileUtil.SaveFile(Path.Combine(SaveDir, graph.name + ".btbytes"), bytes);
            }
        }
    }
}