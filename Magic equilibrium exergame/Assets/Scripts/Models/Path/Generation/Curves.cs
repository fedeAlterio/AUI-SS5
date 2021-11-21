using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Models.Path.Generation
{
    public static class Curves
    {
        public static ParametricCurve Line(Vector3 entry, Vector3 exit)
        {
            CurveEquation equation = t => entry + t * (exit - entry);
            return new ParametricCurve(equation, 0, 1);
        }


        public static ParametricCurve Circle()
        {
            CurveEquation equation = t => new Vector3(-Mathf.Cos(t),0, -Mathf.Sin(t));
            return new ParametricCurve(equation, 0, 2 * Mathf.PI);
        }        
    }
}
