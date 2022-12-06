using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;

namespace Player
{
    public static class PlayerData
    {
        private const string SaveFileName = "T2B_SaveData.json";
        private static string SaveFilePath => Path.Join(Application.persistentDataPath, SaveFileName);

        public static GameProgress Load()
        {
            if (File.Exists(SaveFilePath))
            {
                var json = File.ReadAllText(SaveFilePath);
                return JsonConvert.DeserializeObject<GameProgress>(json);
            }

            Debug.LogWarning("File not found: " + SaveFilePath);
            return new GameProgress
            {
                unlocked = new List<string>()
            };
        }

        public static void Save(GameProgress gameProgress)
        {
            File.WriteAllText(SaveFilePath, JsonConvert.SerializeObject(gameProgress, Formatting.Indented));
        }
    }
}
