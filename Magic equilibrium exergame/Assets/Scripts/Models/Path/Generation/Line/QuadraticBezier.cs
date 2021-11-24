using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Models.Path.Generation.Line
{
    public class QuadraticBezier : ParametricCurve
    {
        // Private fields
        private readonly Vector3 _start;
        private readonly Vector3 _middle;
        private readonly Vector3 _end;



        // Initialization
        public QuadraticBezier(Vector3 start, Vector3 middle, Vector3 end)
        {
            _start = start;
            _middle = middle;
            _end = end;
            _equation = Bezier;
            MinT = 0;
            MaxT = 1;
        }



        // Curve    
        public override Vector3 TangentAt(float t)
        {
            return -2 * (1 - t) * _start
                + 2 * _middle - 4 * t * _middle
                + 2 * t * _end;
        }

        private Vector3 Bezier(float t)
        {
            return (1 - t) * (1 - t) * _start
                 + 2 * t * (1 - t) * _middle
                 + t * t * _end;
        }
    }
}
