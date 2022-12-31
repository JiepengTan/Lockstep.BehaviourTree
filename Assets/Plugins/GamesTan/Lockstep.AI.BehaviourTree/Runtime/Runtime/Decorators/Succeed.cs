using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lockstep.AI {
    [System.Serializable]
    public class Succeed : DecoratorNode {
        protected override void OnStart() {
        }

        protected override void OnStop() {
        }

        protected override State OnUpdate() {
            var state = child.Update();
            if (state == State.Failure) {
                return State.Success;
            }
            return state;
        }
    }
}