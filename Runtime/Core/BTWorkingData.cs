using System;
using System.Collections.Generic;
using UnityEngine;

namespace Lockstep.AI
{
    public unsafe partial class BTWorkingData 
    {
#if !LOCKSTEP_PURE_MODE
        // Blackboard
        [Header("Blackboard")] [SerializeReference]
        public Blackboard Blackboard;
        public void SetValue(string key, int value) => Blackboard.SetValue(key, value);
        public int GetValue(string key, int defaultValue) => Blackboard.GetValue(key, defaultValue);

        public void SetValue(string key, long value) => Blackboard.SetValue(key, value);
        public long GetValue(string key, long defaultValue) => Blackboard.GetValue(key, defaultValue);

        public void SetValue(string key, bool value) => Blackboard.SetValue(key, value);
        public bool GetValue(string key, bool defaultValue) => Blackboard.GetValue(key, defaultValue);

        public void SetValue(string key, float value) => Blackboard.SetValue(key, value);
        public float GetValue(string key, float defaultValue) => Blackboard.GetValue(key, defaultValue);

        public void SetValue(string key, Vector2 value) => Blackboard.SetValue(key, value);
        public Vector2 GetValue(string key, Vector2 defaultValue) => Blackboard.GetValue(key, defaultValue);
        public void SetValue(string key, Vector3 value) => Blackboard.SetValue(key, value);
        public Vector3 GetValue(string key, Vector3 defaultValue) => Blackboard.GetValue(key, defaultValue);

        partial void _Awake(){
            Blackboard = new Blackboard();
            Blackboard.DoInit((_info.Config as BTGraph).blackboardKeys);
        }
#endif
    }

    public unsafe partial class BTWorkingData 
    {
        private BTInfo _info;

        // Tree
        private byte* _treePtr = null;
        private ushort[] _treeOffset;
        private int _treeSize = 0;
        
        // bb
        private Dictionary<string, ushort> _bbKey2Offset => _info.BlackboardOffsets;
        private int _bbSize => _info.BlackboardSize;
        private byte* _bbPtr;

        [Header("RunTime")] 
        public float DeltaTime;
        
        public T As<T>() where T : BTWorkingData
        {
            return (T)this;
        }
        
        public unsafe void* GetContext(int idx)
        {
            var offset = _treeOffset[idx];
            Debug.Assert(offset >= 0 && offset < _treeSize, " out of range");
            return _treePtr + offset;
        }

        public void Awake(BTInfo info)
        {
            _info = info;
            _treePtr = NativeHelper.AllocAndZero(info.BlackboardSize + info.TreeSize);
            _treeOffset = info.TreeOffsets;
            _treeSize = info.TreeSize;
            _Awake();
        }

        partial void _Awake();

        public void Destroy()
        {
            if (_treePtr != null)
            {
                NativeHelper.Free(new IntPtr(_treePtr));
                _treePtr = null;
            }
        }

        public bool HasValue(string key) =>
#if LOCKSTEP_PURE_MODE
            _bbKey2Offset.ContainsKey(key);
#else
            Blackboard.HasValue(key);
#endif
        public void SetValue<T>(string key, T value) where T : unmanaged
        {
#if DEBUG
            Debug.Assert(_bbKey2Offset.ContainsKey(key) && _bbKey2Offset[key] + sizeof(T) < _bbSize,
                $"Can not find a key{key} or out of range {_bbSize} type ={typeof(T).Name}");
#endif
            var offset = _bbKey2Offset[key];
            *(T*)(_bbPtr + offset) = value;
        }

        public T GetValue<T>(string key, T defaultValue) where T : unmanaged
        {
#if DEBUG
            Debug.Assert(_bbKey2Offset.ContainsKey(key) && _bbKey2Offset[key] + sizeof(T) < _bbSize,
                $"Can not find a key{key} or out of range {_bbSize} type ={typeof(T).Name}");
#endif

            var offset = _bbKey2Offset[key];
            return *(T*)(_bbPtr + offset);
        }


    }
}