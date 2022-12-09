using System;
using UnityEngine;
using Util;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "NewLevel", menuName = "ScriptableObjects/LevelDefinition")]
    [Serializable]
    public class LevelDefinition : ScriptableObject
    {
        [ScriptableObjectId]
        public string id;
        public string title;
        public string description;
        public string sceneName;
        public Sprite thumbnail;
        public float timeLimitSeconds;
        public int ammoLimit;
    }
}
