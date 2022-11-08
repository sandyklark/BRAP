using TMPro;
using UnityEngine;

namespace Gameplay
{
    public class RayCastFromMouse
    {
        private void FixedUpdate()
        {
            // If we're not clicking, return;
            if (!Input.GetMouseButtonDown(0)) return;

            // if we are clicking
            // convert mouse position to world space
            var mouseWorldPos = Camera.current.ScreenToWorldPoint(Input.mousePosition);
            // construct ray
            var ray = new Ray(mouseWorldPos, Vector3.back);
            // do raycast and see if it hit
            if (Physics.Raycast(ray.origin, ray.direction, out var raycastHit, 10f))
            {
                //
            }
        }
    }
}
