using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Models.Path.Generation
{
    public static class Surfaces
    {
        public static ParametricSurface Plane()
        {
            SurfaceEquation equation = (u, v) => new Vector3(u, v,0);
            return new ParametricSurface(equation, -1, 1, -1, 1);
        }

        public static ParametricSurface Sphere()
        {
            SurfaceEquation equation = (u, v) => new Vector3(Mathf.Cos(u) * Mathf.Sin(v), Mathf.Sin(u) * Mathf.Sin(v), Mathf.Cos(v));
            return new ParametricSurface(equation, 0, 2 * Mathf.PI, 0, Mathf.PI);
        }

        public static ParametricSurface Circle()
        {
            SurfaceEquation equation = (u, v) => new Vector3(v*Mathf.Cos(u), v *Mathf.Sin(u), 0);
            return new ParametricSurface(equation, 0, Mathf.PI * 2, 0,1);
        }

        public static ParametricSurface FromCurve(ParametricCurve curve, float thickness)
        {
            Vector3 Equation(float u, float v)
            {
                var center = curve.PointAt(u);
                var normal = curve.GetNormalAt(u);
                return center + normal * v;
            }

            return new ParametricSurface(Equation, curve.MinT, curve.MaxT, -thickness / 2, thickness / 2);
        }
    }
}
