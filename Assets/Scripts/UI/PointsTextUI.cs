using System;
using Gameplay;
using TMPro;
using UnityEngine;

namespace UI
{
    public class PointsTextUI : MonoBehaviour
    {
        public TextMeshProUGUI pointsText;
        public PointsTracker pointsTracker;

        private int _currentPoints;
        private int _points;
        private const float CountUpSpeed = 0.005f;
        private const float TextScaleSpeed = 20f;

        private void Awake()
        {
            pointsTracker.PointsAdded += OnPointsAdded;
        }

        private void OnPointsAdded()
        {
            _points = pointsTracker.Points;

            pointsText.transform.localScale = Vector3.one * 1.5f;
        }

        private void Update()
        {
            pointsText.transform.localScale = Vector3.Lerp(
                pointsText.transform.localScale,
                Vector3.one,
                Time.deltaTime * TextScaleSpeed
            );
            _currentPoints = Mathf.CeilToInt(Mathf.Lerp(_currentPoints, _points, Time.deltaTime * CountUpSpeed));
            pointsText.text = $"Points: {_currentPoints}";
        }

    }
}
