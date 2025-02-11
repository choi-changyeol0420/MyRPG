using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;

namespace MyRPG.Manager
{
    public static class SaveSystem
    {
        #region Variables
        private static string path = Path.Combine(Application.persistentDataPath, "SaveData.json");
        #endregion
        public static void SaveDataPlayer(CharacterData saveData)
        {
            string json = JsonConvert.SerializeObject(saveData, Formatting.Indented);
            File.WriteAllText(path, json);
            Debug.Log("게임 데이터 저장 완료! " + path);
        }
        public static CharacterData LoadDataPlayer()
        {
            if(File.Exists(path))
            {
                string json = File.ReadAllText(path);
                Debug.Log("저장된 파일을 불러왔습니다");
                return JsonConvert.DeserializeObject<CharacterData>(json);
            }
            else
            {
                Debug.Log("저장된 파일이 업습니다");
                return new CharacterData();
            }
        }
    }
    [System.Serializable]
    public class SaveKeybind
    {
        public List<string> actionKeys;
        public List<int> Keys;
        public SaveKeybind(Dictionary<string,KeyCode> keyData)
        {
            actionKeys = new List<string>();
            Keys = new List<int>();

            foreach(var key in keyData)
            {
                actionKeys.Add(key.Key);
                Keys.Add((int)key.Value);
            }
        }
        public Dictionary<string,KeyCode> ToDictionary()
        {
            Dictionary<string,KeyCode> keyData = new Dictionary<string,KeyCode>();
            for(int i = 0; i < actionKeys.Count; i++)
            {
                keyData[actionKeys[i]] = (KeyCode)Keys[i];
            }
            return keyData;
        }
    }
    public class SaveManager : MonoBehaviour
    {
        #region Singleton
        public static SaveManager Instance { get; private set; }
        #endregion
        #region Variables
        private string keyPath;
        public Dictionary<string, KeyCode> keyData = new Dictionary<string, KeyCode>();
        #endregion
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
            keyPath = Path.Combine(Application.persistentDataPath, "keyData.json");
        }
        private void Start()
        {
            LoadKeybind();
        }
        private void SetDefaultKey()
        {
            keyData["Run"] = KeyCode.LeftShift;
            keyData["Attack"] = KeyCode.Mouse0;
        }
        public void SaveKeyData()
        {
            SaveKeybind data = new SaveKeybind(keyData);
            string json = JsonUtility.ToJson(data,true);
            File.WriteAllText(keyPath, json);
        }
        public void LoadKeybind()
        {
            if(File.Exists(keyPath))
            {
                string json = File.ReadAllText(keyPath);
                SaveKeybind data = JsonUtility.FromJson<SaveKeybind>(json);
                keyData = data.ToDictionary();
            }
            else
            {
                SetDefaultKey();
                SaveKeyData();
            }
        }
        public void ChangeKey(string key, KeyCode newkey)
        {
            if(keyData.ContainsKey(key))
            {
                keyData[key] = newkey;
                SaveKeyData();
            }
        }
        public KeyCode GetKey(string action)
        {
            return keyData.ContainsKey(action) ? keyData[action] : KeyCode.None;
        }
    }
}