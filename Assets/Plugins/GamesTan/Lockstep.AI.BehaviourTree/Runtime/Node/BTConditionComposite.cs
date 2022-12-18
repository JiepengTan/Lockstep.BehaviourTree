using System;

namespace Lockstep.AI
{
    [Serializable,GraphProcessor.NodeMenuItem("Precondition/Composite")]
    public class BTConditionComposite : BTCondition
    {
        public bool IsAllTrue;
        public BTConditionComposite():base(-1){ }

        protected override bool OnEvaluate( /*in*/ BTWorkingData wData)
        {
            var count = _children.Count;
            if (count == 0) return true;
            if (IsAllTrue)
            {
                for (int i = 0; i < count; i++)
                {
                    var val = ((BTCondition)_children[i]).Evaluate(wData);
                    if (!val) return false;
                }

                return true;
            }
            else
            {
                for (int i = 0; i < count; i++)
                {
                    var val = ((BTCondition)_children[i]).Evaluate(wData);
                    if (val) return true;
                }
                return false;
            }
        }
    }
}