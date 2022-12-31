using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using GraphProcessor;
using UnityEngine.UIElements;

namespace Lockstep.AI
{
	public class BTGraphWindow : BaseGraphWindow
	{
		
		public VisualTreeAsset behaviourTreeXml;
		public VisualTreeAsset nodeXml;
		public StyleSheet behaviourTreeStyle;
		
		BaseGraph tmpGraph;
		BTToolbarView toolbarView;


		public InspectorView inspectorView;
		public BlackboardView blackboardView;
		protected override void OnDestroy()
		{
			graphView?.Dispose();
			DestroyImmediate(tmpGraph);
		}

		protected override void InitializeWindow(BaseGraph graph)
		{
			var root = rootView;
			if (graphView == null)
			{
				titleContent = new GUIContent("All Graph");
				// Import UXML
				var visualTree = behaviourTreeXml;
				visualTree.CloneTree(root);
				graphView = root.Q<BTGraphView>();
				graphView.DoInit(this,graph);
				graphView.Add(new MiniMapView(graphView));
				graphView.Add(new BTToolbarView(graphView));
				inspectorView = root.Q<InspectorView>();
				blackboardView = root.Q<BlackboardView>();
			}

		}
		void OnNodeSelectionChanged(NodeView node) {
			//inspectorView.UpdateSelection(serializer, node);
		}
		private void OnInspectorUpdate()
		{
			var view =graphView as BTGraphView;
			view?.UpdateRuntimeStatus();
		}

		protected override void InitializeGraphView(BaseGraphView view)
		{
		}
	}
}