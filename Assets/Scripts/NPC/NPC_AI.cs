using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Playables;

namespace MyRPG.NPC
{
    /// <summary>
    /// NPC 상태
    /// </summary>
    public enum NPCState
    {
        Idle,     // 가만히 있음
        Patrol,   // 순찰
        Talk,     // 대화
        Flee,     // 도망
        Combat    // 전투
    }
    /// <summary>
    ///  NPC 상태에 따른 행동
    /// </summary>
    public class NPC_AI : MonoBehaviour
   {
        #region Variables
        public NPCState currentNPC = NPCState.Patrol;
        public Transform[] patrolPosition;
        private int currentPatrolIndex;
        private NavMeshAgent agent;
        private Transform Player;
        public float detectionRange = 10;
        public float fleeDistance = 5;
        public float talkDistance = 3;
        #endregion
        private void Start()
        {
            agent = GetComponent<NavMeshAgent>();
            Player = GameObject.FindWithTag("Player").transform;
            GoTONextPatrolPoints();
        }
        private void Update()
        {
            CurrentState();
        }
        private void CurrentState()
        {
            switch(currentNPC)
            {
                case NPCState.Idle:
                    break;
                case NPCState.Patrol:
                    PatrolState();
                    break;
                case NPCState.Talk:
                    TalkState();
                    break;
                case NPCState.Flee:
                    FleeState();
                    break;
                case NPCState.Combat:
                    CombatState();
                    break;
            }
            CheckPlayerDistance();
        }
        void PatrolState()
        {
            if(agent.remainingDistance< 0.5f && !agent.pathPending)
            { 
                GoTONextPatrolPoints();
            }
        }
        void GoTONextPatrolPoints()
        {
            if (patrolPosition.Length == 0) return;
            agent.destination = patrolPosition[currentPatrolIndex].position;
            currentPatrolIndex = (currentPatrolIndex + 1) % patrolPosition.Length;
        }
        void CheckPlayerDistance()
        {
            float distance = Vector3.Distance(Player.position, transform.position);
            if(distance < talkDistance)
            {
                currentNPC = NPCState.Talk;
            }
            else if(distance < detectionRange && Random.value > 0.5f)
            {
                currentNPC= NPCState.Flee;
            }
            else if(distance < detectionRange)
            {
                currentNPC= NPCState.Combat;
            }
            else
            {
                currentNPC= NPCState.Combat;
            }
        }
        void TalkState()
        {
            Debug.Log("플레이어와 대화 중..");
        }
        void FleeState()
        {
            Vector3 fleeDirection = (transform.position - Player.position).normalized;
            Vector3 fleeTarget = transform.position + fleeDirection * fleeDistance;
            agent.SetDestination(fleeTarget);
            Debug.Log("NPC 도망 중..");
        }
        void CombatState()
        {
            Debug.Log("전투모드");
        }
    }
}
