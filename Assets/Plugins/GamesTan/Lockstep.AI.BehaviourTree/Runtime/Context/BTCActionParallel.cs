using System;
using System.Runtime.InteropServices;

namespace Lockstep.AI {
    [System.Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = NativeHelper.STRUCT_PACK)]
    public unsafe partial struct BTCActionParallel {
        [System.Serializable]
        public struct BTCParallelStatusEval
        {
            public const int Size = 16;
            public fixed bool status[Size] ;
            public bool this[int index] {
                get => status[index];
                set => status[index] = value;
            }
            public void Init(bool val){
                for (int i = 0; i < Size; i++) {
                    status[i] = val;
                }
            }
        }

        [System.Serializable]
        public struct BTCParallelStatusRunning {

            public const int Size = 16;
            public fixed byte status[Size] ;
            public byte this[int index] {
                get => status[index];
                set => status[index] = value;
            }
            public void Init(byte val){
                for (int i = 0; i < Size; i++) {
                    status[i] = val;
                }
            }
        }

        public BTCParallelStatusEval evaluationStatus;
        public BTCParallelStatusRunning StatusRunning;
    }
}