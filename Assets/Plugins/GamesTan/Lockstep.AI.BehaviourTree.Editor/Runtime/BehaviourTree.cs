namespace Lockstep.AI
{
    public class BehaviourTree<T> where T:BTWorkingData,new()
    {
        protected T _workingData;
        protected BTNode _bt;
        public T WorkingData => _workingData;
        protected object _transform;

#if UNITY_EDITOR
        private bool _isDebuging => _transform != null &&
                                              object.ReferenceEquals(UnityEditor.Selection.activeTransform, _transform);
#else
        private bool _isDebuging => false;
#endif
        public T DoAwake( object config, object transform) 
        {
            _transform = transform;
            var btInfo = BTFactory.GetOrCreateInfo(config);
            _bt = btInfo.RootNode;
            _workingData = new T();
            _workingData.Init(btInfo.Offsets, btInfo.MemSize);
            return _workingData;
        }


        public T GetValue<T>(string key, T defaultValue)
        {
            return _workingData.GetValue<T>(key, defaultValue);
        }

        public void SetValue(string key, object defaultValue)
        {
            _workingData.SetValue(key, defaultValue);
        }

        public void Reset()
        {
            _bt.Transition(_workingData);
        }

        public void DoUpdate(float deltaTime)
        {
            WorkingData.deltaTime = deltaTime;
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