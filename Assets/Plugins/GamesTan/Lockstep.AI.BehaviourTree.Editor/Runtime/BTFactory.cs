﻿using System;
using System.Collections.Generic;
using System.Linq;
using Lockstep.AI;
using UnityEngine;

namespace Lockstep.AI
{
    public static class BTFactory
    {
        private static Dictionary<BTGraph, BTInfo> _config2Infos = new Dictionary<BTGraph, BTInfo>();
        public static BTInfo GetOrCreateInfo(object obj)
        {
            var config = obj as BTGraph;
            if (_config2Infos.TryGetValue(config, out var info)) return info;
            info = CreateBtInfo(config);
            _config2Infos[config] = info;
            return info;
        }

        static BTInfo CreateBtInfo(BTGraph config)
        {
            BTNode bt = null;
            var nodes = config.nodes.Select(a => a as BTNode).ToList();
            var edges = config.edges;
            foreach (var node in nodes)
            {
                node.CleanChildren();
            }

            HashSet<BTNode> childNodes = new HashSet<BTNode>();
            foreach (var edge in edges)
            {
                var child = edge.inputNode as BTNode;
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

            return CreateBtInfo(bt);
        }

        static BTInfo CreateBtInfo(BTNode bt)
        {
            var offsets = bt.GetTotalOffsets();
            var memSize = bt.GetTotalMemSize();
            return new BTInfo()
            {
                MemSize = memSize,
                Offsets = offsets,
                RootNode = bt,
            };
        }
    }
}