using System;
using System.Runtime.InteropServices;

namespace Lockstep.AI
{
    [System.Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = NativeHelper.STRUCT_PACK)]
    public unsafe partial struct BTCActionLoop 
    {
        public int CurrentIndex;
    }
    
    //[Serializable, GraphProcessor.NodeMenuItem("BTComposite/Loop")]
    public unsafe partial class BTActionLoop : BTActionComposite
    {
        public const int INFINITY = -1;
        //--------------------------------------------------------
        private int _loopCount;
        //--------------------------------------------------------
        public BTActionLoop()
            : base(1)
        {
            _loopCount = INFINITY;
        }
        public BTActionLoop SetLoopCount(int count)
        {
            _loopCount = count;
            return this;
        }
        
        //-------------------------------------------------------
        protected override bool OnEvaluate(/*in*/BTWorkingData wData)
        {
            var thisContext = (BTCActionLoop*)wData.GetContext(_uniqueKey);
            bool checkLoopCount = (_loopCount == INFINITY || thisContext->CurrentIndex < _loopCount);
            if (checkLoopCount == false) {
                return false;
            }
            if (IsIndexValid(0)) {
                var node = GetChild(0);
                return node.Evaluate(wData);
            }
            return false;
        }
        protected override int OnUpdate(BTWorkingData wData)
        {
            var thisContext = (BTCActionLoop*)wData.GetContext(_uniqueKey);
            int runningStatus = BTRunningStatus.FINISHED;
            if (IsIndexValid(0)) {
                var node = GetChild(0);
                runningStatus = node.Update(wData);
                if (BTRunningStatus.IsFinished(runningStatus)) {
                    thisContext->CurrentIndex++;
                    if (thisContext->CurrentIndex < _loopCount || _loopCount == INFINITY) {
                        runningStatus = BTRunningStatus.EXECUTING;
                    }
                }
            }
            return runningStatus;
        }
        protected override void OnTransition(BTWorkingData wData)
        {
            var thisContext = (BTCActionLoop*)wData.GetContext(_uniqueKey);
            if (IsIndexValid(0)) {
                var node = GetChild(0);
                node.Transition(wData);
            }
            thisContext->CurrentIndex = 0;
        }
    }
}
