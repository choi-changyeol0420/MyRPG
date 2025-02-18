using MyRPG.Effect;
using UnityEngine;

namespace MyRPG.Player
{
    public class DroneProjectile : TriggerEffect
    {
        #region Variables
        public float speed = 10f;
        public float lifeTime = 5f;
        [HideInInspector]public int damage;

        private Transform target;
        #endregion
        private void Start()
        {
            target = GameObject.FindGameObjectWithTag("EnemyPoint").transform;
            Destroy(gameObject, lifeTime);
        }
        private void Update()
        {
            if(!target)
            {
                Destroy(gameObject);
                return;
            }
            //적을 방향으로 이동
            Vector3 direction = (target.position - transform.position).normalized;
            transform.position += direction * speed * Time.deltaTime;
            transform.LookAt(target);
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
            Destroy(gameObject);
        }
    }
}