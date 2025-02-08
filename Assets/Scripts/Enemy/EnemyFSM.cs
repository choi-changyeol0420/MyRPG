using MyRPG.Player;
using UnityEngine;
using UnityEngine.UI;

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

        [HideInInspector]public EnemyParams enemyParams;
        private EnemyAni myAni;

        private Transform player;
        private PlayerParams playerParams;

        float chaseDistance = 5f;
        float attackDistance = 2.5f;
        float reChaseDistance = 3f;

        float rotAnglePerSecond = 360f;
        float movespeed = 1.3f;

        float attackDelay = 2f;
        float attackTimer = 0f;

        public GameObject effect;

        public GameObject projectilePrefab;
        public Transform firePoint;
        #endregion

        private void Awake()
        {
            myAni = GetComponent<EnemyAni>();
            enemyParams = GetComponent<EnemyParams>();
            enemyParams.InitParams();
            enemyParams.dieEvent += CallDieEvent;
            ChangeState(State.Idle, EnemyAni.IDLE);
            player = GameObject.FindGameObjectWithTag("Player").transform;
            playerParams = player.GetComponent<PlayerParams>();
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
        public void AttackCalculate()
        {
            if(player)
            {
                int attackPower = enemyParams.GetRandomAttack();
                GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
                projectile.GetComponent<EnemyProjectile>().damage = attackPower;
            } 
        }
        void CallDieEvent(GameObject Enemy)
        {
            ChangeState(State.Die, EnemyAni.DIE);
            player.gameObject.SendMessage("CurrentEnemyDie");
        }
        public void ChangeState(State newState, int EnemyAni)
        {
            if(currentState == newState) return;
            currentState = newState;
            myAni.ChangeState(EnemyAni);
        }
        public void ShowHitEffect()
        {
            PlayerFSM playerFSM = player.GetComponent<PlayerFSM>();
            GameObject hiteffect = Instantiate(effect, playerFSM.effectPos.position, Quaternion.identity);
            Destroy(hiteffect,2f);
        }
        void IdleState()
        {
            if(GetDistanceFromPlayer() < chaseDistance)
            {
                ChangeState(State.Chase, EnemyAni.WALK);
            }
        }
        void ChaseState()
        {
            if (GetDistanceFromPlayer() > chaseDistance)
            {
                ChangeState(State.Idle, EnemyAni.IDLE);
            }
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
                if (Time.time >= attackTimer)
                {
                    transform.LookAt(player.position);
                    myAni.ChangeState(EnemyAni.ATTACK);

                    attackTimer = Time.time + attackDelay;
                }
            }
        }
        void DieState()
        {
            GetComponent<BoxCollider>().enabled = false;
            Destroy(gameObject,5f);
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
            float fill = enemyParams.curHP / enemyParams.maxHP;
            enemyParams.healthBar.fillAmount = Mathf.Lerp(enemyParams.healthBar.fillAmount, fill, Time.deltaTime * 5f);
        }
    }
}