using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lockstep.AI {

    [System.Serializable]
    public class FloatVar {

        [SerializeReference]
        private BlackboardKey key;

        public float Value {
            get {
                return key.FloatValue;
            }
            set {
                key.FloatValue = value;
            }
        }
    }

    [System.Serializable]
    public class IntVar {

        [SerializeReference]
        private BlackboardKey key;

        public int Value {
            get {
                return key.IntValue;
            }
            set {
                key.IntValue = value;
            }
        }
    }
    [System.Serializable]
    public class LongVar {

        [SerializeReference]
        private BlackboardKey key;

        public long Value {
            get {
                return key.LongValue;
            }
            set {
                key.LongValue = value;
            }
        }
    }
    [System.Serializable]
    public class BoolVar {

        [SerializeReference]
        private BlackboardKey key;

        public bool Value {
            get {
                return key.BooleanValue;
            }
            set {
                key.BooleanValue = value;
            }
        }
    }


    [System.Serializable]
    public class Vector2Var {

        [SerializeReference]
        private BlackboardKey key;

        public Vector2 Value {
            get {
                return key.Vector2Value;
            }
            set {
                key.Vector2Value = value;
            }
        }
    }

    [System.Serializable]
    public class Vector3Var {

        [SerializeReference]
        private BlackboardKey key;

        public Vector3 Value {
            get {
                return key.Vector3Value;
            }
            set {
                key.Vector3Value = value;
            }
        }
    }

}