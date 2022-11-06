using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.Quest
{
    [RequireComponent(typeof(Quest))]
    public class BreakablesQuestTrigger : MonoBehaviour
    {
        public List<Breakable> breakables;

        private Quest _quest;
        private int _totalCount;
        private int _currentCount;

        private void Awake()
        {
            _quest = GetComponent<Quest>();
            _totalCount = breakables.Count;
            breakables.ForEach(b => b.Broken += OnBreak);
        }

        private void OnBreak()
        {
            _currentCount++;
            if(_currentCount == _totalCount) _quest.Trigger();
        }
    }
}
