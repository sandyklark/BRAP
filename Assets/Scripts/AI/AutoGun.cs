using System;
using Gameplay;
using Gun;
using UnityEngine;

namespace AI
{
    [RequireComponent(typeof(GunBehaviour))]
    public class AutoGun : MonoBehaviour
    {
        private GunBehaviour _gun;
        private float _nextTimeoutTime;

        private const float TimeoutIntervalSeconds = 2f;

        private void Awake()
        {
            _gun = GetComponent<GunBehaviour>();
            _nextTimeoutTime = Time.fixedTime + 3;
        }

        private void FixedUpdate()
        {
            // check timeout
            if (Time.fixedTime > _nextTimeoutTime)
            {
                // shoot if timeout reached
                _gun.Fire();
                _nextTimeoutTime = Time.fixedTime + TimeoutIntervalSeconds;
            }

            // fire a ray to look for stuff
            var gunDirection = _gun.barrelPoint.right;
            var hit = Physics2D.Raycast(_gun.barrelPoint.position, gunDirection);

            if (!hit.collider) return;
            // hit something
            // check if thing is shootable
            var parent = hit.collider.transform.parent;
            var isBreakable = parent.TryGetComponent<Breakable>(out var b);
            var isShatterable = parent.TryGetComponent<Shatterable>(out var s);

            if (!isBreakable && !isShatterable) return;

            // shoot at it
            _gun.Fire();
            // reset timeout
            _nextTimeoutTime = Time.fixedTime + TimeoutIntervalSeconds;
        }

        private void OnCollisionExit2D(Collision2D other)
        {
            var nextInterval = Time.fixedTime + TimeoutIntervalSeconds * 0.25f;

            if (_nextTimeoutTime < Time.fixedTime + TimeoutIntervalSeconds * 0.25f) return;

            _nextTimeoutTime = nextInterval;
        }
    }
}
