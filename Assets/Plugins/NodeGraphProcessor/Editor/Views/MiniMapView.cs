using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.UIElements;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

namespace GraphProcessor
{
	public class MiniMapView : MiniMap
	{
		new BaseGraphView	graphView;
		Vector2				size;

		public MiniMapView(BaseGraphView baseGraphView)
		{
			this.graphView = baseGraphView;
			var sizePixel = 100;
			SetPosition(new Rect(0,0, sizePixel, sizePixel));
			size = new Vector2(sizePixel, sizePixel);
		}
	}
}