using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lockstep.AI {

    [System.Serializable]
    public class RootNode : Node {

        [SerializeReference]
        [HideInInspector] 
        public Node child;

        protected override void OnStart() {

        }

        protected override void OnStop() {

        }

        protected override State OnUpdate() {

            return child.Update();
        }
    }
}