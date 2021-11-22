﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Models.Path.Generation.Line
{
    public delegate Vector3 CurveEquation(float t);
    public class ParametricCurve
    {
        // Private fields
        protected CurveEquation _equation;



        // Initialization
        public static ParametricCurve Zero => new ParametricCurve(t => Vector3.zero, 0, 0);

        protected ParametricCurve() { }

        public ParametricCurve(CurveEquation equation, float minT, float maxT)
        {
            _equation = equation;
            MinT = minT;
            MaxT = maxT;
        }



        // Properties
        public float MinT { get; protected set; }
        public float MaxT { get; protected set; }
        public Vector3 UpDirection { get; set; } = Vector3.up;
        public Vector3 LastPoint => PointAt(MaxT);



        // Public Methods
        public Vector3 PointAt(float t) => _equation.Invoke(t);
        public Vector3 TangentAt(float t)
        {
            return VelocityAt(t).normalized;
        }

        public Vector3 VelocityAt(float t)
        {
            var dt = GetSmalDeltaT();
            var dx = PointAt(t + dt) - PointAt(t);
            return dx / dt;
        }

        public Vector3 GetNormalAt(float t)
        {
            var v = VelocityAt(t);
            var n = Vector3.Cross(Vector3.up, v).normalized;
            if (n == Vector3.zero)
                n = Vector3.Cross(Vector3.left, v).normalized;
            if (n == Vector3.zero)
                n = Vector3.Cross(Vector3.forward, v).normalized;
            return n;
        }



        // Operators overload
        public static ParametricCurve operator +(ParametricCurve a, ParametricCurve b)
        {
            var minT = a.MinT;
            var maxT = a.MaxT + (b.MaxT - b.MinT);
            CurveEquation equation = t => t < a.MaxT
                ? a.PointAt(t)
                : b.PointAt(t - a.MaxT);
            return new ParametricCurve(equation, minT, maxT);
        }



        // Utils
        private float GetSmalDeltaT()
        {
            return (MaxT - MinT) / 10000;
        }
    }
}
