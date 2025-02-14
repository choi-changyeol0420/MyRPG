using System.Collections.Generic;
using UnityEngine;

namespace MyRPG.Quest
{
   public class NPC_QuestSystem : MonoBehaviour
   {
        #region Variables
        public QuestData QuestData;
        public string NPCname;
        private List<Quest> npcQuests;
        #endregion
        private void Start()
        {
            npcQuests = QuestData.GetQuestsForNPC(NPCname);
        }
        void AssignRandomQuest()
        {
            if(npcQuests.Count > 0)
            {
                Quest assignedQuest = npcQuests[Random.Range(0, npcQuests.Count)];
                Debug.Log($"📜 NPC ({NPCname})가 퀘스트 제공: {assignedQuest.Title} - {assignedQuest.Description}");
            }
            else
            {
                Debug.Log($"❌ {NPCname}에게 할당된 퀘스트가 없습니다.");
            }
        }
    }
}
