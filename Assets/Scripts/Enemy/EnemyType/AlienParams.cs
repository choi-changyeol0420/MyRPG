using UnityEngine;

namespace MyRPG.Enemy
{
    public class AlienParams : EnemyParams
    {
        #region Variables

        #endregion
        public override void InitParams()
        {
            enemyName = "Alien";
            level = 1;
            maxHP = 50;
            curHP = maxHP;
            attackMax = 5;
            attackMin = 2;
            defense = 1;
            exp = 10;
            rewardMoney = Random.Range(10, 20);
        }
        protected override void UpdateAfterReceiveAttack()
        {
            base.UpdateAfterReceiveAttack();
        }
    }
}