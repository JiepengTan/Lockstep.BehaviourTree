using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;


namespace Lockstep.AI.Editor {
    public class BTBlackboardValueField : VisualElement {

        SerializedProperty property;

        public void BindProperty(SerializedProperty key) {

            property = key;
            string keyName = key.FindPropertyRelative(nameof(BlackboardKey.Name)).stringValue;
            EBlackboardKeyType keyType = (EBlackboardKeyType)key.FindPropertyRelative(nameof(BlackboardKey.Type)).enumValueIndex;

            Clear();
            switch (keyType) {
                case EBlackboardKeyType.Float:
                    FloatField floatField = new FloatField();
                    floatField.BindProperty(property.FindPropertyRelative(nameof(BlackboardKey.FloatValue)));
                    Add(floatField);
                    break;
                case EBlackboardKeyType.Int:
                    IntegerField intField = new IntegerField();
                    intField.BindProperty(property.FindPropertyRelative(nameof(BlackboardKey.IntValue)));
                    Add(intField);
                    break;
                case EBlackboardKeyType.Long:
                    LongField longField = new LongField();
                    longField.BindProperty(property.FindPropertyRelative(nameof(BlackboardKey.LongValue)));
                    Add(longField);
                    break;
                case EBlackboardKeyType.Boolean:
                    Toggle boolField = new Toggle();
                    boolField.BindProperty(property.FindPropertyRelative(nameof(BlackboardKey.BooleanValue)));
                    Add(boolField);
                    break;
                case EBlackboardKeyType.Vector2:
                    Vector2Field vector2Field = new Vector2Field();
                    vector2Field.BindProperty(property.FindPropertyRelative(nameof(BlackboardKey.Vector2Value)));
                    Add(vector2Field);
                    break;
                case EBlackboardKeyType.Vector3:
                    Vector3Field vector3Field = new Vector3Field();
                    vector3Field.BindProperty(property.FindPropertyRelative(nameof(BlackboardKey.Vector3Value)));
                    Add(vector3Field);
                    break;
               
                default:
                    Debug.LogError($"Unhandled type '{keyType}' for key {keyName}");
                    break;
            }
        }
    }
}