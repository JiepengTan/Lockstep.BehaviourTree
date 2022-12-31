using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lockstep.AI {

    [System.Serializable]
    public enum EBlackboardKeyType {
        Float,
        Boolean,
        Vector2,
        Vector3,
        Int,
        Long,
        LayerMask,
        String, // ref type
        Tag, // ref type
        GameObject,// ref type
    }
    [System.Serializable]
    public class BlackboardKey {
        public string name;
        public EBlackboardKeyType type;

        public void SetValue(EBlackboardKeyType type, object val)
        {
            switch (type)
            {
                case EBlackboardKeyType.Boolean: booleanValue = (bool)val ; break;
                case EBlackboardKeyType.Float: floatValue = (float)val ; break;
                case EBlackboardKeyType.Vector2: vector2Value = (Vector2)val ; break;
                case EBlackboardKeyType.Vector3: vector3Value = (Vector3)val ; break;
                case EBlackboardKeyType.Int: intValue = (int)val ; break;
                case EBlackboardKeyType.Long: longValue = (long)val ; break;
                case EBlackboardKeyType.LayerMask: layerMaskValue = (LayerMask)val ; break;
                case EBlackboardKeyType.String: stringValue = (string)val ; break;
                case EBlackboardKeyType.GameObject: gameObjectValue = (GameObject)val ; break;
                case EBlackboardKeyType.Tag: stringValue = (string)val ; break;
            }
        }

        public float floatValue;
        public int intValue;
        public long longValue;
        public bool booleanValue;
        public Vector2 vector2Value;
        public Vector3 vector3Value;
        public LayerMask layerMaskValue;
        public string stringValue;
        public GameObject gameObjectValue;
    }
}