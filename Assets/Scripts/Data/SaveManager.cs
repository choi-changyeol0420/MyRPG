using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using UnityEngine;
using UnityEngine.Rendering;

namespace MyRPG.Manager
{
    [System.Serializable]
    public class SaveDataWrapper
    {
        public List<string> keys;
        public List<string> values;
        public SaveDataWrapper(Dictionary<string, object> data)
        {
            keys = new List<string>();
            values = new List<string>();

            foreach (var pair in data)
            {
                keys.Add(pair.Key);
                values.Add(pair.Value.ToString());
            }
        }
        public Dictionary<string, object> ToDictionary()
        {
            Dictionary<string, object> data = new Dictionary<string, object>();
            for (int i = 0; i < keys.Count; i++)
            {
                int intValue;
                float floatValue;
                if (int.TryParse(values[i], out intValue))
                {
                    data[keys[i]] = intValue;
                }
                else if (float.TryParse(values[i], out floatValue))
                {
                    data[keys[i]] = floatValue;
                }
                else
                {
                    data[keys[i]] = values[i];
                }
            }
            return data;
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
        private string savePath;
        private string keyPath;
        public Dictionary<string, KeyCode> keyData = new Dictionary<string, KeyCode>();
        #endregion
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
            keyPath = Path.Combine(Application.persistentDataPath, "keyData.json");
            savePath = Path.Combine(Application.persistentDataPath, "saveData.json");
        }
        private void Start()
        {
            LoadData();
        }
        // ✅ 데이터 저장 (Dictionary 기반)
        public void SaveData(Dictionary<string, object> data)
        {
            string json = JsonUtility.ToJson(new SaveDataWrapper(data), true);
            byte[] compressedData = CompressString(json);
            File.WriteAllBytes(savePath, compressedData);
            Debug.Log("데이터 저장 완료: " + savePath);
        }
        public Dictionary<string, object> LoadData()
        {
            if (File.Exists(savePath))
            {
                byte[] compressedData = File.ReadAllBytes(savePath);
                string json = DecompressString(compressedData);
                SaveDataWrapper weapper = JsonUtility.FromJson<SaveDataWrapper>(json);
                Debug.Log("📂 데이터 로드 완료!");
                return weapper.ToDictionary();
            }
            Debug.Log("저장한 데이터가 없습니다");
            return new Dictionary<string, object>(); //기본값 반환
        }
        private byte[] CompressString(string str)
        {
            using (MemoryStream ms = new MemoryStream())
                using (GZipStream gZipStream = new GZipStream(ms, CompressionMode.Compress))
                using (StreamWriter sw = new StreamWriter(gZipStream))
            {
                sw.Write(str);
                sw.Close();
                return ms.ToArray();
            }
        }
        private string DecompressString(byte[] data)
        {
            using (MemoryStream ms = new MemoryStream(data))
            using (GZipStream gZipStream = new GZipStream(ms, CompressionMode.Decompress))
            using (StreamReader reader = new StreamReader(gZipStream))
            {
                return reader.ReadToEnd();
            }
        }
        private void CleanUpOldSaves()
        {
            string saveDirectory = Path.GetDirectoryName(savePath);
            string[] saveFiles = Directory.GetFiles(saveDirectory, "saveData_*.json");

            if (saveFiles.Length > 5)
            {
                // 파일 생성 날짜 기준으로 정렬 후 가장 오래된 파일 삭제
                Array.Sort(saveFiles, (a, b) => File.GetCreationTime(a).CompareTo(File.GetCreationTime(b)));
                File.Delete(saveFiles[0]);
                Debug.Log("🗑️ 오래된 저장 파일 삭제: " + saveFiles[0]);
            }
        }

        private void SetDefaultKey()
        {
            keyData["Run"] = KeyCode.LeftShift;
            keyData["Attack"] = KeyCode.Q;
        }
        public void SaveKeyData()
        {
            SaveKeybind data = new SaveKeybind(keyData);
            string json = JsonUtility.ToJson(data,true);
            File.WriteAllText(savePath, json);
        }
        public void LoadKeybind()
        {
            if(!File.Exists(keyPath))
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