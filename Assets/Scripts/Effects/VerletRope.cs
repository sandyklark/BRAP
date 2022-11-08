using System.Collections.Generic;
using UnityEngine;

namespace Effects
{
    public class VerletRope : MonoBehaviour
    {
        public int segmentCount = 10;
        public float ropeSegmentLength = 0.05f;
        public Transform endTarget;

        private LineRenderer _lineRenderer;
        private readonly List<RopeSegment> _ropeSegments = new List<RopeSegment>();

        private Vector2 _anchorPos;

        // Use this for initialization
        private void Start()
        {
            _lineRenderer = GetComponent<LineRenderer>();
            var ropeStartPoint = transform.position;

            for (var i = 0; i < segmentCount; i++)
            {
                _ropeSegments.Add(new RopeSegment(ropeStartPoint));
                ropeStartPoint.y -= ropeSegmentLength;
            }

            if (endTarget)
            {
                ropeSegmentLength = Vector3.Distance(transform.position, endTarget.position) / segmentCount;
            }
        }

        // Update is called once per frame
        private void Update()
        {
            _anchorPos = transform.position;
            Simulate();
            DrawRope();
        }

        private void Simulate()
        {
            // SIMULATION
            var forceGravity = new Vector2(0f, 0f);

            for (var i = 1; i < segmentCount; i++)
            {
                var firstSegment = _ropeSegments[i];
                var velocity = firstSegment.posNow - firstSegment.posOld;
                firstSegment.posOld = firstSegment.posNow;
                firstSegment.posNow += velocity;
                firstSegment.posNow += forceGravity * Time.fixedDeltaTime;
                firstSegment.posNow = Vector3.Lerp(firstSegment.posNow, endTarget.position, (float)i / segmentCount);
                _ropeSegments[i] = firstSegment;
            }

            if (endTarget)
            {
                var last = _ropeSegments[^1];
                last.posNow = endTarget.position;
                _ropeSegments[^1] = last;
            }

            //CONSTRAINTS
            for (var i = 0; i < 50; i++)
            {
                ApplyConstraint();
            }
        }

        private void ApplyConstraint()
        {
            //Constrant to Mouse
            var firstSegment = _ropeSegments[0];
            firstSegment.posNow = _anchorPos;
            _ropeSegments[0] = firstSegment;

            for (var i = 0; i < segmentCount - 1; i++)
            {
                var firstSeg = _ropeSegments[i];
                var secondSeg = _ropeSegments[i + 1];

                var dist = (firstSeg.posNow - secondSeg.posNow).magnitude;
                var error = Mathf.Abs(dist - ropeSegmentLength);
                var changeDir = Vector2.zero;

                if (dist > ropeSegmentLength)
                {
                    changeDir = (firstSeg.posNow - secondSeg.posNow).normalized;
                } else if (dist < ropeSegmentLength)
                {
                    changeDir = (secondSeg.posNow - firstSeg.posNow).normalized;
                }

                var changeAmount = changeDir * error;
                if (i != 0)
                {
                    firstSeg.posNow -= changeAmount * 0.5f;
                    _ropeSegments[i] = firstSeg;
                    secondSeg.posNow += changeAmount * 0.5f;
                    _ropeSegments[i + 1] = secondSeg;
                }
                else
                {
                    secondSeg.posNow += changeAmount;
                    _ropeSegments[i + 1] = secondSeg;
                }
            }
        }

        private void DrawRope()
        {
            var ropePositions = new Vector3[segmentCount];
            for (var i = 0; i < segmentCount; i++)
            {
                ropePositions[i] = _ropeSegments[i].posNow;
            }

            _lineRenderer.positionCount = ropePositions.Length;
            _lineRenderer.SetPositions(ropePositions);
        }

        private struct RopeSegment
        {
            public Vector2 posNow;
            public Vector2 posOld;

            public RopeSegment(Vector2 pos)
            {
                posNow = pos;
                posOld = pos;
            }
        }
    }
}
