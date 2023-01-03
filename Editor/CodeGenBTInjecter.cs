using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lockstep.AI.Editor
{
    public class CodeGenBTInjecter
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

            var path = info.OutputPath;
            CodeGeneratorUtil. SaveFile(path, finalStr);
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