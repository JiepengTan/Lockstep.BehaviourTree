using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

[CustomEditor(typeof(BTGraphBehaviour))]
public class BTGraphBehaviourEditor : Editor
{
    Editor graphEditor;
    BTGraphBehaviour behaviour => target as BTGraphBehaviour;

    void OnEnable()
    {
        graphEditor = Editor.CreateEditor(behaviour.graph);
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