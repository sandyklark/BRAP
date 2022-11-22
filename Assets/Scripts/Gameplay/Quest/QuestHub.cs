using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Gameplay.Quest
{
    public class QuestHub : MonoBehaviour
    {
        public Action<Quest> QuestCompleted;

        public List<Quest> Quests { get; private set; }

        private void Awake()
        {
            Quests = FindObjectsOfType<Quest>().ToList();
            Quests.ForEach(q => q.Complete += OnQuestComplete);
        }

        private void OnQuestComplete(Quest quest)
        {
            QuestCompleted?.Invoke(quest);
        }
    }
}
