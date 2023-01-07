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
            string SaveDir = "Assets/Samples/Lockstep.BehaviourTree/0.1.0/Zombie example/Resources/Configs";
            var guids = AssetDatabase.FindAssets($"t:{nameof(BTGraph)}");
            if (guids.Length > 1)
            {
                Debug.LogWarning($"Found multiple settings files, using the first.");
            }

            int exportCount = 0;
            foreach (var guid in guids)
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                if(!path.StartsWith("Assets")) continue;
                exportCount++;
                var graph = AssetDatabase.LoadAssetAtPath<BTGraph>(path);
                var info = BTFactory.GetOrCreateInfo(graph);
                var bytes =BTFactory.Serialize(info);
                FileUtil.SaveFile(Path.Combine(SaveDir, graph.name + ".json"), bytes);
                var newInfo = BTFactory.Deserialize(bytes);
                var newBytes = BTFactory.Serialize(newInfo);
                Lockstep.Logging.Debug.Assert(newBytes.EqualsEx(bytes),"BehaviourTree Serialize Failed "); 
            }
            Debug.Log("Export Done count= " + exportCount);
        }

    }
}