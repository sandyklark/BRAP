using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Gameplay
{
    public class Shatterable : MonoBehaviour
    {
        public Action Shattered;

        private List<Breakable> _breakables;
        private List<Rigidbody2D> _childRigidbodies;

        public void Shatter(Vector2 force)
        {
            foreach (var child in _childRigidbodies)
            {
                child.simulated = true;
                child.AddForce(force, ForceMode2D.Impulse);
                child.AddTorque(Random.Range(-1f, 1f) * force.magnitude / 5f, ForceMode2D.Impulse);
            }

            _breakables.ForEach(b =>
            {
                b.transform.parent = transform.parent;
                b.enabled = true;
            });

            Shattered?.Invoke();

            Destroy(gameObject);
        }

        private void Start()
        {
            _breakables = GetComponentsInChildren<Breakable>().ToList();
            _childRigidbodies = _breakables.Select(b => b.GetComponentInChildren<Rigidbody2D>()).ToList();

            _breakables.ForEach(b => b.enabled = false);
            _childRigidbodies.ForEach(r => r.simulated = false);
        }
    }
}
