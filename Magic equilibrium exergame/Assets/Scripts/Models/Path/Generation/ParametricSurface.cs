using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Models.Path.Generation
{
    public delegate Vector3 SurfaceEquation(float u, float v);

    public class ParametricSurface
    {
        // Private fields
        private readonly SurfaceEquation _equation;



        // Initialization
        public ParametricSurface(SurfaceEquation equation, float uMin, float uMax, float vMin, float vMax)
        {
            _equation = equation;
            UMin = uMin;
            UMax = uMax;
            VMin = vMin;
            VMax = vMax;
        }



        // Properties
        public float UMin { get; }
        public float UMax { get; }
        public float VMin { get; }
        public float VMax { get; }



        // Public methods
        public Vector3 PointAt(float u, float v) => _equation(u, v);
    }
}
