using System;
using Gameplay;
using TMPro;
using UnityEngine;

namespace UI
{
    public class TimerTextUI : MonoBehaviour
    {
        public TextMeshProUGUI timerText;
        public GameplayController gameplay;

        private void Update()
        {
            var timeSpan = TimeSpan.FromSeconds(gameplay.RemainingTimeSeconds);
            var timeText = $"{timeSpan.Minutes:D2}:{timeSpan.Seconds:D2}";
            timerText.text = timeText;
        }
    }
}
