namespace Lockstep.AI {
    //---------------------------------------------------------------
    public abstract unsafe partial class BTCondition : BTNode
    {
        public bool IsInvert;

        public override bool Evaluate( /*in*/ BTWorkingData wData)
        {
            var result= IsInvert ^OnEvaluate(wData);          
#if !LOCKSTEP_PURE_MODE  
            __DebugSetEvaluateState( result);
#endif
            return result;
        }
    }
}