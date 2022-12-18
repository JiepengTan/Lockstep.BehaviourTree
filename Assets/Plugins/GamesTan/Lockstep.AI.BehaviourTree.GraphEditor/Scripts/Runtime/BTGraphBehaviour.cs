using UnityEngine;
using GraphProcessor;

[ExecuteAlways]
public class BTGraphBehaviour : MonoBehaviour
{
    public BaseGraph graph;

    protected virtual void OnEnable()
    {
        if (graph == null)
            graph = ScriptableObject.CreateInstance<BaseGraph>();

        graph.LinkToScene(gameObject.scene);
    }
}
