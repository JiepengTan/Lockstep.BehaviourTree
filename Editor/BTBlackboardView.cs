using System;
using System.Collections;
using System.Collections.Generic;
using Lockstep.AI;
using UnityEngine.UIElements;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;

namespace Lockstep.AI.Editor {
    public class BTBlackboardView : VisualElement {

        public new class UxmlFactory : UxmlFactory<BTBlackboardView, VisualElement.UxmlTraits> { }

        private BTSerializedBehaviourTree serializer;

        private ListView listView;
        private TextField newKeyTextField;
        private EnumField newKeyEnumField;


        private Button createButton;

        internal void Bind(BTSerializedBehaviourTree serializer) {
            
            this.serializer = serializer;

            listView = this.Q<ListView>("ListView_Keys");
            newKeyTextField = this.Q<TextField>("TextField_KeyName");
            newKeyEnumField = this.Q<EnumField>("EnumField_KeyType");
            createButton = this.Q<Button>("Button_KeyCreate");

            // ListView
            listView.BindProperty(serializer.BlackboardKeys);
            listView.makeItem = MakeItem;
            listView.bindItem = BindItem;

            // TextField
            newKeyTextField.RegisterCallback<ChangeEvent<string>>((evt) => {
                ValidateButton();
            });

            // EnumField
            newKeyEnumField.Init(EBlackboardKeyType.Float);

            // Button
            createButton.clicked -= CreateNewKey;
            createButton.clicked += CreateNewKey;

            ValidateButton();
        }

        public void BindProperty(SerializedProperty keys = null)
        {
            serializer.RebindProperties(keys);
            listView.BindProperty(serializer.BlackboardKeys);
            listView.makeItem = MakeItem;
            listView.bindItem = BindItem;
        }
  
        private void ValidateButton() {
            // Disable the create button if trying to create a non-unique key
            bool isValidKeyText = ValidateKeyText(newKeyTextField.text);
            createButton.SetEnabled(isValidKeyText);
        }

        bool ValidateKeyText(string text) {
            if (text == "") {
                return false;
            }

            bool keyExists = serializer.tree.blackboardKeys.Find((a)=>a.Name==newKeyTextField.text) != null;
            return !keyExists;
        }

        void BindItem(VisualElement item, int index)
        {
            Label label = item.Q<Label>();
            if(index>=serializer.BlackboardKeys.arraySize) return;
            var keyProp = serializer.BlackboardKeys.GetArrayElementAtIndex(index);
            var keyName = keyProp.FindPropertyRelative(nameof(BlackboardKey.Name));
            label.BindProperty(keyName);

            BTBlackboardValueField valueField = item.Q<BTBlackboardValueField>();
            valueField.BindProperty(keyProp);
        }

        VisualElement MakeItem() {

            VisualElement container = new VisualElement();
            container.style.flexGrow = 1.0f;
            container.style.flexDirection = FlexDirection.Row;

            Label keyField = new Label();
            keyField.style.width = 120.0f;

            BTBlackboardValueField valueField = new BTBlackboardValueField();
            valueField.style.flexGrow = 1.0f;

            container.Add(keyField);
            container.Add(valueField);

            container.AddManipulator(new ContextualMenuManipulator((ContextualMenuPopulateEvent evt) => {
                evt.menu.AppendAction($"Delete Key", (a) => {
                    Label label = container.Q<Label>();
                    DeleteKey(label.text);
                });
            }));

            return container;
        }

        void CreateNewKey() {
            serializer.CreateBlackboardKey(newKeyTextField.text, (EBlackboardKeyType)newKeyEnumField.value);
            ValidateButton();
        }

        void DeleteKey(string keyName) {
            serializer.DeleteBlackboardKey(keyName);
        }

        public void ClearSelection() {
            BindProperty();
        }
    }
}