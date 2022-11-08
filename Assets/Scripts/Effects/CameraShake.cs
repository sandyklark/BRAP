using UnityEngine;

namespace Effects
{
    public class CameraShake : MonoBehaviour
    {
        public static CameraShake Instance;

        public float maximumShake;
        public float shakeIntensity = 1f;
        public float dampen = 0.9f;

        private float _currentShake;
        private Vector3 _initialPosition;

        public void Shake(float amount)
        {
            _currentShake += amount;
            if (_currentShake > maximumShake) _currentShake = maximumShake;
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

            _initialPosition = transform.position;
        }

        private void Update()
        {
            transform.position = _initialPosition + new Vector3(GetRandomShake(), GetRandomShake(), 0f);
            _currentShake *= dampen;
        }

        private float GetRandomShake()
        {
            return (-0.5f + Random.value) * shakeIntensity * _currentShake;
        }
    }
}
