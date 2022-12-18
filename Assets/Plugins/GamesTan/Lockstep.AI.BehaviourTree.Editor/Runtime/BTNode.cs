using GraphProcessor;
using Unity.Collections;

namespace Lockstep.AI
{
    public unsafe partial class BTActionLeaf
    {
        [Input, Vertical] public bool __input;
    }
    public unsafe partial class BTActionComposite
    {
        [Input, Vertical] public bool __input;
        [Output, Vertical] public bool __output;
    }

    public abstract unsafe partial class BTPrecondition
    {
        [Input, Vertical] public bool __input;
    }

    public unsafe partial class BTNode : BaseNode
    {
    }
}