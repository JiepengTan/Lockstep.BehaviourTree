using System;
using System.Collections.Generic;
using System.Linq;
using Lockstep.AI;
using UnityEngine;

namespace AIToolkitDemo
{
    public  class AIEntityBehaviorTreeFactory {
        private static BTInfo _bevTreeDemo1;

        private static Dictionary<BTGraph, BTInfo> graph2Infos = new Dictionary<BTGraph, BTInfo>();
        public static BTInfo GetBehaviorTreeDemo1(BTGraph config)
        {
            if (graph2Infos.TryGetValue(config, out var info)) return info;
            info = CreateBtInfo(config);
            graph2Infos[config] = info;
            return info;
        }

        static BTInfo CreateBtInfo(BTGraph _config)
        {
            BTNode bt = null;
            var config = ScriptableObject.Instantiate(_config);
            var nodes = config.nodes.Select(a => a as BTNode).ToList();
            var edges = config.edges;
            foreach (var node in nodes)
            {
                node.CleanChildren();
            }

            HashSet<BTNode> childNodes = new HashSet<BTNode>();
            foreach (var edge in edges)
            {
                var child  = edge.inputNode as BTNode;
                var parent = edge.outputNode as BTNode;
                parent.AddChild(child);
                childNodes.Add(child);
            }

            foreach (var node in nodes)
            {
                if (!childNodes.Contains(node))
                {
                    bt = node as BTNode;
                    break;
                }
            }

            int UniqueKey = 0;
            foreach (var node in nodes)
            {
                node.SortChildren();
            }
            return BTFactory.CreateBtInfo(bt);
        }

        
        

        public static T Create<T>(Action<T> func = null) where T : BTNode, new(){
            var val= BTFactory.CreateNode<T>();
            if (func != null)
            {
                func(val);
            }

            return val;
        }

    }
}