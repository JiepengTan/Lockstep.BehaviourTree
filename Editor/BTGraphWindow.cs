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


		BTBlackboardView blackboardView;
		Label titleLabel;
		Label versionLabel;
		private BTSerializedBehaviourTree serializer;
		protected BTGraphView treeView => graphView as BTGraphView;
		
		protected override void OnDestroy()
		{
			graphView?.Dispose();
			DestroyImmediate(tmpGraph);
		}

		protected override void InitializeWindow(BaseGraph graph)
		{
			var root = rootView;
			var config = graph as BTGraph;
			if (graphView == null)
			{
				serializer = new BTSerializedBehaviourTree(config);
				// Import UXML
				var visualTree = behaviourTreeXml;
				visualTree.CloneTree(root);
				graphView = root.Q<BTGraphView>();
				graphView.DoInit(this,graph);
				graphView.Add(new MiniMapView(graphView));
				graphView.Add(new BTToolbarView(graphView));
				
				blackboardView = root.Q<BTBlackboardView>();
				blackboardView.Bind(serializer);
				
				versionLabel = root.Q<Label>("Version");
				titleLabel = root.Q<Label>("TitleLabel");
				if (titleLabel != null)
				{
					string path = AssetDatabase.GetAssetPath(config)??"BehaviourTree";
					titleLabel.text = $"TreeView ({path})";
				}
				titleContent = new GUIContent("All Graph");
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
                    break;
            }
        }

        private void OnSelectionChange() {
            var runner = Selection.activeGameObject?.GetComponent<IMonoBehaviourTree>();
            if (runner != null) {
                SelectTree(runner);
            }
            else
            {
	            ClearSelection();
            }
        }

        private void OnInspectorUpdate() {
	        if (Application.isPlaying) {
		        treeView?.UpdateRuntimeStatus();
	        }
        }
        void SelectTree(IMonoBehaviourTree mono)
        { 
            blackboardView.BindProperty(mono.EditorBlackboardProperty as SerializedProperty);
        }


        void ClearSelection() {
            treeView.ClearSelection();
            blackboardView.ClearSelection();
        }

        //void OnNodeSelectionChanged(NodeView node) {
        //    inspectorView.UpdateSelection(serializer, node);
        //}
	}
}