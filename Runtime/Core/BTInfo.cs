using System;
using System.Collections.Generic;
using Lockstep.Serialization;
using UnityEngine;

namespace Lockstep.AI {

    public class BTInfo {
        public BTNode RootNode;
        public ushort[] Offsets;
        public int MemSize;
        
        private static List<BTNode> _tempNodes = new List<BTNode>();
        public void Serialize(Serializer writer)
        {
            var root = RootNode;
            List<BTNode> nodes = new List<BTNode>();
            Queue<BTNode> expendingNodes = new Queue<BTNode>();
            expendingNodes.Enqueue(root);
            while (expendingNodes.Count > 0)
            {
                var node = expendingNodes.Dequeue();
                nodes.Add(node);
                var cnt = node.GetChildCount();
                for (int i = 0; i < cnt; i++)
                {
                    expendingNodes.Enqueue(node.GetChild(i));
                }
            }
            _tempNodes.Clear();
            for (int i = 0; i < nodes.Count; i++)
            {
                _tempNodes.Add(null);
            }
            foreach (var node in nodes)
            {
                _tempNodes[node.IndexInTree] = node;
            }
            writer.Write(nodes.Count);
            foreach (var node in _tempNodes)
            {
                writer.Write(node.TypeId);
                node.Serialize(writer);
            }
            
            foreach (var node in _tempNodes)
            {
                var count = node.GetChildCount();
                writer.Write((byte)count);
                for (int i = 0; i < count; i++)
                {
                    writer.Write(node.GetChild(i).IndexInTree);
                }
            }
            
            writer.Write(MemSize);
            writer.Write(Offsets);
        }


        public void Deserialize(Deserializer reader)
        {
            var count = reader.ReadInt32();
            _tempNodes.Clear();
            for (int i = 0; i < count; i++)
            {
                _tempNodes.Add(null);
            }
            for (int i = 0; i < count; i++)
            {
                var type = reader.ReadUInt16();
                var node = BTNodeFactory.CreateNode(type);
                node.Deserialize(reader);
                _tempNodes[node.IndexInTree] = node;
            }
            foreach (var node in _tempNodes)
            {
                var num = reader.ReadByte();
                for (int i = 0; i < num; i++)
                {
                    node.AddChild(_tempNodes[reader.ReadUInt16()]);
                }
            }
            RootNode = _tempNodes[0];
            MemSize = reader.ReadInt32();
            Offsets = reader.ReadArray(Offsets);
        }
    }
}