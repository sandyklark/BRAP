using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Gameplay.Quest
{
    public class QuestHub : MonoBehaviour
    {
        public Action QuestCompleted;

        public List<Quest> quests;

        private void Awake()
        {
            quests = GetComponentsInChildren<Quest>().ToList();
            quests.ForEach(q => q.Complete += OnQuestComplete);
        }

        private void OnQuestComplete(Quest quest)
        {
            QuestCompleted?.Invoke();
        }
    }
}
