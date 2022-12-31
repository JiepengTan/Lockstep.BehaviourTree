using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Lockstep.AI
{
    public partial class Blackboard
    {
        [SerializeReference] public List<BlackboardKey> Keys;
        private Dictionary<string, BlackboardKey> _items;

        public Blackboard()
        {
            _items = new Dictionary<string, BlackboardKey>();
        }


        private bool TryGetValue(string key,out BlackboardKey item)
        {
            if (!_items.TryGetValue(key, out item))
            {
                item = new BlackboardKey();
                item.name = key;
                _items[key] = item;
                return false;
            }
            return true;
        }


        private BlackboardKey ForceGetValue(string key)
        {
            if (!_items.TryGetValue(key, out var item))
            {
                item = new BlackboardKey();
                item.name = key;
                _items[key] = item;
            }
            return item;
        }

        public bool HasValue(string key) => _items.ContainsKey(key);
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
                    if (item.type == type)
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
                var item= ForceGetValue(key);
                item.type = type;
                item.SetValue(type,value);
                return true;
            }
        }

        public BlackboardKey Find(string keyName)
        {
            return Keys.Find((key) => { return key.name == keyName; });
        }
        
        public void SetValue(string key, int value) => ForceGetValue(key).intValue = value;
        public int GetValue(string key, int defaultValue) {if (TryGetValue(key, out var value)) return value.intValue; value.intValue = defaultValue; return defaultValue; }
        
        public void SetValue(string key, long value) => ForceGetValue(key).longValue = value;
        public long GetValue(string key, long defaultValue) {if (TryGetValue(key, out var value)) return value.longValue; value.longValue = defaultValue; return defaultValue; }

        public void SetValue(string key, bool value) => ForceGetValue(key).booleanValue = value;
        public bool GetValue(string key, bool defaultValue) {if (TryGetValue(key, out var value)) return value.booleanValue; value.booleanValue = defaultValue; return defaultValue; }
        
        public void SetValue(string key, float value) => ForceGetValue(key).floatValue = value;
        public float GetValue(string key, float defaultValue) {if (TryGetValue(key, out var value)) return value.floatValue; value.floatValue = defaultValue; return defaultValue; }
        
        public void SetValue(string key, Vector2 value) => ForceGetValue(key).vector2Value = value;
        public Vector2 GetValue(string key, Vector2 defaultValue) {if (TryGetValue(key, out var value)) return value.vector2Value; value.vector2Value = defaultValue; return defaultValue; }
        
        public void SetValue(string key, Vector3 value) => ForceGetValue(key).vector3Value = value;
        public Vector3 GetValue(string key, Vector3 defaultValue) {if (TryGetValue(key, out var value)) return value.vector3Value; value.vector3Value = defaultValue; return defaultValue; }
        public void SetValue(string key, string value) => ForceGetValue(key).stringValue = value;
        public string GetValue(string key, string defaultValue) {if (TryGetValue(key, out var value)) return value.stringValue; value.stringValue = defaultValue; return defaultValue; }

        public void SetValue(string key, GameObject value) => ForceGetValue(key).gameObjectValue = value;
        public GameObject GetValue(string key, GameObject defaultValue) {if (TryGetValue(key, out var value)) return value.gameObjectValue; value.gameObjectValue = defaultValue; return defaultValue; }
        
        public void SetValue(string key, LayerMask value) => ForceGetValue(key).layerMaskValue = value;
        public LayerMask GetValue(string key, LayerMask defaultValue) {if (TryGetValue(key, out var value)) return value.layerMaskValue; value.layerMaskValue = defaultValue; return defaultValue; }

    }
}