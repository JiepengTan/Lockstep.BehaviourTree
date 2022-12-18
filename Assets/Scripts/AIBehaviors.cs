using System;
using System.Runtime.InteropServices;
using Lockstep.AI;
using UnityEngine;
using Random = UnityEngine.Random;

namespace AIToolkitDemo {
    class AIEntityWorkingData : BTWorkingData {
        public AIEntity entity { get; set; }
        public Transform entityTF { get; set; }
        public Animator entityAnimator { get; set; }
        public float gameTime { get; set; }
        public float deltaTime { get; set; }
    }

    [Serializable,GraphProcessor.NodeMenuItem("Condition/HasReachedTarget")]
    partial class CON_HasReachedTarget : BTConditionLeaf {
        protected override bool OnEvaluate (BTWorkingData wData){
            AIEntityWorkingData thisData = wData.As<AIEntityWorkingData>();
            Vector3 targetPos =
                TMathUtils.Vector3ZeroY(
                    thisData.entity.GetBBValue<Vector3>(AIEntity.BBKEY_NEXTMOVINGPOSITION, Vector3.zero));
            Vector3 currentPos = TMathUtils.Vector3ZeroY(thisData.entityTF.position);
            return TMathUtils.GetDistance2D(targetPos, currentPos) < 1f;
        }
    }

    [Serializable,GraphProcessor.NodeMenuItem("Action/Attack")]
    unsafe partial class NOD_Attack : BTActionLeaf
    {
        public UserContextData __content;
        private const float DEFAULT_WAITING_TIME = 5f;
        protected override int MemSize => sizeof(UserContextData) + base.MemSize;

        [StructLayout(LayoutKind.Sequential, Pack = NativeHelper.STRUCT_PACK)]
        public unsafe partial struct UserContextData {
            internal float attackingTime;
        }

        protected override void OnEnter(BTWorkingData wData){
            AIEntityWorkingData thisData = wData.As<AIEntityWorkingData>();
            var userData = (UserContextData*) GetUserContextData(wData);
            userData->attackingTime = DEFAULT_WAITING_TIME;
            thisData.entity.PlayAnimation("Attack");
        }

        protected override int OnExecute(BTWorkingData wData){
            AIEntityWorkingData thisData = wData.As<AIEntityWorkingData>();
            var userData = (UserContextData*) GetUserContextData(wData);
            if (userData->attackingTime > 0) {
                userData->attackingTime -= thisData.deltaTime;
                if (userData->attackingTime <= 0) {
                    thisData.entityAnimator.SetInteger("DeadRnd", Random.Range(0, 3));
                    thisData.entity.PlayAnimation("Dead");
                    thisData.entity.IsDead = true;
                }
            }

            return BTRunningStatus.EXECUTING;
        }
    }

    [Serializable,GraphProcessor.NodeMenuItem("Action/MoveTo")]
    class NOD_MoveTo : BTActionLeaf {
        protected override void OnEnter(BTWorkingData wData){
            AIEntityWorkingData thisData = wData.As<AIEntityWorkingData>();
            if (thisData.entity.IsDead) {
                thisData.entity.PlayAnimation("Reborn");
            }
            else {
                thisData.entity.PlayAnimation("Walk");
            }
        }

        protected override int OnExecute(BTWorkingData wData){
            AIEntityWorkingData thisData = wData.As<AIEntityWorkingData>();
            Vector3 targetPos =
                TMathUtils.Vector3ZeroY(
                    thisData.entity.GetBBValue<Vector3>(AIEntity.BBKEY_NEXTMOVINGPOSITION, Vector3.zero));
            Vector3 currentPos = TMathUtils.Vector3ZeroY(thisData.entityTF.position);
            float distToTarget = TMathUtils.GetDistance2D(targetPos, currentPos);
            if (distToTarget < 1f) {
                thisData.entityTF.position = targetPos;
                return BTRunningStatus.FINISHED;
            }
            else {
                int ret = BTRunningStatus.EXECUTING;
                Vector3 toTarget = TMathUtils.GetDirection2D(targetPos, currentPos);
                float movingStep = 1f * thisData.deltaTime;
                if (movingStep > distToTarget) {
                    movingStep = distToTarget;
                    ret = BTRunningStatus.FINISHED;
                }

                thisData.entityTF.position = thisData.entityTF.position + toTarget * movingStep;
                return ret;
            }
        }
    }

    [Serializable,GraphProcessor.NodeMenuItem("Action/TurnTo")]
    partial class NOD_TurnTo : BTActionLeaf {
        protected override void OnEnter(BTWorkingData wData){
            AIEntityWorkingData thisData = wData.As<AIEntityWorkingData>();
            if (thisData.entity.IsDead) {
                thisData.entity.PlayAnimation("Reborn");
            }
            else {
                thisData.entity.PlayAnimation("Walk");
            }
        }

        protected override int OnExecute(BTWorkingData wData){
            AIEntityWorkingData thisData = wData.As<AIEntityWorkingData>();
            Vector3 targetPos =
                TMathUtils.Vector3ZeroY(
                    thisData.entity.GetBBValue<Vector3>(AIEntity.BBKEY_NEXTMOVINGPOSITION, Vector3.zero));
            Vector3 currentPos = TMathUtils.Vector3ZeroY(thisData.entityTF.position);
            if (TMathUtils.IsZero((targetPos - currentPos).sqrMagnitude)) {
                return BTRunningStatus.FINISHED;
            }
            else {
                Vector3 toTarget = TMathUtils.GetDirection2D(targetPos, currentPos);
                Vector3 curFacing = thisData.entityTF.forward;
                float dotV = Vector3.Dot(toTarget, curFacing);
                float deltaAngle = Mathf.Acos(Mathf.Clamp(dotV, -1f, 1f));
                if (deltaAngle < 0.1f) {
                    thisData.entityTF.forward = toTarget;
                    return BTRunningStatus.FINISHED;
                }
                else {
                    Vector3 crossV = Vector3.Cross(curFacing, toTarget);
                    float angleToTurn = Mathf.Min(3f * thisData.deltaTime, deltaAngle);
                    if (crossV.y < 0) {
                        angleToTurn = -angleToTurn;
                    }

                    thisData.entityTF.Rotate(Vector3.up, angleToTurn * Mathf.Rad2Deg, Space.World);
                }
            }

            return BTRunningStatus.EXECUTING;
        }
    }
}