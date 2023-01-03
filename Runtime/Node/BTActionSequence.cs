using System;
using System.Runtime.InteropServices;

namespace Lockstep.AI
{
    [System.Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = NativeHelper.STRUCT_PACK)]
    public unsafe partial struct BTCActionSequence 
    {
        public int CurrentSelectedIndex;
        public BTCActionSequence(int idx = -1)
        {
            CurrentSelectedIndex = idx;
        }
    }

    [Serializable, GraphProcessor.NodeMenuItem("BTComposite/Sequence")]
    public unsafe partial class BTActionSequence : BTActionComposite
    {
        protected override int MemSize => sizeof(BTCActionSequence);
        //-------------------------------------------------------
        private bool _continueIfErrorOccors;
        //-------------------------------------------------------
        public BTActionSequence()
            : base(-1)
        {
            _continueIfErrorOccors = false;
        }
        public BTActionSequence SetContinueIfErrorOccors(bool v)
        {
            _continueIfErrorOccors = v;
            return this;
        }
        //------------------------------------------------------
        protected override bool OnEvaluate(/*in*/BTWorkingData wData)
        {
            var thisContext = (BTCActionSequence*)wData.GetContext(_uniqueKey);
            int checkedNodeIndex = -1;
            if (IsIndexValid(thisContext->CurrentSelectedIndex)) {
                checkedNodeIndex = thisContext->CurrentSelectedIndex;
            } else {
                checkedNodeIndex = 0;
            }
            if (IsIndexValid(checkedNodeIndex)) {
                var node = GetChild(checkedNodeIndex);
                if (node.Evaluate(wData)) {
                    thisContext->CurrentSelectedIndex = checkedNodeIndex;
                    return true;
                }
            }
            return false;
        }
        protected override int OnUpdate(BTWorkingData wData)
        {
            var thisContext = (BTCActionSequence*)wData.GetContext(_uniqueKey);
            int runningStatus = BTRunningStatus.FINISHED;
            var node = GetChild(thisContext->CurrentSelectedIndex);
            runningStatus = node.Update(wData);
            if (_continueIfErrorOccors == false && BTRunningStatus.IsError(runningStatus)) {
                thisContext->CurrentSelectedIndex = -1;
                return runningStatus;
            }
            if (BTRunningStatus.IsFinished(runningStatus)) {
                thisContext->CurrentSelectedIndex++;
                if (IsIndexValid(thisContext->CurrentSelectedIndex)) {
                    runningStatus = BTRunningStatus.EXECUTING;
                } else {
                    thisContext->CurrentSelectedIndex = -1;
                }
            }
            return runningStatus;
        }
        protected override void OnTransition(BTWorkingData wData)
        {
            var thisContext = (BTCActionSequence*)wData.GetContext(_uniqueKey);
            var node = GetChild(thisContext->CurrentSelectedIndex);
            if (node != null) {
                node.Transition(wData);
            }
            thisContext->CurrentSelectedIndex = -1;
        }
    }
}
