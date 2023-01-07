using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Lockstep.AI {

    [System.Serializable]
    public enum EBlackboardKeyType {
        Float,
        Boolean,
        Vector2,
        Vector3,
        Int,
        Long,
    }
    [System.Serializable]
    public class BlackboardKey {
        public string Name;
        public EBlackboardKeyType Type;

        public float FloatValue;
        public int IntValue;
        public long LongValue;
        public bool BooleanValue;
        public Vector2 Vector2Value;
        public Vector3 Vector3Value;

        public unsafe int MemSize
        {
            get
            {
                switch (Type)
                {
                  case EBlackboardKeyType.Boolean: return sizeof(bool);
                  case EBlackboardKeyType.Float: return sizeof(float);
                  case EBlackboardKeyType.Vector2: return sizeof(Vector2);
                  case EBlackboardKeyType.Vector3: return sizeof(Vector3);
                  case EBlackboardKeyType.Int: return sizeof(int);
                  case EBlackboardKeyType.Long: return sizeof(long);
                }

                throw new Exception("Known Types" + Type);
            }
        }

        public BlackboardKey Clone()
        {
            return new BlackboardKey()
            {
                Name = Name,
                Type = Type,
                BooleanValue =  BooleanValue,
                FloatValue =  FloatValue,
                Vector2Value =  Vector2Value,
                Vector3Value =  Vector3Value,
                IntValue =  IntValue,
                LongValue =  LongValue,
            };
        }

        public void SetValue(EBlackboardKeyType type, object val)
        {
            switch (type)
            {
                case EBlackboardKeyType.Boolean: BooleanValue = (bool)val ; break;
                case EBlackboardKeyType.Float: FloatValue = (float)val ; break;
                case EBlackboardKeyType.Vector2: Vector2Value = (Vector2)val ; break;
                case EBlackboardKeyType.Vector3: Vector3Value = (Vector3)val ; break;
                case EBlackboardKeyType.Int: IntValue = (int)val ; break;
                case EBlackboardKeyType.Long: LongValue = (long)val ; break;
            }
        }

    }
}