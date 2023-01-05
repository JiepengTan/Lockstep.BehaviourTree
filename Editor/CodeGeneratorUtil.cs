using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace Lockstep.Tools.CodeGen
{
    public class CodeGenInfo
    {
        public string OutputPath;
        public string Namespace;
        public List<System.Type> AllTypes;
    }

    public class CodeGenFieldInfo
    {
        public System.Type Type;
        public string Name;
        public string TypeName => Type.Name;
        public string FullTypeName => Type.FullName;
        public bool IsEnum => Type.IsEnum;
        public bool IsStruct => !Type.IsClass && !Type.IsInterface;
        public bool IsClass => Type.IsClass;
        public bool IsString => Type == typeof(string);
        public bool IsArray => Type.IsArray;
        public bool IsList => Type.IsSubclassOf(typeof(IList));
        public bool IsDict => Type.IsSubclassOf(typeof(IDictionary));
    }

    public class CodeGenTemplateInfo
    {
        public string Default = "";
        public string Enum = "";
        public string Struct = "";
        public string Array = "";
        public string List = "";
        public string Dict = "";

        public string GetTemplateStr(CodeGenFieldInfo info)
        {
            if (info.IsEnum && !string.IsNullOrEmpty(Enum)) return Enum;
            if (info.IsList && !string.IsNullOrEmpty(List)) return List;
            if (info.IsDict && !string.IsNullOrEmpty(Dict)) return Dict;
            if (info.IsStruct && !string.IsNullOrEmpty(Struct)) return Struct;
            if (info.IsArray && !string.IsNullOrEmpty(Array)) return Array;
            return Default;
        }
    }

    public class FileUtil
    {
        public static void SaveFile(string path, byte[] finalStr,bool isNeedLog = true)
        {
            var dir = Path.GetDirectoryName(path);
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            if (File.Exists(path))
            {
                var rawContent = File.ReadAllBytes(path);
                if (finalStr.Length == rawContent.Length)
                {
                    bool isSame = true;
                    for (int i = 0; i < finalStr.Length; i++)
                    {
                        if (finalStr[i] != rawContent[i])
                        {
                            isSame = false;
                            break;
                        }
                    }
                   
                    if (isSame)
                    {
                        // 相同的内容跳过，避免重新的导入  
#if UNITY_EDITOR
                        if(isNeedLog) UnityEngine.Debug.Log("Output Skip " + path);
#endif
                        return;
                    }
                }

            }

            File.WriteAllBytes(path, finalStr);
#if UNITY_EDITOR
            UnityEditor.AssetDatabase.ImportAsset(path);
            if(isNeedLog) UnityEngine.Debug.Log("Output  " + path);
#endif
        }
        
        public static void SaveFile(string path, string finalStr,bool isNeedLog = true)
        {
            var dir = Path.GetDirectoryName(path);
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            if (File.Exists(path))
            {
                var rawContent = File.ReadAllText(path);
                if (finalStr == rawContent)
                {
                    // 相同的内容跳过，避免重新的导入  
                    return;
                }
            }

            File.WriteAllText(path, finalStr);
#if UNITY_EDITOR
            UnityEditor.AssetDatabase.ImportAsset(path);
            if(isNeedLog)   UnityEngine.Debug.Log("Output  " + path);
#endif
        }
    }
}