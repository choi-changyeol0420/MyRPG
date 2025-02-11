using MyRPG.Manager;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MyRPG.Player
{
    public class PlayerKeyUI : MonoBehaviour
    {
        #region Variables
        public Button RunKeyButton;
        public Button AttackKeyButton;
        private string currentKeyToRebind = "";
        #endregion
        private void Start()
        {
            RunKeyButton.onClick.AddListener(() => StartRebinding("Run"));
            AttackKeyButton.onClick.AddListener(() => StartRebinding("Attack"));

            UpdateButtonText();
        }
        private void Update()
        {
            if(currentKeyToRebind != "" && Input.anyKeyDown)
            {
                foreach(KeyCode keyCode in System.Enum.GetValues(typeof(KeyCode)))
                {
                    if(Input.GetKeyDown(keyCode))
                    {
                        SaveManager.Instance.ChangeKey(currentKeyToRebind, keyCode);
                        currentKeyToRebind = "";
                        UpdateButtonText();
                        break;
                    }
                }
            }
        }
        private void StartRebinding(string action)
        {
            currentKeyToRebind = action;
            Debug.Log("키를 입력해주세요");
        }
        private void UpdateButtonText()
        {
            TextMeshProUGUI RunText = RunKeyButton.GetComponentInChildren<TextMeshProUGUI>();
            RunText.text = SaveManager.Instance.GetKey("Run").ToString();
            TextMeshProUGUI AttackText = AttackKeyButton.GetComponentInChildren<TextMeshProUGUI>();
            AttackText.text = SaveManager.Instance.GetKey("Attack").ToString();
        }
    }
}