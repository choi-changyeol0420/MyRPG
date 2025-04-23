using MyRPG.Enemy;
using MyRPG.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace MyRPG.Drone
{
    [System.Serializable]
    public class DroneStats
    {
        #region Variables
        public GameObject projectilePrefab;
        public string projectileTag;
        public float FireRate;
        public Transform[] firePoint;
        #endregion
    }
    [RequireComponent(typeof(NavMeshAgent))]
    public abstract class DroneFSM : MonoBehaviour
    {
        #region Variables
        public GameObject upgradeDrone;
        public float followDistance = 3f;
        public float attackRange;
        public List<DroneStats> droneStat;

        private Transform player;
        private Transform targetEnemy;
        private NavMeshAgent agent;
        private static int upgradeDroneindex = 1;
        protected bool isAttacking = false;
        #endregion
        private void Start()
        {
            agent = GetComponent<NavMeshAgent>();
        }
        private void Update()
        {
            if(player == null) return;
            
            if(player.gameObject.GetComponent<PlayerParams>().stat.isDie)
            {
                Destroy(gameObject);
                return;
            }
            FindTarget();
            MoveToTarget();
        }
        public void SetPlayer(Transform playerpos)
        {
            player = playerpos;
        }
        void FindTarget()
        {
            if(targetEnemy != null)
            {
                float distance = Vector3.Distance(transform.position, targetEnemy.position);
                if(distance > attackRange)
                {
                    targetEnemy = null;
                }
            }

            if(targetEnemy == null)
            {
                GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
                float closeDistance = attackRange;
                foreach (GameObject enemy in enemies)
                {
                    float distance = Vector3.Distance(transform.position, enemy.transform.position);
                    if (distance < closeDistance)
                    {
                        closeDistance = distance;
                        targetEnemy = enemy.transform;
                    }
                }
            }
            
            if (targetEnemy && !isAttacking)
            {
                RotateToMoveDirection();
                DroneAttack();
            }
        }
        void DroneAttack()
        {
            if(isAttacking) return;
            isAttacking = true;
            StartCoroutine(FireProjectile());
        }
        void MoveToTarget()
        {
            if(targetEnemy)
            {
                HandleTargetMovement();
            }
            else if (player)
            {
                HandlePlayerFollowing();
            }   
        }
        private void HandleTargetMovement()
        {
            float distance = Vector3.Distance(transform.position, targetEnemy.position);
            if(player)
            {
                float playerDis = Vector3.Distance(transform.position, player.position);
                if (distance < playerDis)
                {
                        agent.SetDestination(player.position);
                    }
                    else
                    {
                        agent.ResetPath();
                    }
                }
                if(distance > attackRange * 0.8f)
                {
                    agent.SetDestination(targetEnemy.position);
                }
                else
                {
                    agent.ResetPath();
                }
        }
        private void HandlePlayerFollowing()
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
        protected virtual IEnumerator FireProjectile()
        {
            while(targetEnemy != null)
            {
                foreach (DroneStats drone in droneStat)
                {
                    if(drone.projectilePrefab && targetEnemy)
                    {
                        foreach (Transform firePoint in drone.firePoint)
                        {
                            if (ObjectPool.Instance != null)
                            {
                                GameObject bullet = ObjectPool.Instance.SpawnFromPool(
                                    drone.projectileTag,
                                    firePoint.position,
                                    firePoint.rotation
                                );

                                if (bullet == null)
                                {
                                    Debug.LogWarning($"Failed to spawn projectile with tag {drone.projectileTag}");
                                }
                            }
                            yield return new WaitForSeconds(drone.FireRate);
                        }
                    }
                }
                yield return null;
            }
            isAttacking = false;
        }
        void RotateToMoveDirection()
        {
            if (targetEnemy != null)
            {
                // 적을 바라보도록 회전
                Quaternion targetRotation = Quaternion.LookRotation(targetEnemy.position - transform.position);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
            }
        }
        public void UpgradeDrone()
        {
            if(upgradeDroneindex < 3 && upgradeDrone != this.gameObject)
            {
                upgradeDroneindex++;
            }
        }
    }
}
