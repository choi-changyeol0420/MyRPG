using System.Collections.Generic;
using System.Xml.Serialization;
using System.IO;
using UnityEngine;

namespace MyRPG.Quest
{
    public class Quest
    {
        public string NPC;
        public int ID;
        public string Title;
        public string Description;
        public int KarmaRequired;
    }
   public class QuestData : MonoBehaviour
    {
        #region Variables
        public List<Quest> QuestDataList = new List<Quest>();
        #endregion
        private void Start()
        {
            LoadQuests();
        }
        void LoadQuests()
        {
            string filePath = Application.streamingAssetsPath + "/NPC_Quests.xml";
            if (File.Exists(filePath))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(List<Quest>), new XmlRootAttribute("Quests"));
                using (StreamReader reader = new StreamReader(filePath))
                {
                    QuestDataList = (List<Quest>)serializer.Deserialize(reader);
                }
                Debug.Log("📜 XML 퀘스트 데이터 로드 완료!");
            }
            else
            {
                Debug.LogError("❌ NPC_Quests.xml 파일을 찾을 수 없습니다!");
            }
        }
        public List<Quest> GetQuestsForNPC(string npcName)
        {
            return QuestDataList.FindAll(q => q.NPC == npcName);
        }
    }
}
