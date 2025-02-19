using MyRPG.Player;
using UnityEngine;
using UnityEngine.AI;

namespace MyRPG.Enemy
{
    public enum State
    {
        Idle,
        Patrol,
        Chase,
        Attack,
        Die,
        NoState
    }
    public class EnemyFSM : MonoBehaviour
    {
        #region Variables
        public State currentState = State.Patrol;

        [HideInInspector]public EnemyParams enemyParams;
        private EnemyAni myAni;

        private Transform player;
        private PlayerParams playerParams;
        private NavMeshAgent agent;

        [SerializeField] private float chaseDistance = 15;
        [SerializeField] private float attackDistance = 5;
        [SerializeField] private float reChaseDistance = 10;

        private float attackDelay = 2f;
        private float attackTimer = 0f;

        private Vector3 originalPosition;
        private Vector3 randowPosition;
        private bool moveToRandom = true;

        public GameObject effect;

        public GameObject projectilePrefab;
        public Transform firePoint;

        private float timer = 0;
        [SerializeField]private float nameHealthBartime = 5;
        #endregion

        private void Awake()
        {
            myAni = GetComponent<EnemyAni>();
            enemyParams = GetComponent<EnemyParams>();
            agent = GetComponent<NavMeshAgent>();
            originalPosition = transform.position;
            enemyParams.InitParams();
            enemyParams.dieEvent += CallDieEvent;
            player = GameObject.FindGameObjectWithTag("Player").transform;
            playerParams = player.GetComponent<PlayerParams>();
            SetRandomPosition();
        }
        void UpdateState()
        {
            switch(currentState)
            {
                case State.Idle:
                    IdleState();
                    break;
                case State.Patrol:
                    PatrolState();
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
        private void SetRandomPosition()
        {
            Vector3 Randompos = new Vector3(Random.Range(-3f, 3f), 0, Random.Range(-3f, 3f));
            randowPosition = originalPosition + Randompos;
        }
        public void AttackCalculate()
        {
            if (playerParams.isDie)
            {
                ChangeState(State.Idle,EnemyAni.IDLE);
                return;
            }
            IDamageable damageable = player.GetComponent<IDamageable>();
            if(damageable != null)
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
            playerParams.AddExperience(enemyParams.enemies.exp);
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
            agent.destination = player.position;
        }
        void PatrolState()
        {
            if(agent.remainingDistance < 0.5f && ! agent.pathPending)
            {
                if(moveToRandom)
                {
                    agent.destination = originalPosition;
                }
                else
                {
                    SetRandomPosition();
                    agent.destination = randowPosition;
                }
                moveToRandom = !moveToRandom;
            }
        }
        void AttackState()
        {
            if (Time.time >= attackTimer)
            {
                transform.LookAt(player.position);
                myAni.ChangeState(EnemyAni.ATTACK);

                attackTimer = Time.time + attackDelay;
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
        void CheckStateChange()
        {
            if (GetDistanceFromPlayer() < attackDistance) ChangeState(State.Attack, EnemyAni.ATTACK);
            else if (GetDistanceFromPlayer() < reChaseDistance)
            {
                attackTimer = 0;
                ChangeState(State.Chase, EnemyAni.WALK); 
            }
            else if (GetDistanceFromPlayer() > chaseDistance) ChangeState(State.Patrol, EnemyAni.WALK);
        }
        float GetDistanceFromPlayer()
        {
            float distance = Vector3.Distance(transform.position, player.position);
            return distance;
        }

        private void Update()
        {
            UpdateState();
            CheckStateChange();
            SetNameHealthBar();
            float fill = enemyParams.curHP / enemyParams.maxHP;
            enemyParams.healthBar.fillAmount = Mathf.Lerp(enemyParams.healthBar.fillAmount, fill, Time.deltaTime * 5f);
        }
        void SetNameHealthBar()
        {
            if(enemyParams.enemyName.gameObject.activeSelf)
            {
                timer += Time.deltaTime;
                if(timer >= nameHealthBartime)
                {
                    enemyParams.enemyName.gameObject.SetActive(false);
                    timer = 0;
                }
            }
        }
    }
}