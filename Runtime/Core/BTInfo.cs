using System;
using System.Collections.Generic;
using Lockstep.Serialization;
using UnityEngine;

namespace Lockstep.AI {

    public class BTInfo
    {
        [Header("Config")] 
        public object Config;
        
        [Header("Tree")] 
        public ushort[] TreeOffsets;
        public ushort TreeSize;
        public BTNode TreeRoot;
        
        [Header("Blackboard")]
        public Dictionary<string, ushort> BlackboardOffsets = new Dictionary<string, ushort>();
        public ushort BlackboardSize;
        
        public void Serialize(Serializer writer)
        {
            List<BTNode> tempNodes = new List<BTNode>();
            var root = TreeRoot;
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
            tempNodes.Clear();
            for (int i = 0; i < nodes.Count; i++)
            {
                tempNodes.Add(null);
            }
            foreach (var node in nodes)
            {
                tempNodes[node.IndexInTree] = node;
            }
            writer.Write(nodes.Count);
            foreach (var node in tempNodes)
            {
                writer.Write(node.TypeId);
                node.Serialize(writer);
            }
            
            foreach (var node in tempNodes)
            {
                var count = node.GetChildCount();
                writer.Write((byte)count);
                for (int i = 0; i < count; i++)
                {
                    writer.Write(node.GetChild(i).IndexInTree);
                }
            }
            
            writer.Write(TreeSize);
            writer.Write(TreeOffsets);

            writer.Write(BlackboardSize);
            var keyCount = this.BlackboardOffsets.Count;
            writer.Write((byte)keyCount);
            foreach (var pair in BlackboardOffsets)
            {
                writer.Write(pair.Key);
                writer.Write(pair.Value);
            }
        }


        public void Deserialize(Deserializer reader)
        {
            List<BTNode> tempNodes = new List<BTNode>();
            var count = reader.ReadInt32();
            tempNodes.Clear();
            for (int i = 0; i < count; i++)
            {
                tempNodes.Add(null);
            }
            for (int i = 0; i < count; i++)
            {
                var type = reader.ReadUInt16();
                var node = BTNodeFactory.CreateNode(type);
                node.Deserialize(reader);
                tempNodes[node.IndexInTree] = node;
            }
            foreach (var node in tempNodes)
            {
                var num = reader.ReadByte();
                for (int i = 0; i < num; i++)
                {
                    node.AddChild(tempNodes[reader.ReadUInt16()]);
                }
            }
            TreeRoot = tempNodes[0];
            TreeSize = reader.ReadUInt16();
            TreeOffsets = reader.ReadArray(TreeOffsets);

            // dict
            BlackboardSize = reader.ReadUInt16();
            var keyCount = reader.ReadByte();
            BlackboardOffsets.Clear();
            for (int i = 0; i < keyCount; i++)
            {
                var key = reader.ReadString();
                var val = reader.ReadUInt16();
                BlackboardOffsets[key] = val;
            }
        }
    }
}