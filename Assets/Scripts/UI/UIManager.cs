using MyRPG.Player;
using TMPro;
using UnityEngine;

namespace MyRPG
{
    public class UIManager : MonoBehaviour
    {
        #region SingleTon
        public static UIManager instance;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
        }
        #endregion
        #region Variables
        public TextMeshProUGUI nameText;
        public TextMeshProUGUI moneyText;
        public TextMeshProUGUI healthText;
        
        #endregion

        public void UpdatePlayerUI(PlayerParams player)
        {
            if (player.isDie) return;
            if(player.data != null)
            {
                nameText.text = player.playerName + "      Level:" + player.curLevel.ToString();
                moneyText.text = "Money" + player.data.money.ToString();
                healthText.text = player.curHP.ToString("F0") + "/" + player.data.maxHP.ToString();
                player.healthBar.fillAmount = Mathf.Lerp(player.healthBar.fillAmount, player.curHP / player.data.maxHP, Time.deltaTime * 5f);
                player.expImg.fillAmount = Mathf.Lerp(player.expImg.fillAmount, (float)player.curExp / (float)player.expToNextLevel, Time.deltaTime * 5f);
                player.exptext.text = "Exp :" + player.curExp.ToString() + "/" +player.expToNextLevel.ToString();
            }
        }
    }
}