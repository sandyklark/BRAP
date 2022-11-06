using System;
using Gameplay;
using UI;
using UnityEngine;

namespace Touch
{
    public class GameplayTouchInput : MonoBehaviour
    {
        public PauseMenu pauseMenu;
        public GunBehaviour gun;

        private Vector3 _lastPointer;

        private void Update()
        {
            if (gun == null) return;

            if (Input.touches.Length == 0) return;

            var touch = Input.touches[0];
            switch (touch.phase)
            {
                case TouchPhase.Began:
                    HandleTouch(touch);
                    break;
                case TouchPhase.Ended:
                    HandleRelease(touch);
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
                pauseMenu.SetPause(true);
            }
        }

        private void HandleTouch(UnityEngine.Touch touch)
        {
            _lastPointer = touch.position;
            gun.SlowTime();
        }

        private void HandleRelease(UnityEngine.Touch touch)
        {
            if (touch.position.y > _lastPointer.y + Screen.height * 0.25f) gun.ResetLaunch();
            gun.ResumeTime();
            gun.Fire();
        }
    }
}
