using System;
using System.Runtime.InteropServices;
using Lockstep.AI;
using UnityEngine;
using Random = UnityEngine.Random;

namespace AIToolkitDemo {
   public class AIEntityWorkingData : BTWorkingData {
        public AIEntity Entity { get; set; }
        public Transform EntityTF { get; set; }
        public Animator EntityAnimator { get; set; }
    }

    [Serializable,GraphProcessor.NodeMenuItem("Condition/HasReachedTarget")]
    partial class CON_HasReachedTarget : BTConditionLeaf {
        public float Distance = 1.5f;
        protected override bool OnEvaluate (BTWorkingData wData){
            AIEntityWorkingData thisData = wData.As<AIEntityWorkingData>();
            Vector3 targetPos = TMathUtils.Vector3ZeroY(thisData.GetValue(AIEntity.BBKEY_NEXTMOVINGPOSITION, Vector3.zero));
            Vector3 currentPos = TMathUtils.Vector3ZeroY(thisData.EntityTF.position);
            return TMathUtils.GetDistance2D(targetPos, currentPos) < Distance;
        }
    }

    [Serializable,GraphProcessor.NodeMenuItem("Action/Attack")]
    unsafe partial class NOD_Attack : BTActionLeaf
    {
        private const float DEFAULT_WAITING_TIME = 5f;
        protected override int RunTimeSize => sizeof(UserContextData);

        [StructLayout(LayoutKind.Sequential, Pack = NativeHelper.STRUCT_PACK)]
        public unsafe partial struct UserContextData {
            internal float AttackingTime;
        }

        protected override void OnEnter(BTWorkingData wData){
            AIEntityWorkingData thisData = wData.As<AIEntityWorkingData>();
            var userData = (UserContextData*) GetUserContextData(wData);
            userData->AttackingTime = DEFAULT_WAITING_TIME;
            thisData.Entity.PlayAnimation("Attack");
        }

        protected override int OnExecute(BTWorkingData wData){
            AIEntityWorkingData thisData = wData.As<AIEntityWorkingData>();
            var userData = (UserContextData*) GetUserContextData(wData);
            if (userData->AttackingTime > 0) {
                userData->AttackingTime -= thisData.DeltaTime;
                if (userData->AttackingTime <= 0) {
                    thisData.EntityAnimator.SetInteger("DeadRnd", Random.Range(0, 3));
                    thisData.Entity.PlayAnimation("Dead");
                    thisData.Entity.IsDead = true;
                }
            }

            return BTRunningStatus.EXECUTING;
        }
    }

    [Serializable,GraphProcessor.NodeMenuItem("Action/MoveTo")]
    partial class NOD_MoveTo : BTActionLeaf
    {
        public float MoveSpeed = 1f;
        public float StopDistance = 1.5f;
        protected override void OnEnter(BTWorkingData wData){
            AIEntityWorkingData thisData = wData.As<AIEntityWorkingData>();
            if (thisData.Entity.IsDead) {
                thisData.Entity.PlayAnimation("Reborn");
            }
            else {
                thisData.Entity.PlayAnimation("Walk");
            }
        }

        protected override int OnExecute(BTWorkingData wData){
            AIEntityWorkingData thisData = wData.As<AIEntityWorkingData>();
            Vector3 targetPos = TMathUtils.Vector3ZeroY(thisData.GetValue(AIEntity.BBKEY_NEXTMOVINGPOSITION, Vector3.zero));
            Vector3 currentPos = TMathUtils.Vector3ZeroY(thisData.EntityTF.position);
            float distToTarget = TMathUtils.GetDistance2D(targetPos, currentPos);
            if (distToTarget < StopDistance) {
                //thisData.EntityTF.position = targetPos;
                return BTRunningStatus.FINISHED;
            }
            else {
                int ret = BTRunningStatus.EXECUTING;
                Vector3 toTarget = TMathUtils.GetDirection2D(targetPos, currentPos);
                float movingStep = MoveSpeed * thisData.DeltaTime;
                if (movingStep > distToTarget) {
                    movingStep = distToTarget;
                    ret = BTRunningStatus.FINISHED;
                }

                thisData.EntityTF.position = thisData.EntityTF.position + toTarget * movingStep;
                return ret;
            }
        }
    }

    [Serializable,GraphProcessor.NodeMenuItem("Action/TurnTo")]
    partial class NOD_TurnTo : BTActionLeaf
    {
        public float TurnSpeed = 3; 
        protected override void OnEnter(BTWorkingData wData){
            AIEntityWorkingData thisData = wData.As<AIEntityWorkingData>();
            if (thisData.Entity.IsDead) {
                thisData.Entity.PlayAnimation("Reborn");
            }
            else {
                thisData.Entity.PlayAnimation("Walk");
            }
        }

        protected override int OnExecute(BTWorkingData wData){
            AIEntityWorkingData thisData = wData.As<AIEntityWorkingData>();
            Vector3 targetPos =
                TMathUtils.Vector3ZeroY(
                    thisData.GetValue(AIEntity.BBKEY_NEXTMOVINGPOSITION, Vector3.zero));
            Vector3 currentPos = TMathUtils.Vector3ZeroY(thisData.EntityTF.position);
            if (TMathUtils.IsZero((targetPos - currentPos).sqrMagnitude)) {
                return BTRunningStatus.FINISHED;
            }
            else {
                Vector3 toTarget = TMathUtils.GetDirection2D(targetPos, currentPos);
                Vector3 curFacing = thisData.EntityTF.forward;
                float dotV = Vector3.Dot(toTarget, curFacing);
                float deltaAngle = Mathf.Acos(Mathf.Clamp(dotV, -1f, 1f));
                if (deltaAngle < 0.1f) {
                    thisData.EntityTF.forward = toTarget;
                    return BTRunningStatus.FINISHED;
                }
                else {
                    Vector3 crossV = Vector3.Cross(curFacing, toTarget);
                    float angleToTurn = Mathf.Min(TurnSpeed * thisData.DeltaTime, deltaAngle);
                    if (crossV.y < 0) {
                        angleToTurn = -angleToTurn;
                    }

                    thisData.EntityTF.Rotate(Vector3.up, angleToTurn * Mathf.Rad2Deg, Space.World);
                }
            }

            return BTRunningStatus.EXECUTING;
        }
    }
}