using MyRPG.Enemy;
using System.Collections;
using Unity.Android.Types;
using UnityEngine;

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
        private bool isRun;

        //공격
        private float attackDelay = 1f;         //공격을 한번하고 다음 공격할 때까지의 지연시간
        private float attackTimer = 0f;         //공격을 하고 난 뒤에 경과되는 시간을 계산하기 위한 변수
        private float attackDistance = 1.5f;    //공격 거리(적과의 거리)
        private float chaseDistance = 2.5f;     //전투 중 적이 도망가면 다시 추적을 시작하기 위한 거리

        //애니메이션 정의
        private PlayerAni ani;

        private PlayerParams playerParams;

        private EnemyParams enemyParams;
        #endregion
        private void Start()
        {
            ani = GetComponent<PlayerAni>();
            ChangeState(State.Idle,PlayerAni.Ani_idle);

            playerParams = GetComponent<PlayerParams>();
            playerParams.InitParams();

            playerParams.dieEvent.AddListener(ChangeToPlayerDie);
        }
        public void ChangeToPlayerDie()
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
                enemy.hpBar.color = Color.red;
                int attackPower = playerParams.GetRandomAttack();
                enemyParams.SetEnemyAttack(attackPower);
                yield return new WaitForSeconds(0.7f);
                enemy.hpBar.color = Color.white;
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

        }
        void MoveState()
        {
            TurnToDesination();
            if (isRun)
            {
                ChangeState(State.Run, PlayerAni.Ani_Run);
            }
            else
            {
                MoveToDesination(false);
            }
        }
        void RunState()
        {
            TurnToDesination();
            if (isRun)
            {
                MoveToDesination(isRun);
            }
            else
            {
                ChangeState(State.Move, PlayerAni.Ani_move);
            }
        }
        void AttackState()
        {
            attackTimer = 0f;
            //tranform.LooAt(폭표지점 위치) 목표지점을 향해 오브젝트를 회전시키는 함수
            transform.LookAt(currentTargetPos);
            ChangeState(State.Attackidle,PlayerAni.Ani_attackidle);
        }
        void AttackIdle()
        {
            if(attackTimer > attackDelay)
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
            if(currentEnemy != null && currentEnemy == enemy)
            {
                return;
            }
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
            UpdateState();
        }
    }
}