using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;
using UnityEditor.UIElements;

namespace Lockstep.AI.Editor {

    [CustomPropertyDrawer(typeof(FloatVar))]
    [CustomPropertyDrawer(typeof(IntVar))]
    [CustomPropertyDrawer(typeof(BoolVar))]
    [CustomPropertyDrawer(typeof(StringVar))]
    [CustomPropertyDrawer(typeof(Vector2Var))]
    [CustomPropertyDrawer(typeof(Vector3Var))]
    [CustomPropertyDrawer(typeof(GameObjectVar))]
    [CustomPropertyDrawer(typeof(TagVar))]
    [CustomPropertyDrawer(typeof(LayerMaskVar))]
    public class BTBlackboardKeyPropertyDrawer : PropertyDrawer {

        [SerializeField]
        public VisualTreeAsset asset;

        BehaviourTree tree;
        SerializedProperty itemProp;

        public override VisualElement CreatePropertyGUI(SerializedProperty property) {
            PopupField<string> dropdown = new PopupField<string>();
            return dropdown;
            //tree = property.serializedObject.targetObject as BehaviourTree;
            itemProp = property.FindPropertyRelative("key");

            string currentValue = "null";
            foreach (var key in tree.blackboard.Keys) {
                if (key == itemProp.managedReferenceValue) {
                    currentValue = key.name;
                }
            }

            dropdown.label = property.name;
            dropdown.value = currentValue;
            dropdown.RegisterCallback<ChangeEvent<string>>((evt) => {
                itemProp.managedReferenceValue = tree.blackboard.Find(evt.newValue);
                bool changesApplied = itemProp.serializedObject.ApplyModifiedProperties();
            });

            dropdown.RegisterCallback<MouseEnterEvent>((evt) => {
                var choices = new List<string>();
                foreach (var key in tree.blackboard.Keys) {

                    // Filter out keys of the same type
                    if (!Matches(key.type, fieldInfo.FieldType)) {
                        continue;
                    }

                    choices.Add(key.name);
                }
                dropdown.choices = choices;
            });

            return dropdown;
        }

        bool Matches(EBlackboardKeyType keyType, System.Type propertyType) {
            switch (keyType) {
                case EBlackboardKeyType.Float:
                    return propertyType == typeof(FloatVar);
                case EBlackboardKeyType.Int:
                    return propertyType == typeof(IntVar);
                case EBlackboardKeyType.Long:
                    return propertyType == typeof(IntVar);
                case EBlackboardKeyType.Boolean:
                    return propertyType == typeof(BoolVar);
                case EBlackboardKeyType.Vector2:
                    return propertyType == typeof(Vector2Var);
                case EBlackboardKeyType.Vector3:
                    return propertyType == typeof(Vector3Var);
                case EBlackboardKeyType.LayerMask:
                    return propertyType == typeof(LayerMaskVar);
                case EBlackboardKeyType.GameObject:
                    return propertyType == typeof(GameObjectVar);
                case EBlackboardKeyType.String:
                    return propertyType == typeof(StringVar);
                case EBlackboardKeyType.Tag:
                    return propertyType == typeof(TagVar);
                default:
                    Debug.LogError($"Unhandled Key Type:{keyType}:{propertyType}");
                    return false;
            }
        }

    }

}