using System;
using System.Collections.Generic;

namespace Lockstep.AI {
    
    [Serializable, GraphProcessor.NodeMenuItem("BTComposite/PrioritizedSelector")]
    public unsafe partial class BTActionSelector : BTActionComposite {   
  
        public BTCActionSelector __content;
        protected override int MemSize => sizeof(BTCActionSelector);
        public BTActionSelector()
            : base(-1){ }

        protected override bool OnEvaluate( /*in*/ BTWorkingData wData){
            var thisContext = (BTCActionSelector*) wData.GetContext(_uniqueKey);
            thisContext->CurrentSelectedIndex = -1;
            int childCount = GetChildCount();
            for (int i = 0; i < childCount; ++i) {
                var node = GetChild(i);
                if (node.Evaluate(wData)) {
                    thisContext->CurrentSelectedIndex = i;
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