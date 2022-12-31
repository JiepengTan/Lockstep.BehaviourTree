using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lockstep.AI {
    [System.Serializable]
    public class Log : ActionNode
    {
        public string message;

        protected override void OnStart() {
        }

        protected override void OnStop() {
        }

        protected override State OnUpdate() {
            Debug.Log($"{message}");
            return State.Success;
        }
    }
}
