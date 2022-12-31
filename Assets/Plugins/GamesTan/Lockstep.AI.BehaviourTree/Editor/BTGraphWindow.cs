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
			view?.UpdateRuntimeStatus();
		}

		protected override void InitializeGraphView(BaseGraphView view)
		{
			view.OpenPinned<ExposedParameterView>();
		}
	}
}