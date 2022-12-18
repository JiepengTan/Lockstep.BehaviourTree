using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using GraphProcessor;

namespace Lockstep.AI
{
	public class BTGraphWindow : BaseGraphWindow
	{
		BaseGraph tmpGraph;
		BTToolbarView toolbarView;

		[MenuItem("Window/05 All Combined")]
		public static BaseGraphWindow OpenWithTmpGraph()
		{
			var graphWindow = CreateWindow<BTGraphWindow>();

			// When the graph is opened from the window, we don't save the graph to disk
			graphWindow.tmpGraph = ScriptableObject.CreateInstance<BaseGraph>();
			graphWindow.tmpGraph.hideFlags = HideFlags.HideAndDontSave;
			graphWindow.InitializeGraph(graphWindow.tmpGraph);

			graphWindow.Show();

			return graphWindow;
		}

		protected override void OnDestroy()
		{
			graphView?.Dispose();
			DestroyImmediate(tmpGraph);
		}

		protected override void InitializeWindow(BaseGraph graph)
		{
			titleContent = new GUIContent("All Graph");

			if (graphView == null)
			{
				graphView = new BTGraphView(this,graph);
				graphView.Add(new MiniMapView(graphView));
				graphView.Add(new BTToolbarView(graphView));
			}

			rootView.Add(graphView);
		}

		private void OnInspectorUpdate()
		{
			var view =graphView as BTGraphView;
			view?.OnStep();
		}

		protected override void InitializeGraphView(BaseGraphView view)
		{
			view.OpenPinned<ExposedParameterView>();
		}
	}
}