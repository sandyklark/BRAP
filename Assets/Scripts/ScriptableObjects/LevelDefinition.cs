using System;
using System.Collections.Generic;
using Gameplay.Quest;
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "NewLevel", menuName = "ScriptableObjects/LevelDefinition")]
    [Serializable]
    public class LevelDefinition : IdentifiableScriptableObject
    {
        public string title;
        public string description;
        public string sceneName;
        public Sprite thumbnail;
        public float timeLimitSeconds;
        public int ammoLimit;
        public List<QuestDefinition> quests;
    }
}
