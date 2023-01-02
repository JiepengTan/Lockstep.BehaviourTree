﻿using System;
using System.Runtime.InteropServices;

namespace Lockstep.AI
{
    


    [System.Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = NativeHelper.STRUCT_PACK)]
    public unsafe partial struct BTCActionRoot {
        public int CurrentSelectedIndex;
        public int LastSelectedIndex;

        public BTCActionRoot(int curIdx = -1, int lastIdx = -1){
            CurrentSelectedIndex = curIdx;
            LastSelectedIndex = lastIdx;
        }
    }
    
    [Serializable, GraphProcessor.NodeMenuItem("BTComposite/BTActionRoot")]
    public unsafe partial class BTActionRoot : BTAction
    {
        public bool IsPriority = true;
        protected override int MemSize => sizeof(BTCActionRoot);
        public BTActionRoot() : base(-1){ }

        protected override bool OnEvaluate( /*in*/ BTWorkingData wData){
            var thisContext = (BTCActionRoot*) wData.GetContext(_uniqueKey);
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
            var thisContext = (BTCActionRoot*) wData.GetContext(_uniqueKey);
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
            var thisContext = (BTCActionRoot*) wData.GetContext(_uniqueKey);
            var node = GetChild(thisContext->LastSelectedIndex);
            if (node != null) {
                node.Transition(wData);
            }

            thisContext->LastSelectedIndex = -1;
        }
    }
}