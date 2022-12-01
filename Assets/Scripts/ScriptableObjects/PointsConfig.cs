using System;
using UnityEngine;

namespace ScriptableObjects
{
    [Serializable]
    public struct PointsItem
    {
        public string name;
        public Color color;
        public int value;
    }

    [CreateAssetMenu(fileName = "PointsConfig", menuName = "ScriptableObjects/PointsConfig")]
    public class PointsConfig : ScriptableObject
    {
        public PointsItem flip;
        public PointsItem tipTap;
        public PointsItem tipStall;
        public PointsItem onTarget;
        public PointsItem reflect;
        public PointsItem proximity;
        public PointsItem pointBlank;
        public PointsItem hover;
    }
}
