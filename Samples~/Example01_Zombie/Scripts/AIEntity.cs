using UnityEngine;
using Lockstep.AI;

namespace AIToolkitDemo
{
    public class AIEntity : MonoBehaviour,IMonoBehaviourTree
    {
        //-----------------------------------------------
        public const string BBKEY_NEXTMOVINGPOSITION = "NextMovingPosition";

        //-----------------------------------------------
        private AIBehaviorRequest _currentRequest;
        private AIBehaviorRequest _nextRequest;

        private GameObject _targetDummyObject;

        private float _nextTimeToGenMovingTarget;
        private string _lastTriggeredAnimation;
        public BehaviourTree Tree => _bt;
        [SerializeReference]
        private BehaviourTree _bt;
        public bool IsDead;

        public BTGraph BTConfig;

        private Animator _anim;
        public Blackboard Blackboard => _bt?.WorkingData?.Blackboard;

        public object EditorBlackboardProperty
        {
            get
            {
#if UNITY_EDITOR && !LOCKSTEP_PURE_MODE
                var obj = new UnityEditor.SerializedObject(this);
                return obj.FindProperty("_bt.WorkingData._blackboard.Keys");
#else
            return null;
#endif
            }
        }
        public BTNode TreeRoot => _bt?.Root;
        
        public AIEntity Init()
        {
            _nextTimeToGenMovingTarget = 0f;
            _lastTriggeredAnimation = string.Empty;
            _bt = new BehaviourTree();
            var data = _bt.DoAwake<AIEntityWorkingData>(BTConfig, transform);
            data.Entity = this;
            data.EntityTF = transform;
            _anim = data.EntityAnimator = GetComponent<Animator>();

            IsDead = false;

            _targetDummyObject = GameResourceManager.instance.LoadResource("AttackTarget");

            return this;
        }

        public void PlayAnimation(string name)
        {
            if (_lastTriggeredAnimation == name)
            {
                return;
            }
            _lastTriggeredAnimation = name;
            _anim.SetTrigger(name);
        }

        public int UpdateAI(float gameTime, float deltaTime)
        {
            if (gameTime > _nextTimeToGenMovingTarget)
            {
                _nextRequest = new AIBehaviorRequest(gameTime,
                    new Vector3(Random.Range(-10f, 10f), 0, Random.Range(-10f, 10f)));
                _nextTimeToGenMovingTarget = gameTime + 20f + Random.Range(-5f, 5f);
            }

            return 0;
        }

        public int UpdateReqeust(float gameTime, float deltaTime)
        {
            if (_nextRequest != _currentRequest)
            {
                //reset bev tree
                _bt.Reset();
                //assign to current
                _currentRequest = _nextRequest;

                //reposition and add a little offset
                Vector3 targetPos = _currentRequest.nextMovingTarget +
                                    TMathUtils.GetDirection2D(_currentRequest.nextMovingTarget, transform.position) *
                                    0.2f;
                Vector3 startPos = new Vector3(targetPos.x, -1.4f, targetPos.z);
                _targetDummyObject.transform.position = startPos;
                LeanTween.move(_targetDummyObject, targetPos, 1f);
            }

            return 0;
        }

        public int UpdateBehavior(float gameTime, float deltaTime)
        {
            if (_currentRequest == null)
            {
                return 0;
            }

            //update working data
            _anim.speed = GameTimer.instance.timeScale;
            _bt.WorkingData.SetValue(BBKEY_NEXTMOVINGPOSITION, _currentRequest.nextMovingTarget);
            _bt.DoUpdate(deltaTime);

            return 0;
        }
    }
}