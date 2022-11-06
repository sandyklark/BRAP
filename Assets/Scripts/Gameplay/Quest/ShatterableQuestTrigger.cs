using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.Quest
{
    [RequireComponent(typeof(Quest))]
    public class ShatterableQuestTrigger : MonoBehaviour
    {
        public List<Shatterable> shatterables;

        private Quest _quest;
        private int _totalCount;
        private int _currentCount;

        private void Awake()
        {
            _quest = GetComponent<Quest>();
            _totalCount = shatterables.Count;
            shatterables.ForEach(s => s.Shattered += OnShatter);
        }

        private void OnShatter()
        {
            _currentCount++;
            if(_currentCount == _totalCount) _quest.Trigger();
        }
    }
}
