using UnityEngine;
using UnityEngine.UI;

namespace MyRPG.Enemy
{
    public class EnemyParams : CharacterParams
    {
        #region Variables
        public string enemyName;
        public int exp {  get; set; }
        public int rewardMoney { get; set; }
        public Image hpBar;
        #endregion
        public override void InitParams()
        {
            enemyName = "Monster";
            level = 1;
            maxHP = 50;
            curHP = maxHP;
            attackMin = 2;
            attackMax = 5;
            defense = 1;

            exp = 10;
            rewardMoney = Random.Range(10, 20);
            isDie = false;
        }
        protected override void UpdateAfterReceiveAttack()
        {
            base.UpdateAfterReceiveAttack();
        }
        private void Update()
        {
            float speed = 2f;
            hpBar.fillAmount = Mathf.Lerp(hpBar.fillAmount, curHP / maxHP, Time.deltaTime * speed);
        }
    }
}