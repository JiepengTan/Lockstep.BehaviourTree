using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using GraphProcessor;
using UnityEditor.Callbacks;
using System.IO;

namespace Lockstep.AI.Editor
{
	public class NodeGraphAssetCallbacks
	{
		[MenuItem("Assets/Create/NodeGraph/BTGraph", false, 10)]
		public static void CreateGraphPorcessor()
		{
			var graph = ScriptableObject.CreateInstance<BTGraph>();
			ProjectWindowUtil.CreateAsset(graph, "GraphProcessor.asset");
		}

		[OnOpenAsset(0)]
		public static bool OnBaseGraphOpened(int instanceID, int line)
		{
			var asset = UnityEditor.EditorUtility.InstanceIDToObject(instanceID) as BTGraph;
			if (asset != null)
			{
				EditorWindow.GetWindow<BTGraphWindow>().InitializeGraph(asset as BaseGraph);
				return true;
			}

			return false;
		}
	}
}