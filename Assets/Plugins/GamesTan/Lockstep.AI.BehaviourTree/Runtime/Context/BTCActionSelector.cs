using System.Runtime.InteropServices;
using UnityEngine.Serialization;

namespace Lockstep.AI {
    [System.Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = NativeHelper.STRUCT_PACK)]
    public unsafe partial struct BTCActionSelector {
        public int CurrentSelectedIndex;
        public int LastSelectedIndex;

        public BTCActionSelector(int curIdx = -1, int lastIdx = -1){
            CurrentSelectedIndex = curIdx;
            LastSelectedIndex = lastIdx;
        }
    }
}