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

        private BlackBoard _blackboard;
        private byte* _pDatas = null;
        private int[] _dataOffset;
        private int _dataLen = 0;
        public float deltaTime;
        
        public unsafe void* GetContext(int idx){
            var offset = _dataOffset[idx];
            Debug.Assert(offset >= 0 && offset < _dataLen, " out of range");
            return _pDatas + offset;
        }
        public T GetValue<T>(string key, T defaultValue)
        {
            return _blackboard.GetValue<T>(key, defaultValue);
        }
        public void SetValue(string key, object defaultValue)
        {
            _blackboard.SetValue(key, defaultValue);
        }     
        public void Init(int[] offsets, int totalMemSize){
            _pDatas = NativeHelper.AllocAndZero(totalMemSize);
            _dataOffset = offsets;
            _dataLen = totalMemSize;
            _blackboard = new BlackBoard();
        }

        public BTWorkingData Clone(){
            var ret = new BTWorkingData();
            ret.Init(this._dataOffset, this._dataLen);
            NativeHelper.MemCpy(this._pDatas, ret._pDatas, _dataLen);
            // TODO clone Blackboard
            return ret;
        }
    }
}