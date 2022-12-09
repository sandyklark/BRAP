using System;
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

        public static Progress Progress { get; private set; }

        public static void Load()
        {
            if (File.Exists(SaveFilePath))
            {
                var json = File.ReadAllText(SaveFilePath);
                Progress = JsonConvert.DeserializeObject<Progress>(json);
                return;
            }

            Debug.LogWarning("File not found: " + SaveFilePath);
            Progress = new Progress
            {
                unlocked = new List<string>()
            };
        }

        public static void Save()
        {
            File.WriteAllText(SaveFilePath, JsonConvert.SerializeObject(Progress, Formatting.Indented));
        }
    }
}
