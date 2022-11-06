using UnityEngine;

namespace Gameplay.Quest
{
    [RequireComponent(typeof(Quest))]
    public class SpecificShatterQuestTrigger : MonoBehaviour
    {
        public Shatterer shatterer;
        public Shatterable shatterable;

        private Quest _quest;

        private void Awake()
        {
            _quest = GetComponent<Quest>();
            shatterer.Shattered += OnShatter;
        }

        private void OnShatter(Shatterable s)
        {
            if(s == shatterable) _quest.Trigger();
        }
    }
}
