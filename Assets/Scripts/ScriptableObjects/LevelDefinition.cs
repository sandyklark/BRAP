using System;
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
    }
}
