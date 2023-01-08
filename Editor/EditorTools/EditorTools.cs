using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Lockstep.Serialization;
using Lockstep.Tools.CodeGen;
using UnityEditor;
using UnityEngine;
using FileUtil = Lockstep.Tools.CodeGen.FileUtil;

namespace Lockstep.AI.Editor
{
    public class EditorTools
    {
        [MenuItem("Tools/Lockstep/BehaviourTree/1.GenCode")]
        public static void GenCode()
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            var projectConfig = BTProjectSettings.GetOrCreateSettings();
            List<Type> types = new List<Type>();
            for (var i = 0; i < assemblies.Length; ++i)
            {
                var assembly = assemblies[i];
                if (assembly == null)
                    continue;
                var assName = assembly.GetName().Name;
                if (projectConfig.CodeGenAssemblies.Contains(assName))
                {
                    var lst = assembly.GetTypes().Where(
                        a => !a.IsAbstract
                             && a.IsSubclassOf(typeof(BTNode)
                             ));
                    types.AddRange(lst);
                }
            }

            var dir = projectConfig.CodeGenDir;
            CodeGenBTInjecter.GenCode(new CodeGenInfo()
            {
                Namespace = projectConfig.CodeGenNameSpace,
                OutputPath = Path.Combine(dir, "_CodeGen_BTNodeExt.cs")  ,
                AllTypes = types
            });

            CodeGenBTSerialization.GenCode(new CodeGenInfo()
            {
                Namespace = projectConfig.CodeGenNameSpace,
                OutputPath = Path.Combine(dir, "_CodeGen_BTNodeExtSerialization.cs"),
                AllTypes = types
            });
            Debug.Log("Gen Code Done OutputDir= " + dir);
        }
#if !LOCKSTEP_PURE_MODE
        [MenuItem("Tools/Lockstep/TurnOn PureMode")]
        public static void TogglePureModeOn()
        {
            EditorMacroUtil.TogglePureMode(true);
        }
#else
        [MenuItem("Tools/Lockstep/TurnOff PureMode")]
        public static void TogglePureModeOff()
        {
            EditorMacroUtil.TogglePureMode(false);
        }
#endif
        [MenuItem("Tools/Lockstep/BehaviourTree/2.GenData")]
        public static void GenData()
        {
            var projectConfig = BTProjectSettings.GetOrCreateSettings();
            string SaveDir = projectConfig.OutputBTBytesDir;
            var guids = AssetDatabase.FindAssets($"t:{nameof(BTGraph)}");
            if (guids.Length > 1)
            {
                Debug.LogWarning($"Found multiple settings files, using the first.");
            }

            int exportCount = 0;
            foreach (var guid in guids)
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                // 过滤内置配置
                if (path.StartsWith("Packages/com.gamestan.lockstepaibehaviourtree")) continue;
                exportCount++;
                var graph = AssetDatabase.LoadAssetAtPath<BTGraph>(path);
                var info = BTFactory.GetOrCreateInfo(graph);
                var bytes = BTFactory.Serialize(info);
                FileUtil.SaveFile(Path.Combine(SaveDir, graph.name + projectConfig.OutputBTBytesFilePosfix), bytes);
                var newInfo = BTFactory.Deserialize(bytes);
                var newBytes = BTFactory.Serialize(newInfo);
                Lockstep.Logging.Debug.Assert(newBytes.EqualsEx(bytes), "BehaviourTree Serialize Failed ");
            }
            Debug.Log("Export Done count= " + exportCount);
        }
    }
}