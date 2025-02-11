using MyRPG.camera;
using MyRPG.Manager;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MyRPG.Player
{
    public class PlayerParams : CharacterParams
    {
        #region Variables
        [HideInInspector]public int expToNextLevel { get; set; }
        [HideInInspector] public string playerName = "ch";
        [HideInInspector]public int curLevel = 1;
        [HideInInspector] public float curMaxHP = 100f;
        [HideInInspector] public int curAttackMin = 5;
        [HideInInspector] public int curAttackMax = 10;
        [HideInInspector] public int curDefense = 3;
        [HideInInspector] public int curExp = 0;
        [HideInInspector] public int curMoney = 0;
        [HideInInspector] public Vector3 curPosition;
        private float healDelay = 3f; // 피해를 입은 후 회복 시작까지의 대기 시간
        private float lastDamageTime; // 마지막으로 피해를 입은 시간
        public Image expImg;
        public TextMeshProUGUI exptext;
        private Camera mainCam;
        private CameraControl control; 
        #endregion

        public override void InitParams()
        {
            mainCam = Camera.main;
            control = mainCam.GetComponent<CameraControl>();
            data = SaveSystem.LoadDataPlayer();
            if (data != null)
            {
                LoadPlayerData();
                curHP = data.maxHP;
                expToNextLevel = 100 * curLevel;
                if (UIManager.instance != null)
                {
                    UIManager.instance.UpdatePlayerUI(this);
                }
                Debug.Log("저장이 잘 되어 로드합니다");
            }
            else
            {
                SavePlayerData();
                data = SaveSystem.LoadDataPlayer();
                curHP = data.maxHP;
                expToNextLevel = 100 * curLevel;
                Debug.Log("저장한 데이터가 없습니다");
            }
        }
        protected override void UpdateAfterReceiveAttack()
        {
            base.UpdateAfterReceiveAttack();
            lastDamageTime = Time.time; // 피해를 입은 시간 갱신
        }
        public void StartHealing(float healRate = 5f)
        {
            if (curHP < data.maxHP)
            {
                // 마지막으로 피해를 입은 후 일정 시간이 지나면 회복 시작
                if (Time.time - lastDamageTime >= healDelay)
                {
                    curHP += healRate * Time.deltaTime;
                    curHP = Mathf.Clamp(curHP, 0, data.maxHP); // 체력 초과 방지
                }
            }
        }
        public void AddExperience(int amount)
        {
            curExp += amount;
            while (curExp >= expToNextLevel)
            {
                curExp -= expToNextLevel;
                LevelUp();
            }
            SaveSystem.SaveDataPlayer(data);
        }
        private void LevelUp()
        {
            curLevel++;
            expToNextLevel = 100 * curLevel;

            curMaxHP += 10;
            data.maxHP = curMaxHP;
            curAttackMax += 5;
            curAttackMin += 2;

            if (curLevel % 5 == 0)
            {
                curDefense += 2;
            }
        }
        public void SavePlayerData()
        {
            data = new CharacterData
            {
                Name = playerName,
                level = curLevel,
                maxHP = curMaxHP,
                attackMax = curAttackMax,
                attackMin = curAttackMin,
                defense = curDefense,
                Exp = curExp,
                money = curMoney,
                position = new float[] { transform.position.x, transform.position.y, transform.position.z },
                camOffSet = new float[] {control.GetCameraOffset().x,control.GetCameraOffset().y,control.GetCameraOffset().z}
            };
            SaveSystem.SaveDataPlayer(data);
        }
        public void LoadPlayerData()
        {
            data = SaveSystem.LoadDataPlayer();
            if (data != null)
            {
                playerName = data.Name;
                curLevel = data.level;
                curMaxHP = data.maxHP;
                curAttackMax = data.attackMax;
                curAttackMin = data.attackMin;
                curDefense = data.defense;
                curExp = data.Exp;
                curMoney = data.money;
                curPosition = new Vector3(data.position[0], data.position[1], data.position[2]);
                transform.position = curPosition;
                control.UpdateCameraPosition(new Vector3(data.camOffSet[0], data.camOffSet[1], data.camOffSet[2]));
                Debug.Log($"로드 완료!");
            }
            else
            {
                Debug.Log($"실패");
            }
            
        }
        private void OnApplicationQuit()
        {
            SaveSystem.SaveDataPlayer(data);
        }
    }
}