using System.Collections.Generic;
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "NewLevelSet", menuName = "ScriptableObjects/LevelSet")]
    public class LevelSet : ScriptableObject
    {
        public List<LevelDefinition> levels;
    }
}
