using System;
using ScriptableObjects;
using UnityEngine;

namespace Gameplay.Quest
{
    public class Quest : MonoBehaviour
    {
        public Action<Quest> Complete;

        public QuestDefinition questDefinition;
        public bool IsComplete { get; private set; }
        public string Description => !questDefinition.secretUntilComplete || IsComplete ? questDefinition.description : "???";

        public void Trigger()
        {
            IsComplete = true;
            Complete?.Invoke(this);
        }
    }
}
