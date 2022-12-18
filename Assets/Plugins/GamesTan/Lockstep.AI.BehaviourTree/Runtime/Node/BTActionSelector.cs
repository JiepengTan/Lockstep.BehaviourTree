using System;
using System.Collections.Generic;

namespace Lockstep.AI {
    
    [Serializable, GraphProcessor.NodeMenuItem("BTComposite/PrioritizedSelector")]
    public unsafe partial class BTActionSelector : BTActionComposite
    {
        public bool IsPriority = true;
        public BTCActionSelector __content;
        protected override int MemSize => sizeof(BTCActionSelector);
        public BTActionSelector()
            : base(-1){ }

        protected override bool OnEvaluate( /*in*/ BTWorkingData wData){
            var thisContext = (BTCActionSelector*) wData.GetContext(_uniqueKey);
            int childCount = GetChildCount();
            var curIdx = thisContext->CurrentSelectedIndex;
            if (IsPriority)
            {
                thisContext->CurrentSelectedIndex = -1;
                curIdx = 0;
            }

            for (int i = 0; i < childCount; ++i)
            {
                var realIdx =(curIdx+ i)%childCount;
                var node = GetChild(realIdx);
                if (node.Evaluate(wData)) {
                    thisContext->CurrentSelectedIndex = realIdx;
                    return true;
                }
            }
            return false;
        }

        protected override int OnUpdate(BTWorkingData wData){
            var thisContext = (BTCActionSelector*) wData.GetContext(_uniqueKey);
            int runningState = BTRunningStatus.FINISHED;
            if (thisContext->CurrentSelectedIndex != thisContext->LastSelectedIndex) {
                if (IsIndexValid(thisContext->LastSelectedIndex)) {
                    var node = GetChild(thisContext->LastSelectedIndex);
                    node.Transition(wData);
                }

                thisContext->LastSelectedIndex = thisContext->CurrentSelectedIndex;
            }

            if (IsIndexValid(thisContext->LastSelectedIndex)) {
                var node = GetChild(thisContext->LastSelectedIndex);
                runningState = node.Update(wData);
                if (BTRunningStatus.IsFinished(runningState)) {
                    thisContext->LastSelectedIndex = -1;
                }
            }

            return runningState;
        }

        protected override void OnTransition(BTWorkingData wData){
            var thisContext = (BTCActionSelector*) wData.GetContext(_uniqueKey);
            var node = GetChild(thisContext->LastSelectedIndex);
            if (node != null) {
                node.Transition(wData);
            }

            thisContext->LastSelectedIndex = -1;
        }
    }
}