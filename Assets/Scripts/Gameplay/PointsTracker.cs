using System;
using Effects;
using Gun;
using ScriptableObjects;
using UnityEditor.Rendering;
using UnityEngine;

namespace Gameplay
{
    public class PointsTracker : MonoBehaviour
    {
        public Action PointsAdded;

        public int Points { get; private set; }

        public PointsConfig config;
        public GunSpawn gunSpawn;

        private GunBehaviour _gun;
        private bool _hasGun;

        private Quaternion _lastRotation = Quaternion.identity;
        private float _currentRotation;
        private float _lastTouchTime;

        private bool _firstLaunch = true;
        private bool _countingBigAir;
        private const float BigAirThresholdSeconds = 2f;

        private void Awake()
        {
            gunSpawn.Spawned += gun =>
            {
                _hasGun = true;
                _gun = gun;
                _gun.ShotFired += OnShotFired;
                _gun.HitSurface += OnHitSurface;
                _gun.LeftSurface += OnLeftSurface;
                _gun.Reflection += OnReflection;
                _gun.TipTap += OnTipTap;
                _gun.Launched += OnLaunched;
            };
        }

        private void OnLaunched()
        {
            if (!_firstLaunch) return;

            ResetBigAir();
            _firstLaunch = false;
        }

        private void Update()
        {
            if (!_hasGun) return;

            TrackFlips();
            TrackAirTime();
        }

        private void OnReflection(Vector3 reflectionPoint)
        {
            ShowPointsPosition(config.reflect, reflectionPoint);
        }

        private void OnTipTap()
        {
            ResetBigAir();
            ShowPoints(config.tipTap);
        }

        private void OnHitSurface()
        {
            _countingBigAir = false;
            _currentRotation = 0;
        }

        private void OnLeftSurface()
        {
            ResetBigAir();
        }

        private void ResetBigAir()
        {
            _lastTouchTime = Time.time;
            _countingBigAir = true;
        }

        private void TrackAirTime()
        {
            if (Time.time - _lastTouchTime < BigAirThresholdSeconds || !_countingBigAir) return;

            _lastTouchTime = Time.time - BigAirThresholdSeconds / 2;

            ShowPoints(config.bigAir);
        }

        private void OnShotFired(ShotInfo shot)
        {
            _currentRotation = 0;

            if (shot.hitTarget)
            {
                ShowPointsPosition(config.onTarget, shot.hitPoint);

            }

            if (shot.shotDistance < 0.5f)
            {
                ShowPoints(shot.hitTarget ? config.pointBlank : config.proximity);
            }

        }

        private void TrackFlips()
        {
            var rotation = _gun.transform.rotation;
            var angle = Quaternion.Angle(_lastRotation, rotation);
            _lastRotation = rotation;

            _currentRotation += angle;

            if (_currentRotation >= 360)
            {
                ShowPoints(config.flip);
                _currentRotation = 0;
            }
        }

        private void ShowPoints(PointsItem item)
        {
            ShowPointsPosition(item, _gun.transform.position);
        }

        private void ShowPointsPosition(PointsItem item, Vector3 position)
        {
            AddPoints(item.value);

            PointsTextEmitter.Instance.ShowText(
                $"+{item.value} {item.name}",
                position,
                item.color
            );
        }

        private void AddPoints(int amount)
        {
            Points += amount;

            PointsAdded?.Invoke();
        }
    }
}
