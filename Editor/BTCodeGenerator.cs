using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using GraphProcessor;
using Unity.Plastic.Antlr3.Runtime.Misc;
using UnityEditor;
using UnityEngine;

namespace Lockstep.AI.Editor
{
    public class CodeGenInfo
    {
        public string OutputPath;
        public string Namespace;
        public List<System.Type> AllTypes;
    }

    public class BTCodeGenerator
    {
        private static string template = @"
// auto generate by tools, DO NOT Modify it!!!
using Lockstep.AI;
namespace ##NAMESPACE
{
    public enum EGameBTNodeType
    {
##CODEREPLACE_0
    }
    
    public partial class BTNodeFactoryInjector
    {
        partial void _Inject()
        {
##CODEREPLACE_1
        }
    }
}";
        public static void GenCode(CodeGenInfo info)
        {
            string prefix = "\t\t";
            var contextTemplates = new List<string>()
            {
                prefix+"##TYPENAME = ##INDEX,",
                prefix+"\tBTNodeFactory.Register((int)(EGameBTNodeType.##TYPENAME),()=>new ##FULLTYPENAME());"
            };
            
            var types = info.AllTypes.ToArray().ToList();
            types.Sort((a,b)=>a.Name.CompareTo(b.Name));
            var finalStr= template
                .Replace("##NAMESPACE", info.Namespace);
            for (int i = 0; i < contextTemplates.Count; i++)
            {
                finalStr = finalStr.Replace("##CODEREPLACE_" + i, GenCodeByTemplate(types, contextTemplates[i]));
            }
            var dir = Path.GetDirectoryName(info.OutputPath);
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            if (File.Exists(info.OutputPath))
            {
                var rawContent = File.ReadAllText(info.OutputPath);
                if (finalStr == rawContent)
                {
                    // 相同的内容跳过，避免重新的导入  
                    return; 
                }
            }

            File.WriteAllText(info.OutputPath, finalStr);
            AssetDatabase.ImportAsset(info.OutputPath);
        }

        private static string GenCodeByTemplate(List<Type> types,string template)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < types.Count; i++)
            {
                var type = types[i];
                var index = ((int)EBuiltinBTNodeType.EnumCount + i).ToString();
                var typeName = type.Name.ToString();
                var fullTypeName = type.FullName.ToString();
                var str = template
                        .Replace("##INDEX", index)
                        .Replace("##TYPENAME", typeName)
                        .Replace("##FULLTYPENAME", fullTypeName)
                    ;
                sb.AppendLine(str);
            }
            return   sb.ToString();
        }
    }


}