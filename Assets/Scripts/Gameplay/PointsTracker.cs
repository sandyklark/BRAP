using System;
using Effects;
using Gun;
using ScriptableObjects;
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

        private void Awake()
        {
            gunSpawn.Spawned += gun =>
            {
                _hasGun = true;
                _gun = gun;
                _gun.ShotFired += OnShotFired;
                _gun.HitSurface += OnHitSurface;
                _gun.Reflection += OnReflection;
                _gun.TipTap += OnTipTap;
            };
        }

        private void OnTipTap()
        {
            ShowPoints(config.tipTap);
        }

        private void OnReflection(Vector3 reflectionPoint)
        {
            ShowPointsPosition(config.reflect, reflectionPoint);
        }

        private void OnHitSurface()
        {
            _currentRotation = 0;
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

        private void Update()
        {
            if (!_hasGun) return;

            TrackFlips();
        }

        private Quaternion _lastRotation = Quaternion.identity;
        private float _currentRotation;
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