using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Lockstep.AI.Editor
{
    [CustomEditor(typeof(BTGraphBehaviour))]
    public class BTGraphBehaviourEditor :UnityEditor. Editor
    {
        UnityEditor. Editor graphEditor;
        BTGraphBehaviour behaviour => target as BTGraphBehaviour;

        void OnEnable()
        {
            graphEditor = UnityEditor.Editor .CreateEditor(behaviour.graph);
        }

        void OnDisable()
        {
            DestroyImmediate(graphEditor);
        }

        public override VisualElement CreateInspectorGUI()
        {
            var root = new VisualElement();
            var graphContainer = graphEditor != null ? graphEditor.CreateInspectorGUI().Q("ExposedParameters") : null;

            root.Add(new Button(() => EditorWindow.GetWindow<BTGraphWindow>().InitializeGraph(behaviour.graph))
            {
                text = "Open"
            });

            root.Add(graphContainer);

            return root;
        }
    }
}