//#define LOCKSTEP_PURE_MODE
using System.Collections.Generic;
using UnityEngine;

namespace Lockstep.AI
{
    public unsafe partial class Blackboard
    {

        public void DoInit(List<BlackboardKey> rawData)
        {
            _DoInit(rawData);
        }

        partial void _DoInit(List<BlackboardKey> rawData);
    }

    public unsafe partial  class Blackboard
    {
        [SerializeReference] public List<BlackboardKey> Keys;
        private Dictionary<string, BlackboardKey> _items;
        partial void _DoInit(List<BlackboardKey> rawData)
        {
            Keys = new List<BlackboardKey>();
            _items = new Dictionary<string, BlackboardKey>();
            foreach (var item in rawData)
            {
                Keys.Add(item.Clone());
            }

            foreach (var item in Keys)
            {
                _items[item.Name] = item;
            }
        }
        
        public bool HasValue(string key) => _items.ContainsKey(key);
        
        public void SetValue(string key, int value) => ForceGetValue(key,EBlackboardKeyType.Int).IntValue = value;
        public int GetValue(string key, int defaultValue) {if (TryGetValue(key, out var value)) return value.IntValue; value.IntValue = defaultValue; return defaultValue; }
        
        public void SetValue(string key, long value) => ForceGetValue(key,EBlackboardKeyType.Long).LongValue = value;
        public long GetValue(string key, long defaultValue) {if (TryGetValue(key, out var value)) return value.LongValue; value.LongValue = defaultValue; return defaultValue; }

        public void SetValue(string key, bool value) => ForceGetValue(key,EBlackboardKeyType.Boolean).BooleanValue = value;
        public bool GetValue(string key, bool defaultValue) {if (TryGetValue(key, out var value)) return value.BooleanValue; value.BooleanValue = defaultValue; return defaultValue; }
        
        public void SetValue(string key, float value) => ForceGetValue(key,EBlackboardKeyType.Float).FloatValue = value;
        public float GetValue(string key, float defaultValue) {if (TryGetValue(key, out var value)) return value.FloatValue; value.FloatValue = defaultValue; return defaultValue; }
        
        public void SetValue(string key, Vector2 value) => ForceGetValue(key,EBlackboardKeyType.Vector2).Vector2Value = value;
        public Vector2 GetValue(string key, Vector2 defaultValue) {if (TryGetValue(key, out var value)) return value.Vector2Value; value.Vector2Value = defaultValue; return defaultValue; }
        
        public void SetValue(string key, Vector3 value) => ForceGetValue(key,EBlackboardKeyType.Vector3).Vector3Value = value;
        public Vector3 GetValue(string key, Vector3 defaultValue) {if (TryGetValue(key, out var value)) return value.Vector3Value; value.Vector3Value = defaultValue; return defaultValue; }



        private bool TryGetValue(string key,out BlackboardKey item)
        {
            if (!_items.TryGetValue(key, out item))
            {
                item = new BlackboardKey();
                item.Name = key;
                _items[key] = item;
                Keys.Add(item);
                return false;
            }
            return true;
        }


        private BlackboardKey ForceGetValue(string key,EBlackboardKeyType type = EBlackboardKeyType.Float)
        {
            if (!_items.TryGetValue(key, out var item))
            {
                item = new BlackboardKey();
                item.Name = key;
                item.Type = type;
                _items[key] = item;
                Keys.Add(item);
            }
            return item;
        }

        public BlackboardKey GetValue(string key)
        {
            if (_items.TryGetValue(key, out var item))
            {
                return item;
            }
            return null;
        }

        public bool SetValue(string key,EBlackboardKeyType type, object value,bool isForce)
        {
            if (!isForce)
            {
                if (TryGetValue(key, out var item))
                {
                    if (item.Type == type)
                    {
                        item.SetValue(type, value);
                        return true;
                    }
                    return false;

                }
                return false;
            }
            else
            {
                var item= ForceGetValue(key,type);
                item.Type = type;
                item.SetValue(type,value);
                return true;
            }
        }

    }
}