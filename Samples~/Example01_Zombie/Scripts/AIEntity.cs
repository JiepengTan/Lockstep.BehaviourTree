using UnityEngine;
using Lockstep.AI;
using UnityEngine.Serialization;

namespace AIToolkitDemo
{
    public class AIEntity : MonoBehaviour, IMonoBehaviourTree
    {
        //-----------------------------------------------
        public const string BBKEY_NEXTMOVINGPOSITION = "TargetPos";

        //-----------------------------------------------
        private AIBehaviorRequest _currentRequest;
        private AIBehaviorRequest _nextRequest;

        private GameObject _targetDummyObject;

        private float _nextTimeToGenMovingTarget;
        private string _lastTriggeredAnimation;
        [SerializeReference] public BehaviourTree Tree;
        public bool IsDead;

#if !LOCKSTEP_PURE_MODE
        public BTGraph BTConfig;
#endif
        public string ConfigPath;

        private Animator _anim;

        public object EditorBlackboardProperty
        {
            get
            {
#if UNITY_EDITOR
                var obj = new UnityEditor.SerializedObject(this);
                return obj.FindProperty("Tree.WorkingData.Blackboard.Keys");
#else
            return null;
#endif
            }
        }

        public AIEntity Init()
        {
            _nextTimeToGenMovingTarget = 0f;
            _lastTriggeredAnimation = string.Empty;
            Tree = new BehaviourTree();
            AIEntityWorkingData data = null;
#if LOCKSTEP_PURE_MODE
            {
                var bytesConfig = Resources.Load<TextAsset>(ConfigPath);
                var bytes = bytesConfig.bytes;
                var id = int.Parse(bytesConfig.name.Split("_")[0]);
                data = Tree.DoAwake<AIEntityWorkingData>(transform, id, bytes);
            }
#else
            {
                data = Tree.DoAwake<AIEntityWorkingData>(transform,BTConfig );
            }
#endif


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
                Tree.Reset();
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
            Tree.WorkingData.SetValue(BBKEY_NEXTMOVINGPOSITION, _currentRequest.nextMovingTarget);
            Tree.DoUpdate(deltaTime);

            return 0;
        }
    }
}