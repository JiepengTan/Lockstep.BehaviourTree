using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using UnityEngine.Serialization;

namespace Lockstep.AI.Editor {
    [CreateAssetMenu(menuName = "Lockstep/BehaviourTree/ProjectSettings")]
    public class BTProjectSettings : ScriptableObject
    {
        [Header("PureMode Configs")]
        public string OutputBTBytesDir = "Assets/Resources/Configs";
        public string OutputBTBytesFilePosfix = ".btbytes";

        [Header("Generated Code")] 
        public string CodeGenDir = "Assets/Scripts/LockstepBehaviourTree/_CodeGen";
        public string CodeGenNameSpace = "Lockstep.AI";
        public List<string> CodeGenAssemblies = new List<string>(){"Assembly-CSharp"};

        static string s_buildInGUID = "2e05252120d089a49ba4928ed7d86158";
        static BTProjectSettings FindSettings(){
            var guids = AssetDatabase.FindAssets($"t:{nameof(BTProjectSettings)}");
            string targetGuid = s_buildInGUID;
            if (guids.Length > 1) {
                Debug.LogWarning($"Found multiple settings files, using the first.");
                foreach (var guid in guids)
                {
                    if (guid != s_buildInGUID)
                    {
                        targetGuid = guid;
                        break;
                    }
                }
            }
            var path = AssetDatabase.GUIDToAssetPath(targetGuid);
            return AssetDatabase.LoadAssetAtPath<BTProjectSettings>(path);
        }

        public static BTProjectSettings GetOrCreateSettings() {
            var settings = FindSettings();
            if (settings == null) {
                settings = ScriptableObject.CreateInstance<BTProjectSettings>();
                AssetDatabase.CreateAsset(settings, "Assets/LockstepBehaviourTreeProjectSettings.asset");
                AssetDatabase.SaveAssets();
            }
            return settings;
        }

        internal static SerializedObject GetSerializedSettings() {
            return new SerializedObject(GetOrCreateSettings());
        }
    }

}
