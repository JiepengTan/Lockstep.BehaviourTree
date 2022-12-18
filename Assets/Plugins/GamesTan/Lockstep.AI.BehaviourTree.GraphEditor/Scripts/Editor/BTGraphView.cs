using UnityEngine.UIElements;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using GraphProcessor;
using System;
using UnityEditor;

public class BTGraphView : BaseGraphView
{
	// Nothing special to add for now
	public BTGraphView(EditorWindow window) : base(window) {}

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
		Vector2 position = (evt.currentTarget as VisualElement).ChangeCoordinatesTo(contentViewContainer, evt.localMousePosition);
		evt.menu.AppendAction("New Stack", (e) => AddStackNode(new BaseStackNode(position)), DropdownMenuAction.AlwaysEnabled);
	}


	private void BuildCustomMenu(ContextualMenuPopulateEvent evt)
	{
		evt.menu.AppendSeparator();

		foreach (var nodeMenuItem in NodeProvider.GetNodeMenuEntries())
		{
			var mousePos =
				(evt.currentTarget as VisualElement).ChangeCoordinatesTo(contentViewContainer, evt.localMousePosition);
			Vector2 nodePosition = mousePos;
			evt.menu.AppendAction("Create/" + nodeMenuItem.path,
				(e) => CreateNodeOfType(nodeMenuItem.type, nodePosition),
				DropdownMenuAction.AlwaysEnabled
			);
		}
	}

	void CreateNodeOfType(Type type, Vector2 position)
	{
		RegisterCompleteObjectUndo("Added " + type + " node");
		AddNode(BaseNode.CreateFromType(type, position));
	}
}