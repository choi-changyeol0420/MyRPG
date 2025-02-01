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
    }
}