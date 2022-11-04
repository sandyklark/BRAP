using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Effects
{
    public class VerletRope : MonoBehaviour
    {
        public AnimationCurve tieCurve;

        private LineRenderer _lineRenderer;
        private readonly List<RopeSegment> _ropeSegments = new List<RopeSegment>();
        private const float RopeSegLen = 0.05f;
        private const int SegmentCount = 10;
        private const float LineWidth = 0.1f;

        private Vector2 _anchorPos;

        // Use this for initialization
        private void Start()
        {
            _lineRenderer = GetComponent<LineRenderer>();
            var ropeStartPoint = transform.position;

            for (var i = 0; i < SegmentCount; i++)
            {
                _ropeSegments.Add(new RopeSegment(ropeStartPoint));
                ropeStartPoint.y -= RopeSegLen;
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
            var forceGravity = new Vector2(0f, -0.81f);

            for (var i = 1; i < SegmentCount; i++)
            {
                var firstSegment = _ropeSegments[i];
                var velocity = firstSegment.posNow - firstSegment.posOld;
                firstSegment.posOld = firstSegment.posNow;
                firstSegment.posNow += velocity;
                firstSegment.posNow += forceGravity * Time.fixedDeltaTime;
                _ropeSegments[i] = firstSegment;
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

            for (var i = 0; i < SegmentCount - 1; i++)
            {
                var firstSeg = _ropeSegments[i];
                var secondSeg = _ropeSegments[i + 1];

                var dist = (firstSeg.posNow - secondSeg.posNow).magnitude;
                var error = Mathf.Abs(dist - RopeSegLen);
                var changeDir = Vector2.zero;

                if (dist > RopeSegLen)
                {
                    changeDir = (firstSeg.posNow - secondSeg.posNow).normalized;
                } else if (dist < RopeSegLen)
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
            var width = LineWidth;
            _lineRenderer.startWidth = width;
            _lineRenderer.widthCurve = tieCurve;
            _lineRenderer.endWidth = width;

            var ropePositions = new Vector3[SegmentCount];
            for (var i = 0; i < SegmentCount; i++)
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
