﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Models.Path.Generation.Surface
{
    public delegate bool PiercedSurfaceEquation(float u, float v, out Vector3 point);

    public class PiercedSurface
    {
        // Private fields
        private readonly PiercedSurfaceEquation _equation;



        // Initialization
        public PiercedSurface(PiercedSurfaceEquation equation, float uMin, float uMax, float vMin, float vMax)
        {
            _equation = equation;
            UMin = uMin;
            UMax = uMax;
            VMin = vMin;
            VMax = vMax;
        }

        public static PiercedSurface FromParametricSurface(ParametricSurface surface)
        {
            PiercedSurfaceEquation equation = (float u, float v, out Vector3 point) =>
            {
                point = surface.PointAt(u, v);
                return true;
            };
            return FromParametricSurface(surface, equation);
        }

        public static PiercedSurface FromParametricSurface(ParametricSurface surface, PiercedSurfaceEquation equation)
        {
            return new PiercedSurface(equation, surface.UMin, surface.UMax, surface.VMin, surface.VMax);
        }



        // Properties
        public float UMin { get; }
        public float UMax { get; }
        public float VMin { get; }
        public float VMax { get; }
        public float ULength => UMax - UMin;
        public float VLength => VMax - VMin;


        // Public
        public bool TryGetPointAt(float u, float v, out Vector3 point) => _equation.Invoke(u, v, out point);
    }
}
