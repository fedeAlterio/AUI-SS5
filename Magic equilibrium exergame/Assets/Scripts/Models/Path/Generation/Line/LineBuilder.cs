using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Models.Path.Generation.Line
{
    public class LineBuilder : ILineBuilder
    {
        // Private fields
        private readonly Vector3 _startPosition;
        private readonly Vector3 _startDirection;
        private readonly List<Vector3> _relativeDeltaPositions = new List<Vector3>();



        // initialization
        protected LineBuilder(Vector3 startPosition, Vector3 startDirection, float curveWidth)
        {
            _startPosition = startPosition;
            _startDirection = startDirection.normalized;
            CurveSize = curveWidth;
        }


        public static ILineBuilder NewLine(Vector3 startPosition, Vector3 startDirection, float curveWidth)
        {
            var ret = new LineBuilder(startPosition, startDirection, curveWidth);
            return ret;
        }



        // Properties
        public Vector3 LastPoint => _relativeDeltaPositions[_relativeDeltaPositions.Count - 1];
        public int SegmentVertexCount { get; set; } = 3;
        public int CurveVertexCount { get; set; } = 10;
        public float CurveSize { get; set; } = 3;



        // Line Builder
        public ILineBuilder MoveOf(Vector3 nextPointRelativePosition)
        {
            _relativeDeltaPositions.Add(nextPointRelativePosition);
            return this;
        }

        public IEnumerable<DiscreteCurve> Build()
        {
            var currentPosition = _startPosition;
            var currentDirection = _startDirection;
            foreach (var relativeDeltaPos in _relativeDeltaPositions)
            {
                var deltaPos = ToPathTangentCoordinates(currentDirection, relativeDeltaPos);
                var bezierMiddle = currentPosition + currentDirection * CurveSize;
                var bezierEnd = bezierMiddle + deltaPos.normalized * CurveSize;
                var nextPoint = bezierEnd + deltaPos;

                // Creating continuous lines
                var bezier = new QuadraticBezier(currentPosition, bezierMiddle, bezierEnd);
                var forwardLine = Curves.Line(bezierEnd, nextPoint);


                // Creating discrete lines
                var discreteBezier = new DiscreteCurve(bezier) { VertexCount = CurveVertexCount };
                yield return discreteBezier;

                var discreteForwardLine = new DiscreteCurve(forwardLine) { VertexCount = SegmentVertexCount };
                yield return discreteForwardLine;


                // Next point setup
                currentPosition = nextPoint;
                currentDirection = deltaPos.normalized;
            }
        }


        // Utils
        private Vector3 ToPathTangentCoordinates(Vector3 pathTangentDirection, Vector3 direction)
        {
            var z = pathTangentDirection.normalized;
            var x = Vector3.Cross(Vector3.up, z);
            var y = Vector3.Cross(z, x);

            return direction.x * x + direction.y * y + direction.z * z;
        }
    }
}
