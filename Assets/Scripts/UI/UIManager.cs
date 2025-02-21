using MyRPG.Manager;
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

        public TextMeshProUGUI[] statsText;
        public TextMeshProUGUI[] statsPointText = new TextMeshProUGUI[4];
        public TextMeshProUGUI pointText;
        public Button applyButton;

        public Button statCanvasButton;
        public GameObject statCanvas;
        public GameObject statButton;
        public Transform statgroup;

        public GameObject img;

        private StatButtonUI[] buttonUI = new StatButtonUI[4];
        private GameObject[] statUI = new GameObject[4];
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
            statCanvasButton.onClick.AddListener(() => StatCanvasMenu());

            for (int i = 0; i < statUI.Length; i++)
            {
                statUI[i] = Instantiate(statButton, statgroup.position, Quaternion.identity);
                statUI[i].transform.SetParent(statgroup);
                buttonUI[i] = statUI[i].GetComponent<StatButtonUI>();
                int index = i;
                buttonUI[i].increaseButton.onClick.RemoveAllListeners();
                buttonUI[i].increaseButton.onClick.AddListener(() => player.IncreaseStat((StatType)index));
                buttonUI[i].decreaseButton.onClick.RemoveAllListeners();
                buttonUI[i].decreaseButton.onClick.AddListener(() => player.DecreaseStat((StatType)index));
            }
                applyButton.onClick.AddListener(() => player.ApplyStats());
        }

        public void UpdateUI(PlayerParams player)
        {
            img.SetActive(player.stat.statPoints > 0); 
            pointText.text = player.stat.statPoints.ToString();

            statsText[0].text = "Str: " + player.stat.strength.ToString();
            statsText[1].text = "Dex: " + player.stat.dexterity.ToString();
            statsText[2].text = "Int: " + player.stat.intelligence.ToString();
            statsText[3].text = "Def: " + player.stat.defense.ToString();

            for(int i = 0 ;i< buttonUI.Length;i++)
            {
                statsPointText[i] = buttonUI[i].tempStat;
                statsPointText[i].text = player.stat.tempStat[i].ToString();
            }
        }
        public void StatCanvasMenu()
        {
            statCanvas.SetActive(!statCanvas.activeSelf);
        }
    }

    public enum StatType
    {
        STR,
        DEX,
        INT,
        DEF
    }
}

