using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

namespace Lockstep.AI.Editor {
    // Create a new type of Settings Asset.
    public class BTProjectSettings : ScriptableObject {

        [Tooltip("Folder where new tree assets will be created. (Must begin with 'Assets')")]
        public string newTreePath = "Assets/";

        [Tooltip("Folder where new node scripts will be created. (Must begin with 'Assets')")]
        public string newNodePath = "Assets/";

        static BTProjectSettings FindSettings(){
            var guids = AssetDatabase.FindAssets($"t:{nameof(BTProjectSettings)}");
            if (guids.Length > 1) {
                Debug.LogWarning($"Found multiple settings files, using the first.");
            }

            switch (guids.Length) {
                case 0:
                    return null;
                default:
                    var path = AssetDatabase.GUIDToAssetPath(guids[0]);
                    return AssetDatabase.LoadAssetAtPath<BTProjectSettings>(path);
            }
        }

        internal static BTProjectSettings GetOrCreateSettings() {
            var settings = FindSettings();
            if (settings == null) {
                settings = ScriptableObject.CreateInstance<BTProjectSettings>();
                AssetDatabase.CreateAsset(settings, "Assets/BehaviourTreeProjectSettings.asset");
                AssetDatabase.SaveAssets();
            }
            return settings;
        }

        internal static SerializedObject GetSerializedSettings() {
            return new SerializedObject(GetOrCreateSettings());
        }
    }

}
