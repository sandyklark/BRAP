using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

namespace Gameplay
{
    public class BreakableTracker : MonoBehaviour
    {
        public int PercentBroken => (int)decimal.Round((decimal)((float)_currentBroken / _breakables.Count * 100));

        public TextMeshProUGUI percentText;

        private List<Breakable> _breakables;
        private int _currentBroken;

        private void Awake()
        {
            _breakables = FindObjectsOfType<Breakable>().ToList();

            _breakables.ForEach(b => b.Broken += OnBreak);
        }

        private void OnBreak()
        {
            _currentBroken++;
        }

        private void Update()
        {
            if (percentText)
            {
                percentText.text = $"Destruction: {PercentBroken}%";
            }
        }
    }
}
