using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Experimental.GlobalIllumination;

namespace MyRPG.Player
{
    public class PlayerParams : CharacterParams
    {
        #region Variables
        public string playerName {  get; set; }
        public int curExp { get; set; }
        public int expToNextLevel { get; set; }
        public int money {  get; set; }

        float healDelay = 3f; // 피해를 입은 후 회복 시작까지의 대기 시간
        private float lastDamageTime; // 마지막으로 피해를 입은 시간
        #endregion

        public override void InitParams()
        {
            playerName = "ch";
            level = 1;
            maxHP = 100;
            curHP = maxHP;
            attackMin = 5;
            attackMax = 10;
            defense = 5;

            curExp = 0;
            expToNextLevel = 100 * level;
            money = 0;

            isDie = false;
            UIManager.instance.UpdatePlayerUI(this);
        }
        protected override void UpdateAfterReceiveAttack()
        {
            base.UpdateAfterReceiveAttack();
            lastDamageTime = Time.time; // 피해를 입은 시간 갱신
        }
        public void StartHealing(float healRate = 5f)
        {
            if (this.curHP < this.maxHP)
            {
                // 마지막으로 피해를 입은 후 일정 시간이 지나면 회복 시작
                if (Time.time - lastDamageTime >= healDelay)
                {
                    this.curHP += healRate * Time.deltaTime;
                    this.curHP = Mathf.Clamp(this.curHP, 0, maxHP); // 체력 초과 방지
                }
            }
        }
    }
}