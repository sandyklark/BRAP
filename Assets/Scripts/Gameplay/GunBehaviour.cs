using System;
using System.Collections.Generic;
using Effects;
using UnityEngine;

namespace Gameplay
{
    public class GunBehaviour : MonoBehaviour
    {
        public ParticleSystem smoke;
        public ParticleSystem flash;
        public ParticleSystem sparks;
        public ParticleSystem shells;
        public LineRenderer line;
        // public Transform slide;
        public Transform barrelPoint;

        private int _layerMask = 0;
        private Rigidbody2D _rigidbody;
        private float _lastFiredTime;
        private bool _shouldFire;
        private bool _isLaunched;

        // private Vector3 _slidePos;

        private const float SmokeDurationSeconds = 0.5f;
        private const float HitForce = 20f;
        private const float KickForce = 20f;

        public void Fire()
        {
            _shouldFire = true;
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
            // return slide
            // slide.transform.localPosition = Vector3.Lerp(slide.transform.localPosition, _slidePos, Time.deltaTime * 10f);

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
            var target = 1f;

            if (Input.GetKey(KeyCode.Space))
            {
                target = 0.2f;
            }

            if (Input.GetKeyUp(KeyCode.Space))
            {
                target = 1f;
                Fire();
            }

            Time.timeScale = Mathf.Lerp(Time.timeScale, target, Time.fixedUnscaledDeltaTime * 0.7f);
        }

        private void FixedUpdate()
        {
            if (!_shouldFire) return;

            _reflectionCount = 0;
            _linePositions = new List<Vector3>();
            _shouldFire = false;

            if (_isLaunched)
            {
                InternalFire();
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
            _rigidbody.AddForceAtPosition(
                barrelPoint.up * KickForce * 0.6f,
                barrelPoint.position + -barrelPoint.right * 0.4f,
                ForceMode2D.Impulse
            );

            sparks.transform.position = transform.position + Vector3.up;
            sparks.Emit(5);
        }

        private int _reflectionCount;
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
            // slide
            // var pos = slide.localPosition;
            // pos.x += 0.5f;
            // slide.localPosition = pos;
            // time
            _lastFiredTime = Time.realtimeSinceStartup;
            // camera shake
            if(_reflectionCount == 0) CameraShake.instance.Shake(2);

            if (hit.collider.TryGetComponent<Reflectable>(out var r))
            {
                _reflectionCount++;
                if (_reflectionCount > 15) return;

                var reflection = Vector3.Reflect(direction, hit.normal);
                InternalFire(new Ray((Vector3)hit.point + reflection * 0.01f, reflection));
            }
        }

        private void ApplyParticleEffects(RaycastHit2D hit)
        {
            smoke.Emit(8);
            flash.Emit(3);
            sparks.transform.position = hit.point;
            sparks.Emit(20);
            shells.Emit(1);
        }

        private List<Vector3> _linePositions = new List<Vector3>();
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

            _rigidbody.AddForceAtPosition(
                -direction * KickForce,
                barrelPoint.position,
                ForceMode2D.Impulse
            );
        }
    }
}
