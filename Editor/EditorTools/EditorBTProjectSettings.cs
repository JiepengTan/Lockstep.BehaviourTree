using UnityEditor;
using UnityEngine;

namespace Lockstep.AI.Editor
{
    [CustomEditor(typeof(BTProjectSettings))]
    public class EditorBTProjectSettings:UnityEditor. Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            GUILayout.Space(15);
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("1.GenCode"))
            {
                EditorTools.GenCode();
            }
            GUILayout.Space(10);
            if (GUILayout.Button("2.GenData"))
            {
                EditorTools.GenData();
            }
            GUILayout.EndHorizontal();
        }
    }
}