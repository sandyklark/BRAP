using System;
using UnityEngine;

namespace Gun
{
    public class GunTip : MonoBehaviour
    {
        public Action TipTap;

        private void OnCollisionEnter2D(Collision2D col)
        {
            TipTap?.Invoke();
        }
    }
}
