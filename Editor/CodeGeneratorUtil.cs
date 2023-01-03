using System.Collections.Generic;
using System.IO;
using UnityEditor;

namespace Lockstep.AI.Editor
{
    public class CodeGenInfo
    {
        public string OutputPath;
        public string Namespace;
        public List<System.Type> AllTypes;
    }
    
    public class CodeGeneratorUtil
    {
        public static void SaveFile(string path, string finalStr)
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
            AssetDatabase.ImportAsset(path);
        }
    }
}