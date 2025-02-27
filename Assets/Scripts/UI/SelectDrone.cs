using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

namespace MyRPG.Drone
{
    [System.Serializable]
    public class Drone
    {
        public GameObject drone;
        public Texture droneImage;
    }
    public class SelectDrone : MonoBehaviour
    {
        #region Variables
        public Button selectButton;
        public RawImage droneImage;
        public TextMeshProUGUI selectText;
        public List<Drone> droneList;
        [HideInInspector] public GameObject drone;
        public GameObject @object;
        #endregion
        void Start()
        {
            selectText.text = "선택";
        }
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                @object.SetActive(false);
            }
        }
        private GameObject SelectDroneButton(int index)
        {
            return droneList[index].drone;
        }
        private Texture SelectDroneImage(int index)
        {
            return droneList[index].droneImage;
        }
        public void SelectDrones(int index)
        {
            droneImage.texture = SelectDroneImage(index);
            drone = SelectDroneButton(index);
        }
    }
}