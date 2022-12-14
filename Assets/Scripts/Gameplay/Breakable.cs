using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace Gameplay
{
    public class Breakable : MonoBehaviour
    {
        public Action Broken;

        [FormerlySerializedAs("Health")] [Header("Config")]
        public int health = 1;
        [Header("References")]
        public GameObject main;
        public GameObject insides;

        private SpriteRenderer _sprite;
        private Collider2D _collider;
        private List<Rigidbody2D> _innerRigidbodies;
        private int _currentHealth;

        private void Awake()
        {
            _sprite = GetComponentInChildren<SpriteRenderer>();
            _currentHealth = health;
            _innerRigidbodies = insides.GetComponentsInChildren<Rigidbody2D>().ToList();

            insides.SetActive(false);
        }

        public void Break(Vector2 force)
        {
            _currentHealth--;

            if (_sprite != null) _sprite.color = Color.Lerp(Color.black, Color.white, (float)_currentHealth / health);

            if(_currentHealth > 0) return;

            main.SetActive(false);

            foreach (var innerRigidbody in _innerRigidbodies)
            {
                innerRigidbody.transform.parent = transform.parent;
                innerRigidbody.AddForce(force, ForceMode2D.Impulse);
                innerRigidbody.AddTorque(Random.Range(-1f, 1f) * force.magnitude / 5f, ForceMode2D.Impulse);
            }

            Broken?.Invoke();
        }
    }
}
