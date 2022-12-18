using System.Runtime.InteropServices;
using UnityEngine.Serialization;

namespace Lockstep.AI {
    [System.Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = NativeHelper.STRUCT_PACK)]
    public unsafe partial struct BTCActionSequence 
    {
        public int CurrentSelectedIndex;
        public BTCActionSequence(int idx = -1)
        {
            CurrentSelectedIndex = idx;
        }
    }
}