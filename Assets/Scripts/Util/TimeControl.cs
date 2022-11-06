using System;
using UnityEngine;

namespace Util
{
    public class TimeControl : MonoBehaviour
    {
        public static TimeControl Instance;

        private bool _paused;
        private float _target = 1f;

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
        }

        private void Update()
        {
            if (_paused)
            {
                Time.timeScale = 0f;
                return;
            }

            // blend time
            Time.timeScale = Mathf.Lerp(Time.timeScale, _target, Time.unscaledDeltaTime * 2f);
        }

        public void SetPause(bool paused)
        {
            _paused = paused;
        }

        public void SetTarget(float target)
        {
            _target = target;
        }
    }
}
