using System;
using Gameplay;
using UnityEngine;

namespace Touch
{
    public class GameplayTouchInput : MonoBehaviour
    {
        public SceneControl sceneControl;
        public GunBehaviour gun;

        private void Update()
        {
            if (gun == null) return;

            if (Input.touches.Length == 0) return;

            var touch = Input.touches[0];
            switch (touch.phase)
            {
                case TouchPhase.Began:
                    gun.SlowTime();
                    break;
                case TouchPhase.Ended:
                    gun.ResumeTime();
                    gun.Fire();
                    break;
                case TouchPhase.Moved:
                    break;
                case TouchPhase.Stationary:
                    break;
                case TouchPhase.Canceled:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            if (Input.touches.Length > 1)
            {
                sceneControl.Reload();
            }
        }
    }
}
