using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lockstep.AI {
    public abstract class DecoratorNode : Node {

        [SerializeReference]
        [HideInInspector] 
        public Node child;
    }
}
