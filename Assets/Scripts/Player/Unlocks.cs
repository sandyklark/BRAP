using System;

namespace Player
{
    public static class Unlocks
    {
        public static void Unlock(string id)
        {
            if (Check(id)) return;

            PlayerData.Progress.unlocked.Add(id);
        }

        public static bool Check(string id)
        {
            return PlayerData.Progress.unlocked.Contains(id);
        }
    }
}
