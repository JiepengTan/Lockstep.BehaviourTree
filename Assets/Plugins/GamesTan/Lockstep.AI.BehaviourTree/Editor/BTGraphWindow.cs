using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using GraphProcessor;
using UnityEngine.UIElements;

namespace Lockstep.AI.Editor
{
	public class BTGraphWindow : BaseGraphWindow
	{
		
		public VisualTreeAsset behaviourTreeXml;
		public VisualTreeAsset nodeXml;
		public StyleSheet behaviourTreeStyle;
		
		BaseGraph tmpGraph;
		BTToolbarView toolbarView;


		BTInspectorView inspectorView;
		BTBlackboardView blackboardView;
		Label titleLabel;
		Label versionLabel;
		protected BTGraphView treeView => graphView as BTGraphView;
		
		protected override void OnDestroy()
		{
			graphView?.Dispose();
			DestroyImmediate(tmpGraph);
		}

		protected override void InitializeWindow(BaseGraph graph)
		{
			var root = rootView;
			if (graphView == null)
			{
				titleContent = new GUIContent("All Graph");
				// Import UXML
				var visualTree = behaviourTreeXml;
				visualTree.CloneTree(root);
				graphView = root.Q<BTGraphView>();
				graphView.DoInit(this,graph);
				graphView.Add(new MiniMapView(graphView));
				graphView.Add(new BTToolbarView(graphView));
				inspectorView = root.Q<BTInspectorView>();
				blackboardView = root.Q<BTBlackboardView>();
				titleLabel = root.Q<Label>("TitleLabel");
				versionLabel = root.Q<Label>("Version");
			}

		}
		//void OnNodeSelectionChanged(NodeView node) {
		//	//inspectorView.UpdateSelection(serializer, node);
		//}


		protected override void InitializeGraphView(BaseGraphView view)
		{
		}


		protected override void OnEnable() {
			base.OnEnable();
            EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
            EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
        }

		protected override void OnDisable() {
			base.OnDisable();
            EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
        }

        private void OnPlayModeStateChanged(PlayModeStateChange obj) {
            switch (obj) {
                case PlayModeStateChange.EnteredEditMode:
                    EditorApplication.delayCall += OnSelectionChange;
                    break;
                case PlayModeStateChange.ExitingEditMode:
                    break;
                case PlayModeStateChange.EnteredPlayMode:
                    EditorApplication.delayCall += OnSelectionChange;
                    break;
                case PlayModeStateChange.ExitingPlayMode:
                    inspectorView?.Clear();
                    break;
            }
        }

        private void OnSelectionChange() {
            if (Selection.activeGameObject) {
                BehaviourTreeRunner runner = Selection.activeGameObject.GetComponent<BehaviourTreeRunner>();
                if (runner) {
                    SelectTree(runner.tree);
                }
            }
        }

        private void OnInspectorUpdate() {
	        if (Application.isPlaying) {
		        treeView?.UpdateNodeStates();
	        }
        }
        [HideInInspector]
        public BehaviourTree tree;
        public BTSerializedBehaviourTree serializer;
        void SelectTree(BehaviourTree newTree) {
            if (!newTree) {
                ClearSelection();
                return;
            }

            tree = newTree;
            serializer = new BTSerializedBehaviourTree(newTree);

            if (titleLabel != null) {
                string path = AssetDatabase.GetAssetPath(serializer.tree);
                if (path == "") {
                    path = serializer.tree.name;
                }
                titleLabel.text = $"TreeView ({path})";
            }

            treeView?.PopulateView(serializer);
            blackboardView?.Bind(serializer);
        }

        void ClearSelection() {
            tree = null;
            serializer = null;
            inspectorView.Clear();
            treeView.ClearView();
            blackboardView.ClearView();
        }

        void ClearIfSelected(string path) {
            if (serializer == null) {
                return;
            }

            if (AssetDatabase.GetAssetPath(serializer.tree) == path) {
                // Need to delay because this is called from a will delete asset callback
                EditorApplication.delayCall += () => {
                    SelectTree(null);
                };
            }
        }

        //void OnNodeSelectionChanged(NodeView node) {
        //    inspectorView.UpdateSelection(serializer, node);
        //}
	}
}