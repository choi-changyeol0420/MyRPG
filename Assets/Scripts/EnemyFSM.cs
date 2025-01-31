using UnityEngine;

namespace MyRPG.Enemy
{
    public enum State
    {
        Idle,
        Chase,
        Attack,
        Die,
        NoState
    }
    public class EnemyFSM : MonoBehaviour
    {
        #region Variables
        public State currentState = State.Idle;

        private EnemyAni myAni;

        private Transform player;
        float chaseDistance = 5f;
        float attackDistance = 2.5f;
        float reChaseDistance = 3f;

        float rotAnglePerSecond = 360f;
        float movespeed = 1.3f;

        float attackDelay = 2f;
        float attackTimer = 0f;
        #endregion

        private void Start()
        {
            myAni = GetComponent<EnemyAni>();
            ChangeState(State.Idle, EnemyAni.IDLE);
            player = GameObject.FindWithTag("Player").transform;
        }
        void UpdateState()
        {
            switch(currentState)
            {
                case State.Idle:
                    IdleState();
                    break;
                case State.Chase:
                    ChaseState();
                    break;
                case State.Attack:
                    AttackState();
                    break;
                case State.Die:
                    DieState();
                    break;
                case State.NoState:
                    NoState();
                    break;
            }
        }
        public void ChangeState(State newState, int EnemyAni)
        {
            if(currentState == newState) return;
            currentState = newState;
            myAni.ChangeState(EnemyAni);
        }
        void IdleState()
        {
            if(GetDistanceFromPlayer()<chaseDistance)
            {
                ChangeState(State.Chase, EnemyAni.WALK);
            }
        }
        void ChaseState()
        {
            if (GetDistanceFromPlayer() < attackDistance)
            {
                ChangeState(State.Attack, EnemyAni.ATTACK);
            }
            else
            {
                TurnToDestination();
                MoveToDestination();
            }
        }
        void AttackState()
        {
            if(GetDistanceFromPlayer() > reChaseDistance)
            {
                attackTimer = 0;
                ChangeState(State.Chase,EnemyAni.WALK);
            }
            else
            {
                if (attackTimer > attackDelay)
                {
                    transform.LookAt(player.position);
                    myAni.ChangeState(EnemyAni.ATTACK);

                    attackTimer = 0;
                }
                attackTimer = Time.deltaTime;
            }
        }
        void DieState()
        {

        }
        void NoState()
        {

        }

        void TurnToDestination()
        {
            Quaternion lookRotation = Quaternion.LookRotation(player.position - transform.position);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, lookRotation, Time.deltaTime * rotAnglePerSecond);
        }
        void MoveToDestination()
        {
            transform.position = Vector3.MoveTowards(transform.position, player.position,movespeed * Time.deltaTime);
        }

        float GetDistanceFromPlayer()
        {
            float distance = Vector3.Distance(transform.position, player.position);
            return distance;
        }

        private void Update()
        {
            UpdateState();
        }
    }
}