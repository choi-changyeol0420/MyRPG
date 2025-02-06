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
        [HideInInspector]public Coroutine healCoroutine; // 코루틴을 저장할 변수 
        #endregion

        public override void InitParams()
        {
            playerName = "ch";
            level = 1;
            maxHP = 100;
            curHP = maxHP;
            attackMin = 5;
            attackMax = 10;
            defense = 1;

            curExp = 0;
            expToNextLevel = 100 * level;
            money = 0;

            isDie = false;
            UIManager.instance.UpdatePlayerUI(this);
        }
        protected override void UpdateAfterReceiveAttack()
        {
            base.UpdateAfterReceiveAttack();
        }
        public void StartHealing(float healRate = 5f)
        {
            if (healCoroutine == null) // 중복 실행 방지
            {
                healCoroutine = StartCoroutine(HealOverTime(healRate));
            }
        }
        IEnumerator HealOverTime(float healRate)
        {
            float healInterval = 2; // 회복 간격

            while (curHP < maxHP)
            {
                curHP += healRate * 0.05f; // 일정량씩 회복
                curHP = Mathf.Clamp(curHP, 0, maxHP); // 체력 초과 방지

                yield return new WaitForSeconds(healInterval); // 일정 시간 대기
            }

            healCoroutine = null; // 회복 완료 후 코루틴 초기화
        }
    }
}