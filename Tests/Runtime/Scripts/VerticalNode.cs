using UnityEngine;
using GraphProcessor;

namespace Lockstep.AI.Test
{
	[System.Serializable, NodeMenuItem("Test/Vertical")]
	public class VerticalNode : BaseNode
	{
		[Input, Vertical] public float input;

		[Output, Vertical] public float output;
		[Output, Vertical] public float output2;
		[Output, Vertical] public float output3;

		public override string name => "Vertical";

		protected override void Process()
		{
			output = input * 42;
		}
	}
}