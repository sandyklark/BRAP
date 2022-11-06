using System;
using UnityEngine;

namespace Gameplay
{
    public class Shatterer : MonoBehaviour
    {
        public Action<Shatterable> Shattered;

        private void OnCollisionEnter2D(Collision2D col)
        {
            if (col.transform.TryGetComponent<Shatterable>(out var s))
            {
                s.Shatter(col.relativeVelocity);
                Shattered?.Invoke(s);
            }
        }
    }
}
