using Assets.Scripts.Models.Path.Generation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Models.Path.Blocks.Line
{
    public class LineBuilder : ILineBuilder
    {
        // Private fields
        private readonly Vector3 _startPosition;
        private readonly Vector3 _startDirection;
        private readonly List<Vector3> _relativeDeltaPositions = new List<Vector3>();
        private readonly float _curveWidth;


        // initialization
        public LineBuilder(Vector3 startPosition, Vector3 startDirection, float curveWidth)
        {
            _startPosition = startPosition;
            _startDirection = startDirection.normalized;
            _curveWidth = curveWidth;
        }


        public static ILineBuilder NewLine(Vector3 startPosition, Vector3 startDirection, float curveWidth)
        {
            var ret = new LineBuilder(startPosition, startDirection, curveWidth);
            return ret;
        }



        // Properties
        public Vector3 LastPoint => _relativeDeltaPositions[_relativeDeltaPositions.Count - 1];



        // Line Builder
        public ILineBuilder MoveOf(Vector3 nextPointRelativePosition)
        {
            _relativeDeltaPositions.Add(nextPointRelativePosition);
            return this;
        }

        public ParametricCurve Build()
        {
            var currentPosition = _startPosition;
            var currentDirection = _startDirection;
            ParametricCurve totalCurve = Curves.Line(currentPosition, currentPosition + currentDirection * 0.01f);
            foreach (var relativeDeltaPos in _relativeDeltaPositions)
            {
                var deltaPos = FromRelativeDirection(currentDirection, relativeDeltaPos);
                var bezierMiddle = currentPosition + currentDirection * _curveWidth;
                var bezierEnd = bezierMiddle + deltaPos.normalized * _curveWidth;
                var bezier = new QuadraticBezier(currentPosition, bezierMiddle, bezierEnd);
                var nextPoint = bezierEnd + deltaPos;
                var forwardCurve = Curves.Line(bezierEnd, nextPoint);
                totalCurve += bezier + forwardCurve;
                
                currentPosition = nextPoint;
                currentDirection = deltaPos.normalized;
            }

            return totalCurve;
        }


        // Utils
        private Vector3 FromRelativeDirection(Vector3 currentDirection, Vector3 nextDirection)
        {
            var z = currentDirection.normalized;
            var x = Vector3.Cross(Vector3.up, z);
            var y = Vector3.Cross(z, x);

            return nextDirection.x* x + nextDirection.y * y + nextDirection.z * z;
        }
    }
}
