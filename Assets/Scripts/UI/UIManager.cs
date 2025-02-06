using MyRPG.Player;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
            nameText.text = player.playerName;
            moneyText.text = "Money" + player.money.ToString();
            healthText.text = player.curHP.ToString("F0") + "/" + player.maxHP.ToString();
            player.healthBar.fillAmount = Mathf.Lerp(player.healthBar.fillAmount, player.curHP / player.maxHP,Time.deltaTime*5f);
        }
    }
}