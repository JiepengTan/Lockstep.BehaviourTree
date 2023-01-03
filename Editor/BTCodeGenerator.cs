using GraphProcessor;
using Unity.Plastic.Antlr3.Runtime.Misc;
using UnityEngine;

namespace Lockstep.AI.Editor
{
    public class BTCodeGenerator
    {
        public static void GenCode(CodeGenInfo info)
        {
            CodeGenBTInjecter.GenCode(info);
            CodeGenBTSerialization.GenCode(info);
        }

    }


}