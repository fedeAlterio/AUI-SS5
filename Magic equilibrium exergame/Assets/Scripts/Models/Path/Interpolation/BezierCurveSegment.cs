using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Models.Path.Interpolation
{
    public class BezierCurveSegment
    {
        public BezierCurveSegment()
        {

        }

        public BezierCurveSegment(Vector2 startPoint, Vector2 firstControlPoint, Vector2 secondControlPoint, Vector2 endPoint)
        {
            StartPoint = startPoint;
            FirstControlPoint = firstControlPoint;
            SecondControlPoint = secondControlPoint;
            EndPoint = endPoint;
        }

        public Vector2 StartPoint { get; set; }
        public Vector2 EndPoint { get; set; }
        public Vector2 FirstControlPoint { get; set; }
        public Vector2 SecondControlPoint { get; set; }

        public override bool Equals(object obj)
        {
            var otherCurve = obj as BezierCurveSegment;
            if (otherCurve == null)
                return false;

            return otherCurve.StartPoint.Equals(StartPoint)
                && otherCurve.FirstControlPoint.Equals(FirstControlPoint)
                && otherCurve.SecondControlPoint.Equals(SecondControlPoint)
                && otherCurve.EndPoint.Equals(EndPoint);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
