using System.Collections.Generic;
using UnityEngine;

namespace Lockstep.AI
{
    public enum EBuiltinBTNodeType
    {
        BTActionRoot = 0,
        BTActionSequence = 1,
        BTActionSelector = 2,
        BTActionParallel = 3,
        BTActionLoop = 4,
        BTConditionComposite = 5,
        EnumCount
    }
    
    public partial class BTNodeFactory
    {
        public delegate BTNode NodeCreateFunc();

        private static Dictionary<int, NodeCreateFunc> _typeId2FactoryFunc = new Dictionary<int, NodeCreateFunc>();

        static BTNodeFactory()
        {
            _typeId2FactoryFunc = new Dictionary<int, NodeCreateFunc>();
            Register((int)EBuiltinBTNodeType.BTActionRoot, () => new BTActionRoot());
            Register((int)EBuiltinBTNodeType.BTActionSequence, () => new BTActionSequence());
            Register((int)EBuiltinBTNodeType.BTActionSelector, () => new BTActionSelector());
            Register((int)EBuiltinBTNodeType.BTActionLoop, () => new BTActionLoop());
            Register((int)EBuiltinBTNodeType.BTActionParallel, () => new BTActionParallel());
            Register((int)EBuiltinBTNodeType.BTConditionComposite, () => new BTConditionComposite());
        }

        public static void Register(int typeId, NodeCreateFunc func)
        {
#if DEBUG
            if(_typeId2FactoryFunc.ContainsKey(typeId)) Debug.LogError("Register Factory Function more then once Id=" + typeId);
#endif
            _typeId2FactoryFunc[typeId] = func;
        }

        public static BTNode CreateNode(int typeId)
        {
            if (!_typeId2FactoryFunc.ContainsKey(typeId))
            {
                Debug.LogError("Can Not Find BTNode Factory Function Id=" + typeId);
                return null;
            }
            return _typeId2FactoryFunc[typeId]();
        }
    }
}