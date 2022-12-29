using System.Collections.Generic;
using UnityEngine;
using GraphProcessor;
using UnityEngine.Events;

namespace Lockstep.AI.Test
{
	[System.Serializable, NodeMenuItem("Test/UnityTypeTestNode")]
	public class UnityTypeTestNode : BaseNode
	{
		[Input(name = "In")] public float input;

		[Output(name = "Out")] public float output;

		public UnityEvent evt;

		[Output(name = "Out2"), SerializeField, Vertical]
		public GameObject output2;

		public override string name => "Unity Event Node";
		public List<GameObject> objs = new List<GameObject>();

		[Input(name = "In"), SerializeField] public Vector4 vetical;

		public enum Test1
		{
			A,
			B,
			C,
			D
		}

		public enum Test2
		{
			T1,
			T2,
			T3,
		}

		public Test1 t1;

		[VisibleIf(nameof(t1), Test1.A)] public float f1;
		[VisibleIf(nameof(t1), Test1.B)] public int f2;

		[VisibleIf(nameof(t1), Test1.C)] public string s1;
		[VisibleIf(nameof(t1), Test1.C)] public Test2 t2;

		protected override void Process()
		{
			output = input * 42;
		}
	}
}
