using System;
using UnityEditor;
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "NewLevel", menuName = "ScriptableObjects/LevelDefinition")]
    [Serializable]
    public class LevelDefinition : ScriptableObject
    {
        public string title;
        public string description;
        public string sceneName;
        public Sprite thumbnail;
        public float timeLimitSeconds;
        public int ammoLimit;
    }
}
