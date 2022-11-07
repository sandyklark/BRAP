using System.Collections.Generic;
using Gameplay.Quest;
using UnityEngine;

namespace UI
{
    public class QuestPanelUI : MonoBehaviour
    {
        public QuestHub questHub;
        public GameObject questItemPrefab;
        public Transform questContainer;

        private readonly List<QuestItem> _questItems = new();

        private void Awake()
        {
            questHub.QuestCompleted += OnQuestCompleted;
        }

        private void Start()
        {
            for (var i = 0; i < questHub.quests.Count; i++)
            {
                _questItems.Add(Instantiate(questItemPrefab, questContainer).GetComponent<QuestItem>());
            }

            UpdateQuestItems();
        }

        private void UpdateQuestItems()
        {
            for (var i = 0; i < questHub.quests.Count; i++)
            {
                var q = questHub.quests[i];
                _questItems[i].SetDescription(q.Description);
                _questItems[i].SetComplete(q.IsComplete);
            }
        }

        private void OnQuestCompleted(Quest quest)
        {
            UpdateQuestItems();
        }
    }
}
