using GraphProcessor;

namespace NodeGraphProcessor.Examples
{
      public enum CompareFunction
      {
        /// <summary>
        ///   <para>Depth or stencil test is disabled.</para>
        /// </summary>
        Disabled,
        /// <summary>
        ///   <para>Never pass depth or stencil test.</para>
        /// </summary>
        Never,
        /// <summary>
        ///   <para>Pass depth or stencil test when new value is less than old one.</para>
        /// </summary>
        Less,
        /// <summary>
        ///   <para>Pass depth or stencil test when values are equal.</para>
        /// </summary>
        Equal,
        /// <summary>
        ///   <para>Pass depth or stencil test when new value is less or equal than old one.</para>
        /// </summary>
        LessEqual,
        /// <summary>
        ///   <para>Pass depth or stencil test when new value is greater than old one.</para>
        /// </summary>
        Greater,
        /// <summary>
        ///   <para>Pass depth or stencil test when values are different.</para>
        /// </summary>
        NotEqual,
        /// <summary>
        ///   <para>Pass depth or stencil test when new value is greater or equal than old one.</para>
        /// </summary>
        GreaterEqual,
        /// <summary>
        ///   <para>Always pass depth or stencil test.</para>
        /// </summary>
        Always,
      }
	[System.Serializable, NodeMenuItem("Test/Comparison")]
	public class Comparison : BaseNode
	{
		[Input(name = "In A")]
		public float    inA;
	
		[Input(name = "In B")]
		public float    inB;

		[Output(name = "Out")]
		public bool		compared;

		public CompareFunction		compareFunction = CompareFunction.LessEqual;

		public override string		name => "Comparison";

		protected override void Process()
		{
			switch (compareFunction)
			{
				default:
				case CompareFunction.Disabled:
				case CompareFunction.Never: compared = false; break;
				case CompareFunction.Always: compared = true; break;
				case CompareFunction.Equal: compared = inA == inB; break;
				case CompareFunction.Greater: compared = inA > inB; break;
				case CompareFunction.GreaterEqual: compared = inA >= inB; break;
				case CompareFunction.Less: compared = inA < inB; break;
				case CompareFunction.LessEqual: compared = inA <= inB; break;
				case CompareFunction.NotEqual: compared = inA != inB; break;
			}
		}
	}
}
