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
        public Image playerhealthfill;
        #endregion

        public void UpdatePlayerUI(PlayerParams player)
        {
            nameText.text = player.playerName;
            moneyText.text = player.money.ToString();
            healthText.text = player.curHP.ToString() + "/" + player.maxHP.ToString();
            playerhealthfill.fillAmount = Mathf.Lerp(playerhealthfill.fillAmount, player.curHP / player.maxHP,Time.deltaTime*5f);
        }
    }
}