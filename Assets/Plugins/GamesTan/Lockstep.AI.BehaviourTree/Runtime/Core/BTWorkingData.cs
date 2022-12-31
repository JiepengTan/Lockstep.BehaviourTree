using System;
using System.Collections.Generic;
using UnityEngine;

namespace Lockstep.AI {
    public unsafe partial class BTWorkingData : TAny {
        ~BTWorkingData(){
            if (_pDatas != null) {
                NativeHelper.Free(new IntPtr(_pDatas));
                _pDatas = null;
            }
        }

        public Blackboard Blackboard => _blackboard;
        private Blackboard _blackboard;
        private byte* _pDatas = null;
        private int[] _dataOffset;
        private int _dataLen = 0;
        public float DeltaTime;
        
        public unsafe void* GetContext(int idx){
            var offset = _dataOffset[idx];
            Debug.Assert(offset >= 0 && offset < _dataLen, " out of range");
            return _pDatas + offset;
        }
        public void Init(int[] offsets, int totalMemSize){
            _pDatas = NativeHelper.AllocAndZero(totalMemSize);
            _dataOffset = offsets;
            _dataLen = totalMemSize;
            _blackboard = new Blackboard();
        }

        public BTWorkingData Clone(){
            var ret = new BTWorkingData();
            ret.Init(this._dataOffset, this._dataLen);
            NativeHelper.MemCpy(this._pDatas, ret._pDatas, _dataLen);
            // TODO clone Blackboard
            return ret;
        }
                
        public void SetValue(string key,EBlackboardKeyType type, object value,bool isForce = false) => _blackboard.SetValue(key,type,value,isForce);
        public BlackboardKey GetValue(string key) => _blackboard.GetValue(key);
        
        public void SetValue(string key, int value) => _blackboard.SetValue(key,value);
        public int GetValue(string key, int defaultValue) => _blackboard.GetValue(key,defaultValue);
        
        public void SetValue(string key, long value) => _blackboard.SetValue(key,value);
        public long GetValue(string key, long defaultValue) =>_blackboard.GetValue(key,defaultValue);

        public void SetValue(string key, bool value) => _blackboard.SetValue(key,value);
        public bool GetValue(string key, bool defaultValue) =>_blackboard.GetValue(key,defaultValue);
        
        public void SetValue(string key, float value) => _blackboard.SetValue(key,value);
        public float GetValue(string key, float defaultValue) =>_blackboard.GetValue(key,defaultValue);
        
        public void SetValue(string key, Vector2 value) => _blackboard.SetValue(key,value);
        public Vector2 GetValue(string key, Vector2 defaultValue) =>_blackboard.GetValue(key,defaultValue);
        
        public void SetValue(string key, Vector3 value) => _blackboard.SetValue(key,value);
        public Vector3 GetValue(string key, Vector3 defaultValue) =>_blackboard.GetValue(key,defaultValue);
        public void SetValue(string key, string value) => _blackboard.SetValue(key,value);
        public string GetValue(string key, string defaultValue) =>_blackboard.GetValue(key,defaultValue);

        public void SetValue(string key, GameObject value) => _blackboard.SetValue(key,value);
        public GameObject GetValue(string key, GameObject defaultValue) =>_blackboard.GetValue(key,defaultValue);
        
        public void SetValue(string key, LayerMask value) => _blackboard.SetValue(key,value);
        public LayerMask GetValue(string key, LayerMask defaultValue) =>_blackboard.GetValue(key,defaultValue);

    }
}