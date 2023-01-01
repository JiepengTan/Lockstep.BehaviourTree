using System;
using System.Collections.Generic;

namespace Lockstep.AI {

    public abstract unsafe partial class BTAction : BTNode {
        
        //-------------------------------------------------------------
        public BTAction(int maxChildCount)
            : base(maxChildCount){
        }
    }
}