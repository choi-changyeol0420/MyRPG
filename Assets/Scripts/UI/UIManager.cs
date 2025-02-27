using System.Collections.Generic;
using MyRPG.Manager;
using MyRPG.Player;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using MyRPG.Drone;

namespace MyRPG
{
    [System.Serializable]
    public class DroneButton
    {
        public Button[] droneButton;
        public bool isDrone;
    }
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
        public GameObject statImg;
        private StatButtonUI[] buttonUI = new StatButtonUI[4];
        private GameObject[] statUI = new GameObject[4];
        
        public List<DroneButton> droneList;
        public Button droneCanvasButton;
        public GameObject droneCanvas;
        public GameObject currentDrone;
        public SelectDrone selectDrone;
        public WarningPopup warningPopup;
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
            droneCanvasButton.onClick.AddListener(() => droneCanvas.SetActive(!droneCanvas.activeSelf));

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
            for(int i = 0; i < droneList.Count; i++)
            {
                for(int j = 0; j < droneList[i].droneButton.Length; j++)
                {
                    SelectDrone(i, j, player.GetComponent<PlayerFSM>());
                }
            }
            applyButton.onClick.AddListener(() => player.ApplyStats());
        }

        public void UpdateUI(PlayerParams player)
        {
            statImg .SetActive(player.stat.statPoints > 0); 
            pointText.text = player.stat.statPoints.ToString();

            statsText[0].text = "Str: " + player.stat.strength.ToString();
            statsText[1].text = "Dex: " + player.stat.dexterity.ToString();
            statsText[2].text = "Cyb: " + player.stat.cybernetics.ToString();
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
        public void SelectDrone(int index,int index2,PlayerFSM player)
        {
            PlayerParams playerParams = player.GetComponent<PlayerParams>();
            switch(index)
            {
                case 0:
                    switch(index2)
                    {
                        case 0:
                            droneList[0].droneButton[0].onClick.AddListener(() => SelectDroneButton(0,player));
                            droneList[0].isDrone = true;
                            break;
                        case 1:
                            droneList[0].droneButton[1].onClick.AddListener(() => SelectDroneButton(1,player));
                            droneList[0].isDrone = true;
                            break;
                        case 2:
                            droneList[0].droneButton[2].onClick.AddListener(() => SelectDroneButton(2,player));
                            droneList[0].isDrone = true;
                            break;
                    }
                break;
                case 1:
                    if(playerParams.stat.level >= 10 && playerParams.stat.cybernetics >= 30 && droneList[0].isDrone)
                    {
                        switch(index2)
                        {
                            case 0:
                                droneList[1].droneButton[0].onClick.AddListener(() => SelectDroneButton(3,player));
                                droneList[1].isDrone = true;
                                break;
                            case 1:
                                droneList[1].droneButton[1].onClick.AddListener(() => SelectDroneButton(4,player));
                                droneList[1].isDrone = true;
                                break;
                            case 2:
                                droneList[1].droneButton[2].onClick.AddListener(() => SelectDroneButton(5,player));
                                droneList[1].isDrone = true;
                                break;
                        }
                    }
                    else
                    {
                        warningPopup.SetWarningText("레벨 10 이상과 Cyb 30 이상이 되어야 업그레이드 할 수 있습니다.");
                    }
                    break;
                case 2:
                    if(playerParams.stat.level >= 30 && playerParams.stat.cybernetics >= 80 && droneList[1].isDrone)
                    {
                        switch(index2)
                        {
                            case 0:
                                droneList[2].droneButton[0].onClick.AddListener(() => SelectDroneButton(6,player));
                                droneList[2].isDrone = true;
                                break;
                            case 1:
                                droneList[2].droneButton[1].onClick.AddListener(() => SelectDroneButton(7,player));
                                droneList[2].isDrone = true;
                                    break;
                            case 2:
                                droneList[2].droneButton[2].onClick.AddListener(() => SelectDroneButton(8,player));
                                droneList[2].isDrone = true;
                                break;
                        }
                    }
                    else
                    {
                        warningPopup.SetWarningText("레벨 30 이상과 Cyb 80 이상이 되어야 업그레이드 할 수 있습니다.");
                    }
                    break;
            }
        }
        public void SelectDroneButton(int index,PlayerFSM player)
        {
            PlayerParams playerParams = player.GetComponent<PlayerParams>();
            currentDrone.SetActive(true);
            selectDrone.SelectDrones(index);
            selectDrone.selectButton.onClick.AddListener(() => SelectDrones(player,selectDrone));
        }
        void SelectDrones(PlayerFSM player, SelectDrone drone)
        {
            Debug.Log(player.dronePrefab);
            DroneFSM droneFSM = player.dronePrefab.GetComponent<DroneFSM>();
            droneFSM.UpgradeDrone();
            player.dronePrefab = null;
            player.dronePrefab = drone.drone;
        }
    }
    public enum StatType
    {
        STR,
        DEX,
        CYB,
        DEF
    }
}

