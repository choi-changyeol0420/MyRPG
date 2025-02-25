using MyRPG.camera;
using MyRPG.Manager;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace MyRPG.Player
{
    [System.Serializable]
    public class PlayerStat
    {
        public string playerName = "ch";
        public int expToNextLevel { get; set; }
        public int level = 1;
        public int exp = 0;
        public int statPoints = 0;

        public int strength = 5;
        public int dexterity = 5;
        public int cybernetics = 5;
        public int defense = 3;
        public int[] tempStat = new int[4];

        public float maxHP = 100f;
        public float moveSpeed = 3f;
        public float critChance = 10;

        public int attackMin = 3;
        public int attackMax = 5;

        public int money = 0;
        public Vector3 curPosition;

        public float curHP { get; set; }
        public bool isDie { get; set; }
    }
    public class PlayerParams : CharacterParams, IDamageable
    {
        #region Variables
        public float attackTotal;
        #endregion
        public void StartHealing(float healRate = 5f)
        {
            if (stat.curHP < data.maxHP)
            {
                // 마지막으로 피해를 입은 후 일정 시간이 지나면 회복 시작
                if (Time.time - lastDamageTime >= healDelay)
                {
                    stat.curHP += healRate * Time.deltaTime;
                    stat.curHP = Mathf.Clamp(stat.curHP, 0, data.maxHP); // 체력 초과 방지
                }
            }
        }
        public void AddExperience(int amount)
        {
            stat.exp += amount;
            while (stat.exp >= stat.expToNextLevel)
            {
                LevelUp();
            }
            SaveSystem.SaveDataPlayer(data);
        }
        private void LevelUp()
        {
            stat.exp -= stat.expToNextLevel;
            stat.level++;
            stat.expToNextLevel = Mathf.RoundToInt(stat.expToNextLevel * 1.2f);

            stat.statPoints += 5;
            OnLevelUp?.Invoke();
            OnStatsUpdate?.Invoke(this);
        }
        protected float GetRandomAttack()
        {
            attackTotal = Random.Range(data.attackMin, data.attackMax + 1);
            return attackTotal;
        }
        public void TakeDamage(float Damage, float CritChance = 0, float critMult = 2)
        {
            if (stat.isDie) return;
            // 방어력 적용 공식: 받은 피해량 = 기본 피해량 - (방어력 * 피해 감소율)
            float damageReduction = data.defense * 0.2f; // 방어력의 20%만큼 피해 감소
            float finalDamage = Mathf.Max(Damage - damageReduction, 1); // 최소 1 이상의 피해 적용
            stat.curHP -= finalDamage;
            UpdateAfterReceiveAttack();
        }
        //캐릭터가 적으로 부터 공격을 받은 뒤에 자동으로 실행될 함수를 가상함수로 만듬
        private void UpdateAfterReceiveAttack()
        {
            //print(name + "'s HP: " + curHP);
            if (stat.curHP <= 0)
            {
                stat.curHP = 0;
                stat.isDie = true;
                dieEvent?.Invoke(gameObject);
            }
            lastDamageTime = Time.time; // 피해를 입은 시간 갱신
        }
        #region Stat
        private bool IncreaseStat( ref int stats, ref int tempStat, int Points = 1)
        {
            if (stat.statPoints <= 0) return false;
            stats += Points;
            tempStat += Points;
            stat.statPoints -= Points;
            OnStatsUpdate?.Invoke(this);
            return true;
        }
        private bool DecreaseStat(ref int stats, ref int tempStat, int Points = 1)
        {
            if (tempStat <= 0) return false;
            stats -= Points;
            tempStat -= Points;
            stat.statPoints += Points;
            OnStatsUpdate?.Invoke(this);
            return true;
        }
        private void UpdateStat()
        {
            attackTotal = GetRandomAttack() + (stat.strength * 1.2f);
            stat.moveSpeed += stat.dexterity * 0.15f;
            stat.critChance += stat.dexterity * 0.35f;
            stat.maxHP += stat.defense * 0.5f;
        }
        public void IncreaseStat(StatType index)
        {
            switch (index)
            {
                case StatType.STR:
                    IncreaseStat(ref stat.strength, ref stat.tempStat[(int)index]);
                    break;
                case StatType.DEX:
                    IncreaseStat(ref stat.dexterity, ref stat.tempStat[(int)index]);
                    break;
                case StatType.CYB:
                    IncreaseStat(ref stat.cybernetics, ref stat.tempStat[(int)index]);
                    break;
                case StatType.DEF:
                    IncreaseStat(ref stat.defense, ref stat.tempStat[(int)index]);
                    break;
            }
        }
        public void DecreaseStat(StatType index)
        {
            switch(index)
            {
                case StatType.STR:
                    DecreaseStat(ref stat.strength, ref stat.tempStat[(int)index]);
                    break;
                case StatType.DEX:
                    DecreaseStat(ref stat.dexterity, ref stat.tempStat[(int)index]);
                    break;
                case StatType.CYB:
                    DecreaseStat(ref stat.cybernetics, ref stat.tempStat[(int)index]);
                    break;
                case StatType.DEF:
                    DecreaseStat(ref stat.defense, ref stat.tempStat[(int)index]);
                    break;
            }
        }
        public void ApplyStats()
        {
            for(int i = 0; i< stat.tempStat.Length; i++)
            {
                stat.tempStat[i] = 0;
            }
            UIManager.instance.UpdateUI(this);
        }
        #endregion
    }
}