using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Lockstep.Serialization;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Lockstep.AI
{

    
    public unsafe partial class BTNode
    {
        public virtual ushort TypeId=>0;
        public virtual ushort MemSize => 0;
        protected UInt16 _indexInTree;
        protected List<BTNode> _children;
        public UInt16 IndexInTree => _indexInTree;
        
        public int Update(BTWorkingData wData)
        {
            var state= OnUpdate(wData);
            __DebugSetUpdateState(state);
            return state;
        }

        public void Transition(BTWorkingData wData)
        {
            OnTransition(wData);
        }


        public override int GetHashCode()
        {
            return _indexInTree;
        }


        protected virtual int OnUpdate(BTWorkingData wData)
        {
            return BTRunningStatus.FINISHED;
        }

        protected virtual void OnTransition(BTWorkingData wData)
        {
        }

        public virtual bool Evaluate( /*in*/ BTWorkingData wData)
        {
            var result= OnEvaluate(wData);
            __DebugSetEvaluateState(result);
            return result;
        }

        protected virtual bool OnEvaluate( /*in*/ BTWorkingData wData)
        {
            return true;
        }

        public BTNode()
        {
        }

        ~BTNode()
        {
            _children = null;
        }

        //-------------------------------------------------------------------
        public BTNode AddChild(BTNode node)
        {
            if (_children == null )
            {
                _children = new List<BTNode>();
            }
            _children.Add(node);
            return this;
        }

        public void SortChildren()
        {
            if(_children == null) return;
            _children.Sort((a,b)=>a.position.xMin .CompareTo(b.position.xMin)  );
        }

        public void CleanChildren()
        {
            if (_children != null) _children.Clear();
        }

        public int GetChildCount()
        {
            if (_children == null) return 0;
            return _children.Count;
        }

        public bool IsIndexValid(int index)
        {
            return index >= 0 && _children != null && index < _children.Count;
        }

        public BTNode GetChild(int index)
        {
            if (index < 0 || _children == null || index >= _children.Count)
            {
                return null;
            }
            return _children[index];
        }



        public static List<BTNode> Flatten(BTNode root)
        {
            var nodes = new List<BTNode>();
            Queue<BTNode> expendingNodes = new Queue<BTNode>();
            expendingNodes.Enqueue(root);
            int idx = 0;
            while (expendingNodes.Count > 0)
            {
                var node = expendingNodes.Dequeue();
                node._indexInTree = (ushort)idx++;
                nodes.Add(node);
                var count = node.GetChildCount();
                for (int i = 0; i < count; i++)
                {
                    expendingNodes.Enqueue(node.GetChild(i));
                }
            }
            return nodes;
        }
    }
}