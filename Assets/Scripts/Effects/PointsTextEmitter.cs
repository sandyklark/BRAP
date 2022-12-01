using System.Collections.Generic;
using UnityEngine;

namespace Effects
{
    public class PointsTextEmitter : MonoBehaviour
    {
        public PointsText pointsTextPrefab;
        public ParticleSystem burstParticles;

        public static PointsTextEmitter Instance;

        private int _poolSize = 15;
        private List<PointsText> _pointsTextPool;

        public void ShowText(string text, Vector3 position, Color color)
        {
            if (_pointsTextPool.Count == 0)
            {
                Debug.Log("Pool empty");
                return;
            }

            var pointsText = _pointsTextPool[0];

            pointsText.SetText(text);
            pointsText.SetColor(color);
            pointsText.transform.position = position;
            pointsText.gameObject.SetActive(true);

            _pointsTextPool.RemoveAt(0);
        }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(this);
            }

            _pointsTextPool = new List<PointsText>();

            for (var i = 0; i < _poolSize; i++)
            {
                var pointsText = Instantiate(pointsTextPrefab, transform);
                pointsText.gameObject.SetActive(false);
                pointsText.Burst += HandleBurst;
                _pointsTextPool.Add(pointsText);
            }
        }

        private void HandleBurst(PointsText pointsText, Color color)
        {
            burstParticles.transform.position = pointsText.transform.position;
            var main = burstParticles.main;
            main.startColor = color;
            burstParticles.Emit(10);

            _pointsTextPool.Add(pointsText);
            pointsText.gameObject.SetActive(false);
        }
    }
}
