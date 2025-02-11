using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace MyRPG.Enemy
{
    public class EnemyParams : MonoBehaviour,IDamageable
    {
        #region Variables
        public string enemyName;
        public int level { get; set; }
        public float maxHP { get; set; }
        public int attackMax { get; set; }
        public int attackMin { get; set; }
        public int defense { get; set; }
        public int exp {  get; set; }
        public int rewardMoney { get; set; }
        public float curHP { get; set; }
        public bool isDie { get; set; }
        public UnityAction<GameObject> dieEvent;
        public Image healthBar;
        #endregion
        public int GetRandomAttack()
        {
            int randAttack = Random.Range(attackMin, attackMax + 1);
            return randAttack;
        }
        public virtual void InitParams()
        {

        }

        public void TakeDamage(float damage)
        {
            if (isDie) return;
            // 방어력 적용 공식: 받은 피해량 = 기본 피해량 - (방어력 * 피해 감소율)
            float damageReduction = defense * 0.2f; // 방어력의 20%만큼 피해 감소
            float finalDamage = Mathf.Max(damage - damageReduction, 1); // 최소 1 이상의 피해 적용
            curHP -= finalDamage;
            UpdateAfterReceiveAttack();
        }

        protected virtual void UpdateAfterReceiveAttack()
        {
            if(curHP < 0)
            {
                curHP = 0;
                isDie = true;
                dieEvent?.Invoke(gameObject);
            }
        }
    }
}