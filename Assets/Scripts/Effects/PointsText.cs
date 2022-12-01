using System;
using TMPro;
using UnityEngine;

namespace Effects
{
    public class PointsText : MonoBehaviour
    {
        public Action<PointsText, Color> Burst;

        private TextMeshPro _text;

        private void Awake()
        {
            _text = GetComponent<TextMeshPro>();
        }

        public void TriggerBurst()
        {
            Burst?.Invoke(this, _text.color);
        }

        public void SetText(string text)
        {
            _text.text = text;
        }

        public void SetColor(Color color)
        {
            _text.color = color;
        }

        private void Update()
        {
            transform.Translate(Vector3.up * Time.deltaTime);
        }
    }
}
