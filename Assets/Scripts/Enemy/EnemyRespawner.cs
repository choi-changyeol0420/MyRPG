using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

namespace MyRPG.Enemy
{
    public class EnemyRespawner : MonoBehaviour
    {
        public GameObject enemyPrefab; // 리스폰할 적 프리팹
        public Transform player; // 플레이어 위치
        public float respawnDelay = 3f; // 리스폰 대기 시간
        public int maxEnemies = 10; // 최대 적 개수
        public int enemiesPerSpawn = 3; // 한 번에 리스폰할 적 개수
        public float spawnRange = 10f; // 플레이어나 리스폰 지점 근처에서 생성할 범위
        public float despawnRange = 30f; // 플레이어와 일정 거리 이상이면 삭제

        [SerializeField]private List<GameObject> activeEnemies = new List<GameObject>(); // 현재 활성화된 적 리스트
        private GameObject newEnemys;
        void Start()
        {
            StartCoroutine(CheckAndRespawn()); // 자동 리스폰 시작
        }

        private IEnumerator CheckAndRespawn()
        {
            while (true)
            {
                yield return new WaitForSeconds(respawnDelay); // 일정 시간 대기

                if (activeEnemies.Count < maxEnemies)
                {
                    SpawnEnemiesNearPlayer();
                }

                RemoveFarEnemies();
            }
        }

        private void SpawnEnemiesNearPlayer()
        {
            int spawnCount = Mathf.Min(enemiesPerSpawn, maxEnemies - activeEnemies.Count);

            for (int i = 0; i < spawnCount; i++)
            {
                Vector3 spawnPosition = GetRandomSpawnPosition();
                newEnemys = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
                activeEnemies.Add(newEnemys);

                EnemyParams enemyParams = newEnemys.GetComponent<EnemyParams>();
                enemyParams.dieEvent.AddListener(RemoveEnemy);
            }
        }
        private void RemoveEnemy(GameObject enemy)
        {
            if (activeEnemies.Contains(enemy))
            {
                enemy.GetComponent<EnemyFSM>().ChangeState(State.Die,EnemyAni.DIE);
                activeEnemies.Remove(enemy);
            }
        }
        private Vector3 GetRandomSpawnPosition()
        {
            Vector3 randomOffset = new Vector3(Random.Range(-spawnRange, spawnRange), 0, Random.Range(-spawnRange, spawnRange));
            Vector3 spawnPosition = this.transform.position + randomOffset;

            // 바닥 높이 설정 (레이캐스트 사용)
            if (Physics.Raycast(spawnPosition + Vector3.up * 10f, Vector3.down, out RaycastHit hit, 20f))
            {
                spawnPosition.y = hit.point.y;
            }

            return spawnPosition;
        }

        private void RemoveFarEnemies()
        {
            for (int i = activeEnemies.Count - 1; i >= 0; i--)
            {
                if (Vector3.Distance(player.position, activeEnemies[i].transform.position) > despawnRange)
                {
                    Destroy(activeEnemies[i]);
                    activeEnemies.RemoveAt(i);
                }
            }
        }
    }
}