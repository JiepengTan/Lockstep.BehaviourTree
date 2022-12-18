using UnityEngine;
using GraphProcessor;
using System;
using System.Collections.Generic;

namespace Lockstep.AI
{
    public class BTConvertions : ITypeAdapter
    {
        public static Vector4 ConvertFloatToVector4(float from) => new Vector4(from, from, from, from);
        public static float ConvertVector4ToFloat(Vector4 from) => from.x;

        public override IEnumerable<(Type, Type)> GetIncompatibleTypes()
        {
            //yield return (typeof(ConditionalLink), typeof(object));
            yield return (typeof(RelayNode.PackedRelayData), typeof(object));
        }
    }
}