using UnityEngine.UIElements;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using GraphProcessor;
using System;
using UnityEditor;

namespace Lockstep.AI.Editor
{
	public class BTGraphView : BaseGraphView
	{
		public new class UxmlFactory : UxmlFactory<BTGraphView, GraphView.UxmlTraits> { }


		public override void ClearSelection()
		{
			base.ClearSelection();
		}

		public override void DoInit(EditorWindow window, BaseGraph graph)
		{
			Insert(0, new GridBackground());
			base.DoInit(window,graph);
		}

		public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
		{
			BuildStackNodeContextualMenu(evt);
			BuildCustomMenu(evt);
			base.BuildContextualMenu(evt);
		}

		/// <summary>
		/// Add the New Stack entry to the context menu
		/// </summary>
		/// <param name="evt"></param>
		protected void BuildStackNodeContextualMenu(ContextualMenuPopulateEvent evt)
		{
			Vector2 position =
				(evt.currentTarget as VisualElement).ChangeCoordinatesTo(contentViewContainer, evt.localMousePosition);
			evt.menu.AppendAction("New Stack", (e) => AddStackNode(new BaseStackNode(position)),
				DropdownMenuAction.AlwaysEnabled);
		}


		private void BuildCustomMenu(ContextualMenuPopulateEvent evt)
		{
			evt.menu.AppendSeparator();

			foreach (var nodeMenuItem in NodeProvider.GetNodeMenuEntries())
			{
				var isBTNode = typeof(BTNode).IsAssignableFrom(nodeMenuItem.type);
				if (!isBTNode) continue;
				
				var mousePos =
					(evt.currentTarget as VisualElement).ChangeCoordinatesTo(contentViewContainer,
						evt.localMousePosition);
				Vector2 nodePosition = mousePos;

				evt.menu.AppendAction("Create/" + nodeMenuItem.path,
					(e) => CreateNodeOfType(nodeMenuItem.type, nodePosition),
					DropdownMenuAction.AlwaysEnabled
				);
			}
		}
		
		public void UpdateRuntimeStatus()
		{
			var states = BaseNode.__DebugRecordNodesUpdateState;
			foreach (var view in nodeViews)
			{
				if (states.TryGetValue(view.nodeTarget.GUID, out var state))
				{
					view.Highlight();
				}
				else
				{
					view.UnHighlight();
				}
			}
		}
		void CreateNodeOfType(Type type, Vector2 position)
		{
			RegisterCompleteObjectUndo("Added " + type + " node");
			AddNode(BaseNode.CreateFromType(type, position));
		}
	}
}