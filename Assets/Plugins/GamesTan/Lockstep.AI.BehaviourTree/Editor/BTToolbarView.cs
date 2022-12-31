using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using GraphProcessor;
using Status = UnityEngine.UIElements.DropdownMenuAction.Status;

namespace Lockstep.AI.Editor
{
	public class BTToolbarView : ToolbarView
	{
		public BTToolbarView(BaseGraphView graphView) : base(graphView)
		{
		}

		protected override void AddButtons()
		{
			AddButton("Center", graphView.ResetPositionAndZoom);

			AddButton("Show In Project", () => EditorGUIUtility.PingObject(graphView.graph), false);
			// Add the hello world button on the left of the toolbar
			AddButton("Hello !", () => Debug.Log("Hello World"), left: false);

			//bool conditionalProcessorVisible = graphView.GetPinnedElementStatus< ConditionalProcessorView >() != Status.Hidden;
			//AddToggle("Show Conditional Processor", conditionalProcessorVisible, (v) => graphView.ToggleView< ConditionalProcessorView>());
		}
	}
}