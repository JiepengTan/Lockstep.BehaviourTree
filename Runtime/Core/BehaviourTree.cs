using UnityEngine;

namespace Lockstep.AI
{
    public interface IMonoBehaviourTree
    {
        object EditorBlackboardProperty { get; }
    }

    public class BehaviourTree
    {
        [SerializeReference] protected BTWorkingData _workingData;
        protected BTNode _bt;
        public BTWorkingData WorkingData => _workingData;
        public Blackboard Blackboard => _workingData.Blackboard;
        public BTNode Root =>_bt;
        protected object _transform;

#if UNITY_EDITOR
        private bool _isDebuging => _transform != null &&
                                              object.ReferenceEquals(UnityEditor.Selection.activeTransform, _transform);

        public BTGraph Config;
#else
        private bool _isDebuging => false;
#endif
        public T DoAwake<T>( object config, object transform) where T:BTWorkingData,new()
        {
            Config = config as BTGraph;
            _transform = transform;
            var btInfo = BTFactory.GetOrCreateInfo(config);
            _bt = btInfo.RootNode;
            _workingData = new T();
            _workingData.Init(btInfo.Offsets, btInfo.MemSize);
            _workingData.Blackboard.DoInit(Config.blackboardKeys);
            return _workingData as T;
        }

        public void Reset()
        {
            _bt.Transition(_workingData);
        }

        public void DoUpdate(float deltaTime)
        {
            WorkingData.DeltaTime = deltaTime;
            if (_isDebuging)
            {
                BTNode.__DebugStartRecordInfo();
            }

            if (_bt.Evaluate(_workingData))
            {
                _bt.Update(_workingData);
            }
            else
            {
                _bt.Transition(_workingData);
            }

            if (_isDebuging)
            {
                BTNode.__DebugStopRecordInfo();
            }
        }
    }
}