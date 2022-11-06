using System;
using UnityEngine;

namespace Gameplay.Quest
{
    public class Quest : MonoBehaviour
    {
        public Action<Quest> Complete;
        public bool IsComplete { get; private set; }
        public string Description => !secretUntilComplete || IsComplete ? description : "???";

        public bool secretUntilComplete;
        public string description;

        public void Trigger()
        {
            IsComplete = true;
            Complete?.Invoke(this);
        }
    }
}
