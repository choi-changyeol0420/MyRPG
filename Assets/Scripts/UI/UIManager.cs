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

        public TextMeshProUGUI[] statsPointText;
        public Button[] increaseButton;
        public Button[] decreaseButton;
        public Button applyButton;
        #endregion
        public void UpdatePlayerUI(CharacterParams character)
        {
            if (character.stat.isDie) return;
            if(character.data != null)
            {
                nameText.text = character.stat.playerName + "      Level:" + character.stat.level.ToString();
                moneyText.text = "Money" + character.data.money.ToString();
                healthText.text = character.stat.curHP.ToString("F0") + "/" + character.data.maxHP.ToString();
                character.healthBar.fillAmount = Mathf.Lerp(character.healthBar.fillAmount, character.stat.curHP / character.data.maxHP, Time.deltaTime * 5f);
                character.expImg.fillAmount = Mathf.Lerp(character.expImg.fillAmount, (float)character.stat.exp / (float)character.stat.expToNextLevel, Time.deltaTime * 5f);
                character.exptext.text = "Exp :" + character.stat.exp.ToString() + "/" +character.stat.expToNextLevel.ToString();
            }
        }
        public void AddOnClick(PlayerParams player)
        {
            increaseButton[0].onClick.AddListener(() => player.IncreaseStr());
            increaseButton[1].onClick.AddListener(() => player.IncreaseDex());
            increaseButton[2].onClick.AddListener(() => player.IncreaseInt());
            increaseButton[3].onClick.AddListener(() => player.IncreaseDef());

            decreaseButton[0].onClick.AddListener(() => player.DecreaseStr());
            decreaseButton[1].onClick.AddListener(() => player.DecreaseDex());
            decreaseButton[2].onClick.AddListener(() => player.DecreaseInt());
            decreaseButton[3].onClick.AddListener(() => player.DecreaseDef());

            applyButton.onClick.AddListener(() => player.ApplyStats());
        }
        public void UpdateUI()
        {

        }
    }
}