using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Lockstep.AI
{
    public unsafe partial class BTNode
    {
#if DEBUG
        protected string _name;
        public string name
        {
            get { return _name; }
            set { _name = value; }
        }
#endif

        protected virtual int MemSize => 0;

        protected int _uniqueKey;

        private const int defaultChildCount = -1;
        protected List<BTNode> _children;
        protected int _maxChildCount;

        public BTNode(int maxChildCount = -1)
        {
            if (maxChildCount != 0) _children = new List<BTNode>();
            if (maxChildCount > 0)
            {
                _children.Capacity = maxChildCount;
            }

            _maxChildCount = maxChildCount;
        }

        public int Update(BTWorkingData wData)
        {
            return OnUpdate(wData);
        }

        public void Transition(BTWorkingData wData)
        {
            OnTransition(wData);
        }


        public override int GetHashCode()
        {
            return _uniqueKey;
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
            return OnEvaluate(wData);
        }

        protected virtual bool OnEvaluate( /*in*/ BTWorkingData wData)
        {
            return true;
        }

        public BTNode()
            : this(defaultChildCount)
        {
        }

        ~BTNode()
        {
            _children = null;
        }

        //-------------------------------------------------------------------
        public BTNode AddChild(BTNode node)
        {
            if (_maxChildCount >= 0 && _children.Count >= _maxChildCount)
            {
                TLogger.WARNING("**BT** exceeding child count");
                return this;
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

        public int GetTotalNodeCount()
        {
            int sum = 0;
            if (_children != null)
            {
                foreach (var child in _children)
                {
                    sum += child.GetTotalNodeCount();
                }
            }

            return sum + 1;
        }

        public int GetTotalMemSize()
        {
            int sum = 0;
            if (_children != null)
            {
                foreach (var child in _children)
                {
                    sum += child.GetTotalMemSize();
                }
            }

            return sum + MemSize;
        }

        public int[] GetTotalOffsets()
        {
            var nodes = new List<BTNode>();
            Flatten(nodes);
            var offsets = new int[nodes.Count];
            for (int i = 0; i < nodes.Count; i++)
            {
                Debug.Assert(nodes[i]._uniqueKey == i, "Error: Idx not match");
            }

            var offset = 0;
            for (int i = 0; i < nodes.Count; i++)
            {
                offsets[i] = offset;
                offset += nodes[i].MemSize;
            }

            return offsets;
        }

        protected virtual void Flatten(List<BTNode> nodes)
        {
            _uniqueKey = nodes.Count;
            nodes.Add(this);
            if (_children != null)
            {
                foreach (var child in _children)
                {
                    child.Flatten(nodes);
                }
            }
        }
    }
}