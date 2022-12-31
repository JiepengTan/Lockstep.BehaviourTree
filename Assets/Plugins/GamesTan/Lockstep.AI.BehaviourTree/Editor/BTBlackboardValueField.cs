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

            string keyName = key.FindPropertyRelative("name").stringValue;
            EBlackboardKeyType keyType = (EBlackboardKeyType)key.FindPropertyRelative("type").enumValueIndex;

            Clear();

            switch (keyType) {
                case EBlackboardKeyType.Float:
                    FloatField floatField = new FloatField();
                    floatField.BindProperty(property.FindPropertyRelative("floatValue"));
                    Add(floatField);
                    break;
                case EBlackboardKeyType.Int:
                    IntegerField intField = new IntegerField();
                    intField.BindProperty(property.FindPropertyRelative("intValue"));
                    Add(intField);
                    break;
                case EBlackboardKeyType.Long:
                    LongField longField = new LongField();
                    longField.BindProperty(property.FindPropertyRelative("longValue"));
                    Add(longField);
                    break;
                case EBlackboardKeyType.Boolean:
                    Toggle boolField = new Toggle();
                    boolField.BindProperty(property.FindPropertyRelative("booleanValue"));
                    Add(boolField);
                    break;
                case EBlackboardKeyType.String:
                    TextField stringField = new TextField();
                    stringField.BindProperty(property.FindPropertyRelative("stringValue"));
                    Add(stringField);
                    break;
                case EBlackboardKeyType.Vector2:
                    Vector2Field vector2Field = new Vector2Field();
                    vector2Field.BindProperty(property.FindPropertyRelative("vector2Value"));
                    Add(vector2Field);
                    break;
                case EBlackboardKeyType.Vector3:
                    Vector3Field vector3Field = new Vector3Field();
                    vector3Field.BindProperty(property.FindPropertyRelative("vector3Value"));
                    Add(vector3Field);
                    break;
                case EBlackboardKeyType.GameObject:
                    ObjectField gameObjectField = new ObjectField();
                    gameObjectField.objectType = typeof(GameObject);
                    gameObjectField.allowSceneObjects = false;
                    gameObjectField.BindProperty(property.FindPropertyRelative("gameObjectValue"));
                    Add(gameObjectField);
                    break;
                case EBlackboardKeyType.Tag:
                    TagField tagField = new TagField();
                    tagField.BindProperty(property.FindPropertyRelative("stringValue"));
                    Add(tagField);
                    break;
                case EBlackboardKeyType.LayerMask:
                    LayerMaskField layerMaskField = new LayerMaskField();
                    layerMaskField.BindProperty(property.FindPropertyRelative("layerMaskValue"));
                    Add(layerMaskField);
                    break;
                default:
                    Debug.LogError($"Unhandled type '{keyType}' for key {keyName}");
                    break;
            }
        }
    }
}