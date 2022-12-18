using System.Runtime.InteropServices;

namespace Lockstep.AI {
    [System.Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = NativeHelper.STRUCT_PACK)]
    public unsafe partial struct BTCActionLeaf {
        public int Status;
        public bool NeedExit;
    }
}