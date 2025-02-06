using MyRPG.Enemy;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace MyRPG.Player
{
    public class PlayerFSM : MonoBehaviour
    {
        public enum State
        {
            Idle,
            Move,
            Run,
            Attack,
            Attackidle,
            Dead
        }
        #region Vaiables
        //idle 상태를 기본 상태로 지정
        public State currentState = State.Idle;

        //마우스 클릭 지점, 플레이어가 이동할 목적지의 좌표를 저장할 예정
        private Vector3 currentTargetPos;

        private GameObject currentEnemy;

        //이동
        public float rotAnglePerSecoud = 360f;  //1초에 플레이어의 방향을 360도 회전한다
        public float moveSpeed;            //초당 이동 속도
        private float walkSpeed = 3f;            //초당 이동 속도
        private float elapsedRunTime = 0f; // 현재까지 뛴 시간
        private float maxRunTime = 5f; // 최대 달릴 수 있는 시간 (예: 5초)
        private bool isRun;
        public float stamina = 5f;     // 최대 스태미나 (초 단위)
        public float staminaRecoveryRate = 1f; // 초당 스태미나 회복량
        private float currentStamina;
        public Image staminaImage;

        //공격
        private float attackDelay = 1f;         //공격을 한번하고 다음 공격할 때까지의 지연시간
        private float attackTimer = 0f;         //공격을 하고 난 뒤에 경과되는 시간을 계산하기 위한 변수
        private float attackDistance = 1.5f;    //공격 거리(적과의 거리)

        //애니메이션 정의
        private PlayerAni ani;

        private PlayerParams playerParams;
        public Transform effectPos;

        private EnemyParams enemyParams;
        #endregion
        private void Start()
        {
            ani = GetComponent<PlayerAni>();
            ChangeState(State.Idle,PlayerAni.Ani_idle);
            
            playerParams = GetComponent<PlayerParams>();
            playerParams.InitParams();
            currentStamina = stamina; // 스태미나 초기화
            playerParams.dieEvent.AddListener(ChangeToPlayerDie);
        }
        public void ChangeToPlayerDie(GameObject player)
        {
            ChangeState(State.Dead, PlayerAni.Ani_die);
        }
        public void CurrentEnemyDie()
        {
            ChangeState(State.Idle, PlayerAni.Ani_idle);
            print("enemy was killed");

            currentEnemy = null;
        }
        public void AttackCalculate()
        {
            if (currentEnemy == null) return;
            EnemyFSM enemy = currentEnemy.GetComponent<EnemyFSM>();
            enemy.ShowHitEffect();
            StartCoroutine(HpColorChange(enemy));
        }
        IEnumerator HpColorChange(EnemyFSM enemy)
        {
            if(enemy)
            {
                enemy.enemyParams.healthBar.color = Color.red;
                int attackPower = playerParams.GetRandomAttack();
                enemyParams.TakeDamage(attackPower);
                yield return new WaitForSeconds(0.7f);
                enemy.enemyParams.healthBar.color = Color.white;
            }
        }
        //캐릭터의 상태가 바뀌면 어떤 일이 일어난지를 미리 정의
        void UpdateState()
        {
            switch(currentState)
            {
                case State.Idle:
                    IdleState();
                    break;
                case State.Move:
                    MoveState();
                    break;
                case State.Run:
                    RunState();
                    break;
                case State.Attack:
                    AttackState();
                    break;
                case State.Attackidle:
                    AttackIdle();
                    break;
                case State.Dead:
                    DeadState();
                    break;
                default:
                    break;
            }
        }
        void IdleState()
        {
            if (playerParams.curHP < playerParams.maxHP)
            {
                playerParams.StartHealing(5 * 1.5f); // 체력이 부족하면 회복 시작
            }
            currentStamina += 1.5f * Time.deltaTime; // 초당 스태미나 회복
            if (currentStamina > stamina)
            {
                currentStamina = stamina;
            }
        }
        void MoveState()
        {
            TurnToDesination();
            if (playerParams.curHP < playerParams.maxHP)
            {
                playerParams.StartHealing(); // 체력이 부족하면 회복 시작
            }
            // 뛰는 시간이 초과되었거나 스태미나가 1 이하이면 걷기로 변경
            if ((isRun && currentStamina > 0) && elapsedRunTime < maxRunTime)
            {
                ChangeState(State.Run, PlayerAni.Ani_Run);
            }
            else
            {
                // 애니메이션을 걷기로 변경
                ani.ChangeAni(PlayerAni.Ani_move);
                MoveToDesination(false);

                // 스태미나 회복
                currentStamina += staminaRecoveryRate * Time.deltaTime;
                if (currentStamina > stamina)
                {
                    currentStamina = stamina;
                }

                // 뛰는 시간 초기화
                elapsedRunTime = 0f;
            }
        }
        void RunState()
        {
            TurnToDesination();
            if (playerParams.curHP < playerParams.maxHP)
            {
                playerParams.StartHealing(); // 체력이 부족하면 회복 시작
            }
            if (isRun)
            {
                // 뛰는 시간이 maxRunTime을 초과하면 걷기로 변경
                if (elapsedRunTime >= maxRunTime)
                {
                    ChangeState(State.Move, PlayerAni.Ani_move);
                    elapsedRunTime = 0f; // 뛰는 시간 초기화
                    return;
                }

                if (currentStamina > 0)
                {
                    currentStamina -= Time.deltaTime; // 초당 스태미나 감소
                    elapsedRunTime += Time.deltaTime; // 뛰는 시간 증가
                    MoveToDesination(isRun);
                }
                else
                {
                    currentStamina = 0;
                    ChangeState(State.Move, PlayerAni.Ani_move);
                    elapsedRunTime = 0f; // 뛰는 시간 초기화
                }
            }
            else
            {
                ChangeState(State.Move, PlayerAni.Ani_move);
                elapsedRunTime = 0f; // 뛰는 시간 초기화
            }
        }
        void AttackState()
        {
            if (playerParams.curHP < playerParams.maxHP)
            {
                playerParams.StartHealing(1.5f); // 체력이 부족하면 회복 시작
            }
            attackTimer = 0f;
            //tranform.LooAt(폭표지점 위치) 목표지점을 향해 오브젝트를 회전시키는 함수
            transform.LookAt(currentTargetPos);
            ChangeState(State.Attackidle,PlayerAni.Ani_attackidle);
        }
        void AttackIdle()
        {
            if (playerParams.curHP < playerParams.maxHP)
            {
                playerParams.StartHealing(1.5f); // 체력이 부족하면 회복 시작
            }
            if (attackTimer > attackDelay)
            {
                if(Input.GetKeyDown(KeyCode.Q))
                {
                    ChangeState(State.Attack, PlayerAni.Ani_attack);
                }
            }
            attackTimer += Time.deltaTime;
        }
        void DeadState()
        {

        }   
        public void AttackEnemy(GameObject enemy)
        {
            if(currentEnemy != null && currentEnemy == enemy) return;
            //적 (몬스터)의 파라미터를 변수에 저장
            enemyParams =enemy.GetComponent<EnemyParams>();
            if(!enemyParams.isDie)
            {
                currentEnemy = enemy;
                currentTargetPos = currentEnemy.transform.position;
                ChangeState(State.Move, PlayerAni.Ani_move);
            }
            else
            {
                enemyParams = null;
            }
        }
        void ChangeState(State newState ,int Number)
        {
            if(currentState == newState) return;
            currentState = newState;
            ani.ChangeAni(Number);
        }
        public void MoveTo(Vector3 point)
        {
            if (currentState == State.Dead) return;
            currentEnemy = null;
            currentTargetPos = point;
            if (isRun)
            {
                ChangeState(State.Move, PlayerAni.Ani_Run);
            }
            else
            {
                ChangeState(State.Move, PlayerAni.Ani_move);
            }
        }
        void TurnToDesination()
        {
            Quaternion lookRotation = Quaternion.LookRotation(currentTargetPos - transform.position);

            transform.rotation = Quaternion.RotateTowards(transform.rotation, lookRotation, Time.deltaTime * rotAnglePerSecoud);
        }
        void MoveToDesination(bool isRunning)
        {
            //Vector3.MoveTowards(시작지점,목표지점,최대이동거리)
            moveSpeed = isRunning ? walkSpeed * 2f : walkSpeed;

            transform.position = Vector3.MoveTowards(transform.position,currentTargetPos,moveSpeed * Time.deltaTime);
            if(currentEnemy == null)
            {
                //플레이어의 위치와 목표지점의 위치가 같으면, 상태를 idle상태로 바꾸라는 명령
                if (transform.position == currentTargetPos)
                {
                    ChangeState(State.Idle, PlayerAni.Ani_idle);
                }
            }
            else if(Vector3.Distance(transform.position,currentTargetPos) < attackDistance)
            {
                ChangeState(State.Attack, PlayerAni.Ani_attack);
            }
        }
        private void Update()
        {
            isRun = Input.GetKey(KeyCode.LeftShift);
            UIManager.instance.UpdatePlayerUI(playerParams);
            staminaImage.fillAmount = Mathf.Lerp(staminaImage.fillAmount,currentStamina/stamina,Time.deltaTime*5f);
            UpdateState();
        }
    }
}