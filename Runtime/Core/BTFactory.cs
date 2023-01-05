using System;
using System.Collections.Generic;
using System.Linq;
using Lockstep.AI;
using Lockstep.Serialization;
using UnityEngine;

namespace Lockstep.AI
{
    public static class BTFactory
    {
        private static Dictionary<BTGraph, BTInfo> _config2Infos = new Dictionary<BTGraph, BTInfo>();
        private static Dictionary<long, BTInfo> _obj2Infos = new Dictionary<long, BTInfo>();

        public static BTInfo GetOrCreateInfo(long id,byte[] bytes)
        {
            if (_obj2Infos.TryGetValue(id, out var info)) return info;
             info = Deserialize(bytes);
            _obj2Infos[id] = info;
            return info;
        }

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

            // find the root
            BTNode root = null;
            foreach (var node in nodes)
            {
                if (node is BTActionRoot)
                {
                    root = node;
                    break;
                }
            }

            if (root == null)
            {
                Debug.LogError("Can not find Root node in "+ config.name);
                return null;
            }

            foreach (var node in nodes)
            {
                node.SortChildren();
            }
            return CreateBtInfo(root);
        }

        static BTInfo CreateBtInfo(BTNode bt)
        {
            var nodes = BTNode.Flatten(bt);
            var count = nodes.Count;
            var offsets = new ushort[count];
            ushort offset = 0;
            for (int i = 0; i <count; i++)
            {
                offsets[i] = offset;
                offset += (ushort)nodes[i].MemSize;
            }
            return new BTInfo()
            {
                MemSize = offset,
                Offsets = offsets,
                RootNode = bt,
            };
        }
        public static byte[] Serialize(BTInfo info)
        {
            var writer = new Serializer();
            info.Serialize(writer);
            return  writer.CopyData();
        }
        
        public static BTInfo Deserialize(byte[] bytes)
        {
            var reader = new Deserializer(bytes);
            var info = new BTInfo();
            info.Deserialize(reader);
            return info;
        }
    }
}