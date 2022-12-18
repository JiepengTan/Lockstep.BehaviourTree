using System;
using System.Collections.Generic;

namespace Lockstep.AI
{
    [Serializable, GraphProcessor.NodeMenuItem("BTComposite/Selector")]
    public unsafe partial class BTActionNonSelector : BTActionSelector
    {
        public BTCActionNonPrioritizedSelector __content;
        protected override int MemSize => sizeof(BTCActionNonPrioritizedSelector);
        protected override bool OnEvaluate(/*in*/BTWorkingData wData)
        {
            var thisContext = (BTCActionNonPrioritizedSelector*) wData.GetContext(_uniqueKey);
            //check last node first
            if (IsIndexValid(thisContext->CurrentSelectedIndex)) {
                var node = GetChild(thisContext->CurrentSelectedIndex);
                if (node.Evaluate(wData)) {
                    return true;
                }
            }
            return base.OnEvaluate(wData);
        }
    }
}
