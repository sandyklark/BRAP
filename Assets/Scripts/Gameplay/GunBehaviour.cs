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

        private void Awake()
        {
            // _slidePos = slide.localPosition;
            _rigidbody = GetComponent<Rigidbody2D>();
            _layerMask = LayerMask.GetMask("Default", "Surfaces");

            _rigidbody.constraints = RigidbodyConstraints2D.FreezeAll;
        }

        private void Update()
        {
            SetTimeScale();

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

        private void SetTimeScale()
        {
            var target = 1f;

            if (Input.GetKey(KeyCode.Space))
            {
                target = 0.2f;
            }

            if (Input.GetKeyUp(KeyCode.Space))
            {
                target = 1f;
                _shouldFire = true;
            }

            Time.timeScale = Mathf.Lerp(Time.timeScale, target, Time.fixedUnscaledDeltaTime * 0.7f);
        }

        private void FixedUpdate()
        {
            if (!_shouldFire) return;

            _shouldFire = false;

            if (_isLaunched)
            {
                Fire();
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

        private void Fire()
        {
            // raycast
            var position = barrelPoint.position;
            var hit = Physics2D.Raycast(position, barrelPoint.right, 100f, _layerMask);
            // particles
            ApplyParticleEffects(hit);
            // line
            ApplyLineEffects(position, hit);
            // physics
            ApplyPhysics(hit);
            // slide
            // var pos = slide.localPosition;
            // pos.x += 0.5f;
            // slide.localPosition = pos;
            // time
            _lastFiredTime = Time.realtimeSinceStartup;
            // camera shake
            CameraShake.instance.Shake(2);
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
            line.positionCount = 2;
            line.SetPositions(new Vector3[] { position, hit.point });
            line.startWidth = 0.1f;
        }

        private void ApplyPhysics(RaycastHit2D hit)
        {
            if (hit.collider.attachedRigidbody != null)
            {
                hit.collider.attachedRigidbody.AddForceAtPosition(
                    barrelPoint.right * HitForce,
                    hit.point,
                    ForceMode2D.Impulse
                );

                if (hit.collider.transform.parent &&
                    hit.collider.transform.parent.TryGetComponent<Breakable>(out var breakable))
                {
                    breakable.Break(barrelPoint.right * HitForce);
                }
            }

            _rigidbody.AddForceAtPosition(
                -barrelPoint.right * KickForce,
                barrelPoint.position,
                ForceMode2D.Impulse
            );
        }
    }
}
