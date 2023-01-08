using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Lockstep.AI.Editor
{
    public class EditorMacroUtil
    {

        public static void TogglePureMode(bool isOn)
        {
            EditorMacroUtil.ToggleMacro("LOCKSTEP_PURE_MODE",isOn);
        }

        public static void ToggleMacro(string def, bool isOn) {
            Debug.Log("ToggleMacro " + def + " = " + isOn);
#if UNITY_EDITOR
            ToggleMacro(def, isOn, (int) BuildTargetGroup.Standalone);
            ToggleMacro(def, isOn, (int) BuildTargetGroup.Android);
            ToggleMacro(def, isOn, (int) BuildTargetGroup.iOS);
#endif
        }

        static void ToggleMacro(string def, bool isOn, int type) {
#if UNITY_EDITOR
            var targetGroup = (BuildTargetGroup) type;
            string ori = PlayerSettings.GetScriptingDefineSymbolsForGroup(targetGroup);
            List<string> defineSymbols = new List<string>(ori.Split(';'));
            if (isOn) {
                if (!defineSymbols.Contains(def)) {
                    defineSymbols.Add(def);
                }
            }
            else {
                defineSymbols.Remove(def);
            }

            PlayerSettings.SetScriptingDefineSymbolsForGroup(targetGroup, string.Join(";", defineSymbols.ToArray()));
#endif
        }
    }
}