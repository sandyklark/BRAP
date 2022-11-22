using Gameplay.Quest;
using UnityEngine;

namespace UI
{
    public class QuestUI : MonoBehaviour
    {
        public Animator questAnimator;
        public QuestHub questHub;
        public QuestItem questItem;

        private void Awake()
        {
            questHub.QuestCompleted += OnQuestCompleted;
        }

        private void OnQuestCompleted(Quest quest)
        {
            questItem.descriptionText.text = quest.Description;
            questAnimator.Play("QuestToast");
        }
    }
}
