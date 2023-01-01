using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Lockstep.AI.Editor {

    // This is a helper class which wraps a serialized object for finding properties on the behaviour.
    // It's best to modify the behaviour tree via SerializedObjects and SerializedProperty interfaces
    // to keep the UI in sync, and undo/redo
    // It's a hodge podge mix of various functions that will evolve over time. It's not exhaustive by any means.
    [System.Serializable]
    public class BTSerializedBehaviourTree {

        // Wrapper serialized object for writing changes to the behaviour tree
        public SerializedObject serializedObject;

        public BTGraph tree;

        // Property names. These correspond to the variable names on the behaviour tree
        const string sPropRootNode = "rootNode";
        const string sPropNodes = "nodes";
        const string sPropBlackboard = "blackboardKeys";
        bool batchMode = false;


        public SerializedProperty BlackboardKeys;

        public void RebindProperties(SerializedProperty keys =null)
        {
            if (keys == null)
            {
                RebindProperties(serializedObject.FindProperty(sPropBlackboard));
            }
            else
            {
                this.BlackboardKeys = keys;
            }

        }

        public BTSerializedBehaviourTree(BTGraph tree)
        {
            serializedObject = new SerializedObject(tree);
            this.tree = tree;
            RebindProperties();
        }


        public void CreateBlackboardKey(string keyName, EBlackboardKeyType keyType) {
            SerializedProperty keysArray = BlackboardKeys;
            keysArray.InsertArrayElementAtIndex(keysArray.arraySize);
            SerializedProperty newKey = keysArray.GetArrayElementAtIndex(keysArray.arraySize - 1);

            BlackboardKey key = new BlackboardKey();
            key.Name = keyName;
            key.Type = keyType;
            newKey.managedReferenceValue = key;

            ApplyChanges();
        }

        public void DeleteBlackboardKey(string keyName) {
            SerializedProperty keysArray = BlackboardKeys;
            for(int i = 0; i < keysArray.arraySize; ++i) {
                var key = keysArray.GetArrayElementAtIndex(i);
                BlackboardKey itemKey = key.managedReferenceValue as BlackboardKey;
                if (itemKey.Name == keyName) {
                    keysArray.DeleteArrayElementAtIndex(i);
                    ApplyChanges();
                    return;
                }
            }
        }

        public void BeginBatch() {
            batchMode = true;
        }

        public void ApplyChanges() {
            if (!batchMode) {
                serializedObject?.ApplyModifiedProperties();
            }
        }

        public void EndBatch() {
            batchMode = false;
            ApplyChanges();
        }
    }
}
