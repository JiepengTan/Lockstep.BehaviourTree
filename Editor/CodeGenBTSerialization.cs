using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lockstep.AI.Editor
{
    public class CodeGenBTSerialization
    {
        private static string fileTemplate = @"
// auto generate by tools, DO NOT Modify it!!!
using Lockstep.AI;
";
        private static string typeTemplate = @"
namespace ##NAMESPACE
{
    public partial class ##CLSNAME
    {
        public override void Serialize(Serializer writer)
        {
            base.Serialize(writer);
##CODEREPLACE_0
        }

        public override void Deserialize(Deserializer reader)
        {
            base.Deserialize(reader);
##CODEREPLACE_1
        }
    }
}";
        public static void GenCode(CodeGenInfo info)
        {
            return;
            string prefix = "\t\t";
            
            var types = info.AllTypes.ToArray().ToList();
            types.Sort((a,b)=>a.Name.CompareTo(b.Name));
            StringBuilder sb = new StringBuilder();
            // TODO tanjp 完成 属性成员的序列化
            foreach (var type in types)
            {
                var clsStr= GenCodeByTemplate(type,typeTemplate);
                var contextTemplates = new List<string>()
                {
                    prefix+"##TYPENAME = ##INDEX,",
                    prefix+"\tBTNodeFactory.Register((int)(EGameBTNodeType.##TYPENAME),()=>new ##FULLTYPENAME());"
                };
                for (int i = 0; i < contextTemplates.Count; i++)
                {
                    //clsStr = clsStr.Replace("##CODEREPLACE_" + i, GenCodeByTemplate(types, contextTemplates[i]));
                }

                sb.AppendLine(clsStr);
            }
            var finalStr= typeTemplate.Replace("##NAMESPACE", info.Namespace);
            finalStr += sb.ToString();
            var path = info.OutputPath;
            CodeGeneratorUtil. SaveFile(path, finalStr);
        }

        private static string GenCodeByTemplate(Type type,string template)
        {
            StringBuilder sb = new StringBuilder();
            var nameSpace = type.Namespace.ToString();
            var typeName = type.Name.ToString();
            var fullTypeName = type.FullName.ToString();
            var str = template
                    .Replace("##NAMESPACE", nameSpace)
                    .Replace("##TYPENAME", typeName)
                    .Replace("##FULLTYPENAME", fullTypeName)
                ;
            sb.AppendLine(str);
            return   sb.ToString();
        }
    }
}