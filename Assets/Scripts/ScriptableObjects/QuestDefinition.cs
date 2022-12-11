using System;
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "NewQuest", menuName = "ScriptableObjects/QuestDefinition")]
    [Serializable]
    public class QuestDefinition : IdentifiableScriptableObject
    {
        public bool secretUntilComplete;
        public string description;
    }
}
