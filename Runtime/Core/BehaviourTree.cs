using UnityEngine;

namespace Lockstep.AI
{
    public interface IMonoBehaviourTree
    {
        object EditorBlackboardProperty { get; }
    }

    public class BehaviourTree
    {
        [SerializeReference] 
        public BTWorkingData WorkingData;
        protected BTNode _bt;
        protected object _transform;

#if UNITY_EDITOR
        private bool _isDebuging => _transform != null &&
                                              object.ReferenceEquals(UnityEditor.Selection.activeTransform, _transform);

        public BTGraph Config;
#else
        private bool _isDebuging => false;
#endif
        public T DoAwake<T>(object transform,int id,byte[] bytes ) where T : BTWorkingData, new()
        {
            var btInfo = BTFactory.GetOrCreateInfo(id,bytes);
            return Init<T>(transform, btInfo);
        }

        public T DoAwake<T>(object transform,object config ) where T:BTWorkingData,new()
        {
#if !LOCKSTEP_PURE_MODE
            Config = config as BTGraph;
            var btInfo = BTFactory.GetOrCreateInfo(config);
            return Init<T>(transform, btInfo);
#else
            return default(T);
#endif
        }

        private T Init<T>(object transform, BTInfo btInfo) where T : BTWorkingData, new()
        {
            _transform = transform;
            _bt = btInfo.TreeRoot;
            WorkingData = new T();
            WorkingData.Awake(btInfo);
            return WorkingData as T;
        }

        public void Reset()
        {
            _bt.Transition(WorkingData);
        }

        public void DoUpdate(float deltaTime)
        {
            WorkingData.DeltaTime = deltaTime;
#if !LOCKSTEP_PURE_MODE
            if (_isDebuging)
            {
                BTNode.__DebugStartRecordInfo();
            }
#endif

            if (_bt.Evaluate(WorkingData))
            {
                _bt.Update(WorkingData);
            }
            else
            {
                _bt.Transition(WorkingData);
            }

#if !LOCKSTEP_PURE_MODE
            if (_isDebuging)
            {
                BTNode.__DebugStopRecordInfo();
            }
#endif
        }
    }
}