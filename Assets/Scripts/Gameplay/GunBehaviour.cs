using System;
using System.Collections.Generic;
using Effects;
using TMPro;
using UnityEngine;
using Util;

namespace Gameplay
{
    public class GunBehaviour : MonoBehaviour
    {
        [Header("Config")]
        public float recoilAdjustStrength = 1f;
        public float recoilRecoveryStrength = 1f;

        [Header("References")]
        public LineRenderer line;
        public ParticleSystem smoke;
        public ParticleSystem flash;
        public ParticleSystem sparks;
        public ParticleSystem shells;
        public Transform barrelPoint;

        private int _layerMask;
        private bool _shouldFire;
        private bool _isLaunched;
        private int _reflectionCount;
        private float _lastFiredTime;
        private Rigidbody2D _rigidbody;
        private float _currentRecoilAdjust;
        private List<Vector3> _linePositions = new();

        private float _lastShotDistance;
        private float _lastShotReflectedDistance;

        private const float HitForce = 20f;
        private const float KickForce = 20f;
        private const float SmokeDurationSeconds = 0.5f;

        public void Fire()
        {
            _shouldFire = true;
        }

        public void ResetLaunch()
        {
            _isLaunched = false;
        }

        public void SlowTime()
        {
            TimeControl.Instance.SetTarget(0.2f);
        }

        public void ResumeTime()
        {
            TimeControl.Instance.SetTarget(1f);
        }

        private void Awake()
        {
            // _slidePos = slide.localPosition;
            _rigidbody = GetComponent<Rigidbody2D>();
            _layerMask = LayerMask.GetMask("Default", "Surfaces");

            _rigidbody.constraints = RigidbodyConstraints2D.FreezeAll;
        }

        private void Update()
        {
            HandleInput();

            // thin shoot line
            line.startWidth = Mathf.Lerp(line.startWidth, 0f, Time.deltaTime * 50f);
            // recover recoil
            _currentRecoilAdjust = Mathf.Lerp(_currentRecoilAdjust, 0f, Time.deltaTime * recoilRecoveryStrength);

            // smoke emission
            var emission = smoke.emission;
            if (_lastFiredTime > 0 && Time.realtimeSinceStartup - _lastFiredTime < SmokeDurationSeconds)
            {
                emission.rateOverTime = 20;
            }
            else
            {
                emission.rateOverTime = 0;
            }

        }

        private void HandleInput()
        {
            if (Input.GetKeyDown(KeyCode.LeftControl))
            {
                Launch();
            }

            if (Input.GetKey(KeyCode.Space))
            {
                SlowTime();
            }

            if (Input.GetKeyUp(KeyCode.Space))
            {
                ResumeTime();
                Fire();
            }

        }

        private void FixedUpdate()
        {
            if (!_shouldFire) return;

            _shouldFire = false;
            _reflectionCount = 0;
            _lastShotDistance = _lastShotReflectedDistance = 0f;
            _linePositions = new List<Vector3>();

            if (_isLaunched)
            {
                InternalFire();

                var dist = decimal.Round((decimal)(_lastShotDistance + _lastShotReflectedDistance), 2);
                var initial = decimal.Round((decimal)_lastShotDistance, 2);
                var deflected = decimal.Round((decimal)_lastShotReflectedDistance, 2);
                Debug.Log($"Last shot was {dist}m - {initial}m Initial shot + {deflected}m ReflectedShots");
            }
            else
            {
                Launch();
            }
        }

        private void Launch()
        {
            _isLaunched = true;
            _rigidbody.constraints = RigidbodyConstraints2D.None;

            _rigidbody.AddForce(
                Vector3.up * KickForce * 0.7f,
                ForceMode2D.Impulse
            );

            _rigidbody.AddTorque(
                3f,
                ForceMode2D.Impulse
            );

            sparks.transform.position = transform.position + Vector3.up;
            sparks.Emit(5);
        }

        private void InternalFire(Ray? reflectionRay = null)
        {
            // raycast
            var right = barrelPoint.right;
            var position = reflectionRay?.origin ?? barrelPoint.position;
            var direction = reflectionRay?.direction ?? right;
            var hit = Physics2D.Raycast(position, direction, 100f, _layerMask);

            _linePositions.AddRange(new List<Vector3> {position, hit.point});
            // particles
            ApplyParticleEffects(hit);
            // line
            ApplyLineEffects(position, hit);
            // physics
            ApplyPhysics(hit, direction);
            // time
            _lastFiredTime = Time.realtimeSinceStartup;
            // camera shake
            if(_reflectionCount == 0) CameraShake.Instance.Shake(2);

            if (hit.collider.TryGetComponent<Reflectable>(out var r))
            {
                _reflectionCount++;
                if (_reflectionCount > 15) return;

                var reflection = Vector3.Reflect(direction, hit.normal);
                InternalFire(new Ray((Vector3)hit.point + reflection * 0.01f, reflection));
            }


            if (reflectionRay == null)
            {
                _lastShotDistance += Vector3.Distance(position, hit.point);
                AdjustRecoil();
            }
            else
            {
                _lastShotReflectedDistance += Vector3.Distance(position, hit.point);
            }
        }

        private void AdjustRecoil()
        {
            _currentRecoilAdjust += recoilAdjustStrength;
            if (_currentRecoilAdjust > 1f) _currentRecoilAdjust = 1f;
        }

        private void ApplyParticleEffects(RaycastHit2D hit)
        {
            smoke.Emit(8);
            flash.Emit(3);
            sparks.transform.position = hit.point;
            sparks.Emit(20);
            shells.Emit(1);
        }


        private void ApplyLineEffects(Vector3 position, RaycastHit2D hit)
        {
            line.positionCount = _linePositions.Count;
            line.SetPositions(_linePositions.ToArray());
            line.startWidth = 0.1f;
        }

        private void ApplyPhysics(RaycastHit2D hit, Vector3 direction)
        {
            if (hit.collider.attachedRigidbody != null)
            {
                hit.collider.attachedRigidbody.AddForceAtPosition(
                    direction * HitForce,
                    hit.point,
                    ForceMode2D.Impulse
                );

                // shatter
                if (hit.collider.TryGetComponent<Shatterable>(out var shatterable))
                {
                    shatterable.Shatter(direction * HitForce);
                }

                // break
                if (hit.collider.transform.parent &&
                    hit.collider.transform.parent.TryGetComponent<Breakable>(out var breakable))
                {
                    breakable.Break(direction * HitForce);
                }
            }

            if (_reflectionCount > 0) return;

            var recoil = -direction * KickForce;
            var recoilAdjusted = recoil * (1f - _currentRecoilAdjust);

            _rigidbody.AddForceAtPosition(
                 recoilAdjusted,
                barrelPoint.position,
                ForceMode2D.Impulse
            );
        }
    }
}
