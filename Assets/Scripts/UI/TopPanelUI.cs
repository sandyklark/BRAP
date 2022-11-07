using Gameplay.Quest;
using UnityEngine;

namespace UI
{
    public class TopPanelUI : MonoBehaviour
    {
        public QuestHub questHub;
        public QuestItem questItem;

        private Animator _animator;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            questHub.QuestCompleted += OnQuestCompleted;
        }

        private void OnQuestCompleted(Quest quest)
        {
            questItem.descriptionText.text = quest.Description;
            _animator.Play("QuestToast");
        }
    }
}
