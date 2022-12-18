using System.Runtime.InteropServices;
using UnityEngine.Serialization;

namespace Lockstep.AI {
    [System.Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = NativeHelper.STRUCT_PACK)]
    public unsafe partial struct BTCActionNonPrioritizedSelector {
        [FormerlySerializedAs("currentSelectedIndex")] public int CurrentSelectedIndex;
        [FormerlySerializedAs("lastSelectedIndex")] public int LastSelectedIndex;

        public BTCActionNonPrioritizedSelector(int curIdx = -1, int lastIdx = -1){
            CurrentSelectedIndex = curIdx;
            LastSelectedIndex = lastIdx;
        }
    }
}