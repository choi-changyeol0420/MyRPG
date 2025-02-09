using MyRPG.Manager;
using System.Collections.Generic;
using UnityEngine;

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

        public const string playerCurExp = "playerCurExp";
        public const string playerlevel = "playerlevel";
        public const string playerMaxHp = "playerMaxHp";
        public const string playerCurHp = "playerCurHp";
        public const string playerAttackMax = "playerAttackMax";
        public const string playerAttackMin = "playerAttackMin";
        public const string playerDefense = "playerDefense";
        public const string playerMoney = "playerMoney";
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
            if (curHP < maxHP)
            {
                // 마지막으로 피해를 입은 후 일정 시간이 지나면 회복 시작
                if (Time.time - lastDamageTime >= healDelay)
                {
                    curHP += healRate * Time.deltaTime;
                    curHP = Mathf.Clamp(curHP, 0, maxHP); // 체력 초과 방지
                }
            }
        }
        public void AddExperience(int amount)
        {
            curExp += amount;
            while (curExp >= expToNextLevel)
            {
                curExp -= expToNextLevel;
                LevelUp();
            }
            OnPlayerStateSave();
        }
        private void LevelUp()
        {
            level++;
            expToNextLevel = 100 * level;

            maxHP += 10;
            attackMax += 5;
            attackMin += 2;

            if(level % 5 == 0)
            {
                defense += 2;
            }
        }
        public void OnPlayerStateSave()
        {
            Dictionary<string, object> data = new Dictionary<string, object>()
            {
                {playerCurExp ,curExp },
                {playerlevel , level },
                {playerMaxHp ,maxHP},
                {playerCurHp ,curHP },
                {playerAttackMax ,attackMax },
                {playerAttackMin ,attackMin },
                {playerDefense ,defense },
                {playerMoney ,money }
            };
            SaveManager.Instance.SaveData(data);
        }
        private void OnApplicationQuit()
        {
            OnPlayerStateSave();
        }
    }
}