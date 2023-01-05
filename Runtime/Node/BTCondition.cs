﻿namespace Lockstep.AI {
    //---------------------------------------------------------------
    public abstract unsafe partial class BTCondition : BTNode
    {
        public bool IsInvert;

        public override bool Evaluate( /*in*/ BTWorkingData wData)
        {
            var result= IsInvert ^OnEvaluate(wData);
            __DebugSetEvaluateState( result);
            return result;
        }
    }
}