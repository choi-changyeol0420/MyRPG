using MyRPG.camera;
using MyRPG.Manager;
using MyRPG.Player;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace MyRPG
{
    public class CharacterData
    {
        #region Variables
        public string Name { get; set; }
        public int level { get; set; }
        public float maxHP { get; set; }
        public int attackMax { get; set; }
        public int attackMin { get; set; }
        public int defense { get; set; }
        public int Exp { get; set; }
        public int money { get; set; }
        public float[] position { get; set; }
        public float[] camOffSet { get; set; }
        #endregion
    }
    /// <summary>
    /// CharacterParams는 플레이어의 파라미터 클래스와 몬스터 파라미터 클래스의 부모 클래스 역활을 하게 됨
    /// </summary>
    public abstract class CharacterParams : MonoBehaviour
    {
        //퍼블릭 변수가 아니라 약식프로퍼티, 속성으로 지정
        //퍼블릭 변수와 똑같이 사용할 수 있지만 유니티 인스펙터에 노출되는 것을 막고 보안을 위해 정식 프로퍼티로 전환이 쉬워짐
        #region Variables
        public PlayerStat stat = new PlayerStat();
        public CharacterData data;

        protected Camera mainCam;
        protected CameraControl control;
        //체력 회복
        protected float healDelay = 3f; // 피해를 입은 후 회복 시작까지의 대기 시간
        protected private float lastDamageTime; // 마지막으로 피해를 입은 시간
        public Image healthBar;
        public Image expImg;
        public TextMeshProUGUI exptext;

        public UnityAction<GameObject> dieEvent;
        public UnityAction OnLevelUp;
        public UnityAction OnStatsUpdate;
        #endregion
        //나중에 CharacterParams 클래스를 상속한 자식클래스에서 InitParams 함수에 자신만의 명령어를 추가하기만 하면 자동으로 필요한 명령어들이 실행
        public void InitParams()
        {
            mainCam = Camera.main;
            control = mainCam.GetComponent<CameraControl>();
            if (data != null)
            {
                LoadPlayerData();
                stat.curHP = data.maxHP;
                stat.expToNextLevel = 100 * stat.level;
                if (UIManager.instance != null)
                {
                    UIManager.instance.UpdatePlayerUI(this);
                }
                Debug.Log("저장이 잘 되어 로드합니다");
            }
            else if (data == null)
            {
                SavePlayerData();
                data = SaveSystem.LoadDataPlayer();
                stat.curHP = data.maxHP;
                stat.expToNextLevel = 100 * stat.level;
                Debug.Log("저장한 데이터가 없습니다");
            }
        }
        public void SavePlayerData()
        {
            data = new CharacterData
            {
                Name = stat.playerName,
                level = stat.level,
                maxHP = stat.maxHP,
                attackMax = stat.attackMax,
                attackMin = stat.attackMin,
                defense = stat.defense,
                Exp = stat.exp,
                money = stat.money,
                position = new float[] { transform.position.x, transform.position.y, transform.position.z },
                camOffSet = new float[] { control.GetCameraOffset().x, control.GetCameraOffset().y, control.GetCameraOffset().z }
            };
            SaveSystem.SaveDataPlayer(data);
        }
        public void LoadPlayerData()
        {
            data = SaveSystem.LoadDataPlayer();
            if (data != null)
            {
                stat.playerName = data.Name;
                stat.level = data.level;
                stat.maxHP = data.maxHP;
                stat.attackMax = data.attackMax;
                stat.attackMin = data.attackMin;
                stat.defense = data.defense;
                stat.exp = data.Exp;
                stat.money = data.money;
                stat.curPosition = new Vector3(data.position[0], data.position[1], data.position[2]);
                transform.position = stat.curPosition;
                control.UpdateCameraPosition(new Vector3(data.camOffSet[0], data.camOffSet[1], data.camOffSet[2]));
                Debug.Log($"로드 완료!");
            }
            else
            {
                Debug.Log($"실패");
            }
        }
        protected void OnApplicationQuit()
        {
            SaveSystem.SaveDataPlayer(data);
        }
    }
}