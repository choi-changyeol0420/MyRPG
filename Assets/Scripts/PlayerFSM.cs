using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyRPG
{
    public class PlayerFSM : MonoBehaviour
    {
        public enum State
        {
            Idle,
            Move,
            Attack,
            Attackidle,
            Dead
        }
        #region Vaiables
        //idle 상태를 기본 상태로 지정
        public State currentState = State.Idle;

        //마우스 클릭 지점, 플레이어가 이동할 목적지의 좌표를 저장할 예정
        private Vector3 currentTargetPos;

        public float rotAnglePerSecoud = 360f;  //1초에 플레이어의 방향을 360도 회전한다
        public float moveSpeed = 2f;            //초당 이동 속도

        private PlayerAni ani;
        #endregion
        private void Start()
        {
            ani = GetComponent<PlayerAni>();
            ChangeState(State.Idle,PlayerAni.Ani_idle);
        }
        private void Update()
        {
            UpdateState();
            TurnToDesination();
            MoveToDesination();
        }
        void UpdateState()
        {
            switch(currentState)
            {
                case State.Idle:
                    break;
                case State.Move:
                    break;
                case State.Attack:
                    break;
                case State.Attackidle:
                    break;
                case State.Dead:
                    break;
                default:
                    break;
            }
        }
        void ChangeState(State newState ,int Number)
        {
            if(currentState == newState) return;
            ani.ChangeAni(Number);
            currentState = newState;
        }
        public void MoveTo(Vector3 point)
        {
            currentTargetPos = point;
            ChangeState(State.Move,PlayerAni.Ani_move);
        }
        void TurnToDesination()
        {
            Quaternion lookRotation = Quaternion.LookRotation(currentTargetPos - transform.position);

            transform.rotation = Quaternion.RotateTowards(transform.rotation, lookRotation, Time.deltaTime * rotAnglePerSecoud);
        }
        void MoveToDesination()
        {
            transform.position = Vector3.MoveTowards(transform.position,currentTargetPos,moveSpeed * Time.deltaTime);

            if(transform.position == currentTargetPos )
            {
                ChangeState(State.Idle, PlayerAni.Ani_idle);
            }
        }
    }
}