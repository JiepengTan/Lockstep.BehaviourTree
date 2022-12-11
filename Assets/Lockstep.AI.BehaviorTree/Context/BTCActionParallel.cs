using System;
using System.Runtime.InteropServices;

namespace Lockstep.AI {
    [StructLayout(LayoutKind.Sequential, Pack = NativeHelper.STRUCT_PACK)]
    public unsafe partial struct BTCActionParallel {
        public struct BTCParallelStatusEval
        {
            public const int Size = 16;
            private fixed bool status[Size] ;
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

        public struct BTCParallelStatusRunning {

            public const int Size = 16;
            private fixed byte status[Size] ;
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

        internal BTCParallelStatusEval evaluationStatus;
        internal BTCParallelStatusRunning StatusRunning;
    }
}