using MyRPG.Player;
using UnityEngine;

namespace MyRPG.Enemy
{
    public class EnemyProjectile : MonoBehaviour
    {
        #region Variables
        public float speed = 10f; // 발사체 속도
        public float lifetime = 5f; // 발사체 지속 시간
        public int damage;
        public GameObject hitEffect; // 충돌 효과

        private Transform target; // 타겟 (플레이어)
        #endregion
        void Start()
        {
            target = GameObject.FindGameObjectWithTag("Player").transform; // 플레이어 찾기
            Destroy(gameObject, lifetime); // 일정 시간이 지나면 삭제
        }

        void Update()
        {
            if (target == null)
            {
                Destroy(gameObject);
                return;
            }

            // 플레이어 방향으로 이동
            Vector3 direction = (target.position - transform.position).normalized;
            transform.position += direction * speed * Time.deltaTime;
            transform.LookAt(target); // 발사체가 플레이어를 향하도록 설정
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player")) // 플레이어와 충돌 시
            {
                other.GetComponent<PlayerParams>().TakeDamage(damage); // 플레이어에게 데미지 적용

                if (hitEffect != null)
                {
                    GameObject effect = Instantiate(hitEffect, transform.position, Quaternion.identity); // 충돌 효과 생성
                    Destroy(effect, 3f);
                }

                Destroy(gameObject); // 발사체 삭제
            }
        }
    }
}
