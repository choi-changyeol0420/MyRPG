using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace MyRPG.Player
{
    public class DroneFSM : MonoBehaviour
    {
        #region Variables
        public float followDistance = 3f;
        public float attackRange = 10f;
        public float fireRate = 1f;
        public GameObject projectile;
        [HideInInspector] public GameObject bullet;
        public Transform firePoint;

        private Transform player;
        private Transform targetEnemy;
        private NavMeshAgent agent;
        private bool isAttacking = false;
        #endregion
        private void Start()
        {
            agent = GetComponent<NavMeshAgent>();
        }
        private void Update()
        {
            FindTarget();
            MoveToTarget();
        }
        public void SetPlayer(Transform playerpos)
        {
            player = playerpos;
        }
        void FindTarget()
        {
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            float closeDistance = attackRange;
            targetEnemy = null;
            foreach(GameObject enemy in enemies)
            {
                float distance = Vector3.Distance(transform.position, enemy.transform.position);
                if(distance < closeDistance)
                {
                    closeDistance = distance;
                    targetEnemy = enemy.transform;
                }
            }
            if(targetEnemy && !isAttacking)
            {
                StartCoroutine(DroneAttack());
            }
        }
        IEnumerator DroneAttack()
        {
            isAttacking = true;
            while(targetEnemy)
            {
                FireProjectile();
                yield return new WaitForSeconds(fireRate);
            }
            isAttacking = false;
        }
        void MoveToTarget()
        {
            if(targetEnemy)
            {
                float distance = Vector3.Distance(transform.position, targetEnemy.position);
                if(distance > attackRange * 0.8f)
                {
                    agent.SetDestination(targetEnemy.position);
                }
                else
                {
                    agent.ResetPath();
                }
            }
            else if (player)
            {
                float playerdistance = Vector3.Distance(transform.position, player.position);
                if(playerdistance > followDistance)
                {
                    agent.SetDestination(player.position);
                }
                else
                {
                    agent.ResetPath();
                }
            }   
        }
        void FireProjectile()
        {
            if(projectile && targetEnemy)
            {
                bullet = Instantiate(projectile, firePoint.position, firePoint.rotation);
            }
        }
    }
}