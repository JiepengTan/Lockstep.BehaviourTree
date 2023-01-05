using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Lockstep.Tools.CodeGen;

namespace Lockstep.AI.Editor
{
    public class CodeGenBTSerialization
    {
        private static string fileTemplate = @"
// auto generate by tools, DO NOT Modify it!!!
using Lockstep.AI;
using Lockstep.Serialization;
";

        private static string typeTemplate = @"
namespace ##NAMESPACE
{
    public partial class ##TYPE_NAME
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

        private static BindingFlags BindingFlags =
            BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly;

        public static void GenCode(CodeGenInfo info)
        {
            string prefix = "\t\t\t";
            var types = info.AllTypes.ToArray().ToList();
            types.Sort((a, b) => a.Name.CompareTo(b.Name));
            StringBuilder sb = new StringBuilder();
            foreach (var type in types)
            {
                var clsStr = GenCodeByTemplate(type, typeTemplate);
                var contextTemplates = new List<CodeGenTemplateInfo>()
                {
                    new CodeGenTemplateInfo()
                    {
                        Default = prefix + "writer.Write(##NAME);",
                        Enum = prefix + "writer.Write((int)##NAME);", // TODO deal with array list &dict
                    },
                    new CodeGenTemplateInfo()
                    {
                        Default = prefix + "##NAME = reader.Read##TYPE_NAME();",
                        Enum = prefix + "##NAME = (##TYPE_NAME)reader.ReadInt32();",
                    },
                };
                var fields = type.GetFields(BindingFlags)
                    .Select(a => new CodeGenFieldInfo() { Name = a.Name, Type = a.FieldType }).ToList();
                var properties = type.GetProperties(BindingFlags).Where(a=>a.CanRead&&a.CanWrite)
                    .Select(a => new CodeGenFieldInfo() { Name = a.Name, Type = a.PropertyType }).ToList();
                fields.AddRange(properties);
                for (int i = 0; i < contextTemplates.Count; i++)
                {
                    clsStr = clsStr.Replace("##CODEREPLACE_" + i, GetFieldsCode(fields, contextTemplates[i]));
                }

                if (fields.Count > 0)
                {
                    sb.AppendLine(clsStr);
                }
            }

            var finalStr = fileTemplate;
            finalStr += sb.ToString();
            var path = info.OutputPath;
            FileUtil.SaveFile(path, finalStr);
        }


        private static string GetFieldsCode(List<CodeGenFieldInfo> fields, CodeGenTemplateInfo template)
        {
            StringBuilder sbField = new StringBuilder();
            foreach (var field in fields)
            {
                var templateStr = template.GetTemplateStr(field);
                var str = templateStr
                        .Replace("##NAME", field.Name)
                        .Replace("##TYPE_NAME", field.TypeName)
                        .Replace("##FULL_TYPE_NAME", field.FullTypeName)
                    ;
                sbField.AppendLine(str);
            }

            return sbField.ToString();
        }

        private static string GenCodeByTemplate(Type type, string template)
        {
            StringBuilder sb = new StringBuilder();
            var nameSpace = type.Namespace.ToString();
            var typeName = type.Name.ToString();
            var fullTypeName = type.FullName.ToString();
            var str = template
                    .Replace("##NAMESPACE", nameSpace)
                    .Replace("##TYPE_NAME", typeName)
                    .Replace("##FULL_TYPE_NAME", fullTypeName)
                ;
            sb.AppendLine(str);
            return sb.ToString();
        }
    }
}