using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

namespace MyRPG
{
    public class WarningPopup : MonoBehaviour
    {
        #region Variables
        public TextMeshProUGUI warningText;
        public Button okButton;
        public Button closeButton;
        public GameObject @object;
        #endregion
        public void SetWarningText(string text, Action Onaction = null, Action Offaction = null, float time = 3)
        {
            @object.SetActive(true);
            warningText.text = text;
            okButton.onClick.AddListener(() =>
            {
                Onaction?.Invoke();
                ClosePopup();
            });
            closeButton.onClick.AddListener(() =>
            {
                Offaction?.Invoke();
                ClosePopup();
            });
            okButton.gameObject.SetActive(Onaction != null);
            closeButton.gameObject.SetActive(Offaction != null);
            if(Onaction == null && Offaction == null)
            {
                Invoke("ClosePopup", time);
                return;
            }
        }
        void ClosePopup()
        {
            @object.SetActive(false);
        }
        

    }
}