using System;
using System.Collections.Generic;

namespace Lockstep.AI {
    public abstract unsafe partial class BTActionLeaf : BTAction {
        private const int ACTION_READY = 0;
        private const int ACTION_RUNNING = 1;
        private const int ACTION_FINISHED = 2;

        protected override int MemSize => sizeof(BTCActionLeaf);
        public BTActionLeaf()
            : base(0){ }

        protected sealed override int OnUpdate(BTWorkingData wData){
            int runningState = BTRunningStatus.FINISHED;
            var thisContext = (BTCActionLeaf*)wData.GetContext(_uniqueKey);
            if (thisContext->Status == ACTION_READY) {
                OnEnter(wData);
                thisContext->NeedExit = true;
                thisContext->Status = ACTION_RUNNING;
            }

            if (thisContext->Status == ACTION_RUNNING) {
                runningState = OnExecute(wData);
                if (BTRunningStatus.IsFinished(runningState)) {
                    thisContext->Status = ACTION_FINISHED;
                }
            }

            if (thisContext->Status == ACTION_FINISHED) {
                if (thisContext->NeedExit) {
                    OnExit(wData, runningState);
                }

                thisContext->Status = ACTION_READY;
                thisContext->NeedExit = false;
            }

            return runningState;
        }

        protected sealed override void OnTransition(BTWorkingData wData){
            var thisContext = (BTCActionLeaf*)wData.GetContext(_uniqueKey);
            if (thisContext->NeedExit) {
                OnExit(wData, BTRunningStatus.TRANSITION);
            }

            thisContext->Status = ACTION_READY;
            thisContext->NeedExit = false;
        }

        protected void* GetUserContextData(BTWorkingData wData){
            return ((byte*)wData.GetContext(_uniqueKey) + sizeof(BTCActionLeaf));
        }

        //--------------------------------------------------------
        // inherented by children-
        protected virtual void OnEnter( /*in*/ BTWorkingData wData){ }

        protected virtual int OnExecute(BTWorkingData wData){
            return BTRunningStatus.FINISHED;
        }

        protected virtual void OnExit(BTWorkingData wData, int runningStatus){ }
    }
}