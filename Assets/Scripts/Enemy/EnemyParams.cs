using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace MyRPG.Enemy
{
    public class EnemyParams : MonoBehaviour,IDamageable
    {
        #region Variables
        public EnemyAttribute enemies;
        public int level { get; set; }
        public int attackMax { get; set; }
        public int attackMin { get; set; }
        public int defense { get; set; }
        public int exp {  get; set; }
        public int rewardMoney { get; set; }
        public float maxHP { get; set; }
        public float curHP { get; set; }
        public bool isDie { get; set; }
        public UnityAction<GameObject> dieEvent;
        //UI
        public Image healthBar;
        public TextMeshProUGUI enemyName;

        private System.Random random = new System.Random();
        #endregion
        public int GetRandomAttack()
        {
            int randAttack = Random.Range(enemies.attackMin, enemies.attackMax + 1);
            return randAttack;
        }
        public int GetRandomMoney()
        {
            rewardMoney = Random.Range(enemies.rewardMoneyMin, enemies.rewardMoneyMax + 1);
            return rewardMoney;
        }

        public void InitParams()
        {
            enemyName.gameObject.SetActive(true);
            enemyName.text = enemies.enemyname;
            level = enemies.level;
            attackMax = enemies.attackMax;
            attackMin = enemies.attackMin;
            maxHP = enemies.maxHp;
            curHP = maxHP;
            defense = enemies.defense;
            exp = enemies.exp;
            GetRandomMoney();
            isDie = false;
        }

        public void TakeDamage(float damage, float CritChance = 0, float critMult = 2)
        {
            if (isDie) return;
            enemyName.gameObject.SetActive(true);
            healthBar.gameObject.SetActive(true);
            bool isCritical = random.NextDouble() * 100 < CritChance;
            if (isCritical)
            {
                damage *= critMult;
                Debug.Log("크리티컬");
            }
            // 방어력 적용 공식: 받은 피해량 = 기본 피해량 - (방어력 * 피해 감소율)
            float damageReduction = enemies.defense * 0.2f; // 방어력의 20%만큼 피해 감소
            float finalDamage = Mathf.Max(damage - damageReduction, 1); // 최소 1 이상의 피해 적용
            curHP -= finalDamage;
            Debug.Log(finalDamage);
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