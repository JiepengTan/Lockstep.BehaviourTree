using System.Runtime.InteropServices;
using UnityEngine.Serialization;

namespace Lockstep.AI
{
    [System.Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = NativeHelper.STRUCT_PACK)]
    public unsafe partial struct BTCActionLoop 
    {
        public int CurrentIndex;
    }
}