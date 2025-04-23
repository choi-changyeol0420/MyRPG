using MyRPG.Effect;
using MyRPG.Drone;
using UnityEngine;

namespace MyRPG.Player
{
    public class DroneProjectile : TriggerEffect
    {
        #region Variables
        public float speed = 10f;
        public float lifeTime = 5f;
        [HideInInspector]public int damage;
        public string poolTag;

        private Transform target;
        private bool isInitialized = false;
        #endregion

        private void OnEnable()
        {
            if (!isInitialized)
            {
                target = GameObject.FindGameObjectWithTag("EnemyPoint").transform;
                isInitialized = true;
            }
            
            // 발사체가 활성화될 때 자동으로 비활성화되도록 설정
            CancelInvoke();
            Invoke(nameof(DisableProjectile), lifeTime);
        }

        private void Update()
        {
            if(!target)
            {
                DisableProjectile();
                return;
            }
            //적을 방향으로 이동
            Vector3 direction = (target.position - transform.position).normalized;
            transform.position += direction * speed * Time.deltaTime;
            transform.LookAt(target);
        }

        private void DisableProjectile()
        {
            if (ObjectPool.Instance != null)
            {
                ObjectPool.Instance.ReturnToPool(poolTag, gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public override void EffectTrigger(Collider other)
        {
            IDamageable damageable = other.GetComponent<IDamageable>();
            if(damageable != null)
            {
                damageable.TakeDamage(damage);
                if (damageEffect)
                {
                    damageEff = Instantiate(damageEffect, transform.position, Quaternion.identity);
                    Destroy(damageEff, 1);
                }
            }
            DisableProjectile();
        }
    }
}