using System;
using System.Collections.Generic;

namespace Player
{
    [Serializable]
    public struct EnergyData
    {
        public int amount;
        public int max;
        public int extra;
    }

    [Serializable]
    public struct TokenData
    {
        public int gold;
        public int gunTokens;
    }

    [Serializable]
    public struct Progress
    {
        public List<string> unlocked;
        public TokenData tokenData;
        public EnergyData energyData;
    }
}
