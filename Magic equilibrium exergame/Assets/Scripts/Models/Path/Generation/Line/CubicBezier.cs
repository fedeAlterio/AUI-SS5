using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Models.Path.Generation.Line
{
    public class CubicBezier : ParametricCurve
    {
        // Initialization
        public CubicBezier(Vector3 start, Vector3 end, Vector3 controlPoint1, Vector3 controlPoint2)
        {
            Start = start;
            End = end;
            ControlPoint1 = controlPoint1;
            ControlPoint2 = controlPoint2;
            _equation = Bezier;
            MinT = 0;
            MaxT = 1;
        }




        // Properties
        public Vector3 Start { get; }
        public Vector3 End { get; }
        public Vector3 ControlPoint1 { get; }
        public Vector3 ControlPoint2 { get; }



        // Curve
        private Vector3 Bezier(float t)
        {
            return (1 - t) * (1 - t) * (1 - t) * Start
               + 3 * (1 - t) * (1 - t) * t * ControlPoint1
               + 3 * (1 - t) * t * t * ControlPoint2
               + t * t * t * End;
        }
    }
}
